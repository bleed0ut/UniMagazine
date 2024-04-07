using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMagazine.Repository;
using UniMagazine.Repository.IRepository;
using UniMagazine.Models;
using NuGet.Protocol.Plugins;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Hosting;
using System.IO.Compression;
using Microsoft.AspNetCore.Identity;

namespace UniMagazine.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AcademicYearController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public AcademicYearController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            var aca = _unitOfWork.AcademicYearRepository.GetAllAcademicYear();
            return View(aca);
        }
        public IActionResult Add() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AcademicYear academicY)
        {
            if (ModelState.IsValid)
            {
                // Validate opened date against closed date
                if (academicY.OpenedDate > academicY.ClosedDate)
                {
                    TempData["Error"] = "Open Date cannot exceed closure date";
                    return View(academicY); // Return to the view with validation error
                }
                var date = DateTime.Now;
                if (date >= academicY.ClosedDate)
                {
                    academicY.Status = "Closed";
                }
                if(date.Year > academicY.OpenedDate.Year)
                {
                    TempData["Error"] = "Can't create academic year that is in the past";
                    return View(academicY);
                }
                else if (date.Year < academicY.OpenedDate.Year)
                {
                    academicY.Status = "Not Started";
                }
                else
                {
                    academicY.Status = "Opening";
                }
                academicY.YearDate = academicY.OpenedDate;
                _unitOfWork.AcademicYearRepository.Add(academicY);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            // If model state is not valid, return to the view with validation error
            return View(academicY);
        }


        public IActionResult Update(int id)
        {
            if (id == null || id == 0)
                return NotFound();
            
            var aca = _unitOfWork.AcademicYearRepository.Get(a => a.Id == id);
            if (aca == null)
                return NotFound();
            return View(aca);
        }

        [HttpPost]
        public IActionResult Update(AcademicYear academicY)
        {
            if (ModelState.IsValid)
            {
                var date = DateTime.Now;
                if (date >= academicY.ClosedDate)
                    academicY.Status = "Closed";
                else if (date.Year >= academicY.OpenedDate.Year)
                {
                    TempData["Error"] = "Can't create academic year that is in the past";
                    return View(academicY);
                }
                else if (date.Year < academicY.OpenedDate.Year)
                    academicY.Status = "Not Started";
                else
                    academicY.Status = "Opening";

                if (academicY == null)
                    return NotFound();

                if (date >= academicY.ClosedDate)
                    academicY.Status = "Closed";
                else
                    academicY.Status = "Opening";
                
                academicY.YearDate = academicY.OpenedDate;
                _unitOfWork.AcademicYearRepository.Update(academicY);
                _unitOfWork.Save();
                TempData["success"] = "Update academic year successfully!";
            }
            
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var academicYear = _unitOfWork.AcademicYearRepository.Get(a => a.Id == id);
            if(academicYear == null)
                return NotFound();
            _unitOfWork.AcademicYearRepository.Delete(academicYear);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DownloadFilesByAcademicYear(int academicYearId)
        {
            // Lấy thông tin về Contribution từ cơ sở dữ liệu
            var magazines = _unitOfWork.MagazineRepository.GetByYear(academicYearId);
            var contributions = _unitOfWork.ContributionRepository.GetByYear(academicYearId);
            // Tạo một tên tệp zip duy nhất bằng cách sử dụng ngày giờ hiện tại
            string zipFileName = $"AcademicYear_{_unitOfWork.AcademicYearRepository.Get(x => x.Id == academicYearId).OpenedDate.ToString("yyyy")}" +
                                 $"_{DateTime.Now.ToString("yyyyMMddHHmmss")}.zip";

            // Tạo thư mục tạm để chứa tất cả các tệp
            string tempFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "TempZip");
            Directory.CreateDirectory(tempFolderPath);

            // Lấy đường dẫn đến tệp zip tạm
            string zipFilePath = Path.Combine(tempFolderPath, zipFileName);
            
            if(contributions.Count() > 0)
            {

                // Tạo tệp zip
                using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                {
                    foreach (var con in contributions)
                    {
                        string facultyName = $"{con.User.Faculty.Name}/";
                        string magazineName = $"Magazine_{con.Magazine.Id}/";
                        string contributionName = $"Contribution_{con.User.Email}_{con.Id}/";

                        if (con.Files.Count() > 0)
                        {
                            foreach (var file in con.Files)
                            {
                                string filePath = _webHostEnvironment.WebRootPath + $"/{file.ImageUrl}";
                                if (System.IO.File.Exists(filePath))
                                {
                                    string entryName = Path.GetFileName(filePath);
                                    zipArchive.CreateEntryFromFile(filePath, facultyName + magazineName + contributionName + entryName);
                                }
                            }
                        }

                    }
                }

                // Đọc tệp zip và trả về nó để tải xuống
                byte[] fileBytes = System.IO.File.ReadAllBytes(zipFilePath);

                // Xóa thư mục tạm và tệp zip sau khi trả về
                Directory.Delete(tempFolderPath, true);

                return File(fileBytes, "application/zip", zipFileName);
            }

            return RedirectToAction("Index");

        }
    }
}

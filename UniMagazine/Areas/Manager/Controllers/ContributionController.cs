using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System;
using System.IO;
using System.IO.Compression;
using UniMagazine.Models;
using UniMagazine.Repository;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class ContributionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ContributionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult DownloadFiles(int contributionId)
        {
            // Lấy thông tin về Contribution từ cơ sở dữ liệu
            var contribution = _unitOfWork.ContributionRepository.Get(contributionId);

            if (contribution == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy Contribution
            }

            // Tạo một tên tệp zip duy nhất bằng cách sử dụng ngày giờ hiện tại
            string zipFileName = $"Contribution_{DateTime.Now.ToString("yyyyMMddHHmmss")}.zip";

            // Tạo thư mục tạm để chứa tất cả các tệp
            string tempFolderPath = Path.Combine(Path.GetTempPath(), "UniMagazine", "TempZip");
            Directory.CreateDirectory(tempFolderPath);

            // Lấy đường dẫn đến tệp zip tạm
            string zipFilePath = Path.Combine(tempFolderPath, zipFileName);

            // Tạo tệp zip từ các tệp của Contribution
            using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                foreach (var file in contribution.Files)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, file.ImageUrl);
                    if (System.IO.File.Exists(filePath))
                    {
                        string entryName = Path.GetFileName(filePath);
                        zipArchive.CreateEntryFromFile(filePath, entryName);
                    }
                }
            }

            // Đọc tệp zip và trả về nó để tải xuống
            byte[] fileBytes = System.IO.File.ReadAllBytes(zipFilePath);

            // Xóa thư mục tạm và tệp zip sau khi trả về
            Directory.Delete(tempFolderPath, true);
            //TempData["success"] = "Download Contribution successfully !";
            return File(fileBytes, "application/zip", zipFileName);
        }

        [HttpGet]
        public IActionResult DownloadFilesByAcademicYear(int academicYearId)
        {
            var contributions = _unitOfWork.ContributionRepository.GetByYear(academicYearId);
            // Tạo một tên tệp zip duy nhất bằng cách sử dụng ngày giờ hiện tại
            string zipFileName = $"AcademicYear_{_unitOfWork.AcademicYearRepository.Get(x => x.Id == academicYearId).OpenedDate.ToString("yyyy")}" +
                                 $"_{DateTime.Now.ToString("yyyyMMddHHmmss")}_She_ride_a_dick_like_a_carnival_Kanye_East.zip";

            // Tạo thư mục tạm để chứa tất cả các tệp
            string tempFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "TempZip");
            Directory.CreateDirectory(tempFolderPath);

            // Lấy đường dẫn đến tệp zip tạm
            string zipFilePath = Path.Combine(tempFolderPath, zipFileName);

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
            TempData["success"] = " Download Files By Academic Year successfully !";
            return File(fileBytes, "application/zip", zipFileName);

        }

        [HttpGet]
        public IActionResult DownloadFilesOfAMagazine(int magazineId)
        {
            var magazine = _unitOfWork.MagazineRepository.Get(magazineId);
            
            if (magazine == null)
                return NotFound();
            if (magazine.Status == "Not Started" ||  magazine.Status == "Not Assigned" || magazine.Status == "Opening")
            {
                TempData["error"] = "You can only download contributions from an ended magazine";
                return RedirectToAction("MagazineDetail", "Home", new { id = magazine.Id, area = "" });
            }




            var contributions = _unitOfWork.ContributionRepository.GetByMagazine(magazineId);
            
            // Tạo một tên tệp zip duy nhất bằng cách sử dụng ngày giờ hiện tại
            string zipFileName = $"{magazine.Faculty.Name}_Magazine_{magazine.Title}_{magazine.PostedDate.ToString("yyyy")}" +
                                 $"_{DateTime.Now.ToString("yyyyMMddHHmmss")}.zip";

            // Tạo thư mục tạm để chứa tất cả các tệp
            string tempFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "TempZip");
            Directory.CreateDirectory(tempFolderPath);

            // Lấy đường dẫn đến tệp zip tạm
            string zipFilePath = Path.Combine(tempFolderPath, zipFileName);

            // Tạo tệp zip
            using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                foreach (var con in contributions)
                {
                    string contributionName = $"Contribution_{con.User.Email}_{con.Id}_{con.CreatedDate.ToString("yyyyMMdd")}/";

                    if (con.Files.Count() > 0)
                    {
                        foreach (var file in con.Files)
                        {
                            string filePath = _webHostEnvironment.WebRootPath + $"/{file.ImageUrl}";
                            if (System.IO.File.Exists(filePath))
                            {
                                string entryName = Path.GetFileName(filePath);
                                zipArchive.CreateEntryFromFile(filePath, contributionName + entryName);
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
    }
}

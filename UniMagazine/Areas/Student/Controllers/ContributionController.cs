using GroupDocs.Viewer.Options;
using GroupDocs.Viewer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using UniMagazine.Models;
using UniMagazine.Models.ViewModels;
using UniMagazine.Repository;
using UniMagazine.Repository.IRepository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UniMagazine.Utility;

namespace UniMagazine.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class ContributionController : Controller
    {

        private IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public ContributionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Add(int id)
        {
            var magazine = _unitOfWork.MagazineRepository.Get(x => x.Id == id);

            if(magazine.Status == "Closed")
            {
                TempData["error"] = "You cannot add a contribution, this magazine has ended!";
                return RedirectToAction("MagazineDetail", "Home", new { id = id, area = "" });
            }

            Contribution con = new Contribution();
            con.MagazineId = id;
            con.Magazine = magazine;
            
            return View(con);
        }
        [HttpPost]
        public async Task<IActionResult> Add(Contribution con, List<IFormFile> files)
        {
            var magazine = _unitOfWork.MagazineRepository.Get(x => x.Id == con.MagazineId);

            ViewData["MagazineTitle"] = magazine.Title;
            ViewData["MagazineDescription"] = magazine.Detail;
            ViewData["MagazineStatus"] = magazine.Status;
            ViewData["PostedDate"] = magazine.PostedDate.ToString();
            ViewData["ImgUrl"] = magazine.ImageUrl;

            if (ModelState.IsValid)
            {
                if (con != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                    var contribution = new Contribution()
                    {
                        Content = con.Content,
                        Status = "Pending",
                        UserId = currentUser.Id,
                        MagazineId = con.MagazineId,
                        // Assuming Contribution has a UserId property
                    };

                    _unitOfWork.ContributionRepository.Add(contribution);
                    _unitOfWork.Save(); // Save Contribution entity to generate Id

                    foreach (var file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(wwwRootPath, @"upload\Student\");

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        using (var fileStream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        // Save file info to the database
                        var MaCon = new MaterialContribution()
                        {
                            CreatedDate = DateTime.Now,
                            ImageUrl = @"upload\Student\" + fileName,
                            ContributionId = contribution.Id // Set ContributionId with the generated Id of Contribution entity
                        };
                        _unitOfWork.MaterialContributionRepository.Add(MaCon);
                    }
                    TempData["success"] = "Request successfully! Your contribution is pending!";
                    _unitOfWork.Save();

                    contribution.User = _unitOfWork.UserRepository.GetUserById(currentUser.Id);
                    contribution.Magazine = _unitOfWork.MagazineRepository.Get(x => x.Id == con.MagazineId);
                    //send mail to contribution
                    var coordinators = _unitOfWork.UserRepository.GetCoordinators(contribution.User.FacultyId);
                    if(coordinators.Count() > 0)
                        _emailSender.AnounceSubmission(coordinators, contribution);

                    // Save MaterialContribution entities
                    return RedirectToAction("MagazineDetail", "Home", new { id = con.MagazineId, area = "" });
                }
            }
            return View(con);
        }

        public IActionResult Detail(int id)
        {
            var contribution = _unitOfWork.ContributionRepository.Get(id);
            var material = _unitOfWork.MaterialContributionRepository.GetMaterial(id);

            var ContriMa = new ConMaVM()
            {
                Contribution = contribution,
                MaterialContribution = material,
            };
            if (material == null)
            {
                TempData["Error"] = "null";
            }
            return View(ContriMa);
            
        }
        public IActionResult AddMaterial(int id)
        {
            var contribution = _unitOfWork.ContributionRepository.Get(id);
            Contribution cpn = new Contribution()
            {
                Status = contribution.Status,
                Content = contribution.Content,
                UserId = contribution.UserId,
                MagazineId = contribution.MagazineId,
            };
            return View(cpn);

        }
        [HttpPost]
        public IActionResult AddMaterial(Contribution con, List<IFormFile> files)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            _unitOfWork.ContributionRepository.Update(con);
            _unitOfWork.Save();
            if (ModelState.IsValid)
            {
                if (con.Status != null)
                {
                    foreach (var file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(wwwRootPath, @"upload\Student");
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        using (var fileStream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        // Save file info to the database
                        if (_unitOfWork != null && _unitOfWork.MaterialContributionRepository != null)
                        {
                            var MaCon = new MaterialContribution()
                            {
                                CreatedDate = DateTime.Now,
                                ImageUrl = @"upload\Student\" + fileName,
                                ContributionId = con.Id // Set ContributionId with the generated Id of Contribution entity
                            };
                            _unitOfWork.MaterialContributionRepository.Add(MaCon);
                            _unitOfWork.Save();

                        }
                    }
                }
                return RedirectToAction("Detail", "Contribution", new { id = con.Id, area = "Student" });
            }
            
            _unitOfWork.ContributionRepository.Update(con);
            _unitOfWork.Save();
            var contribution = _unitOfWork.ContributionRepository.Get(con.Id);

            return View(contribution);

        }
        //public IActionResult ViewFile()
        //{
        //    var filePath = "C:\\Users\\PC\\source\\repos\\UniMagazine\\UniMagazine\\wwwroot\\upload\\Student\\3ac8ad29-c241-4c68-a9f6-309b751707b3_A1 project.docx";
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return NotFound();
        //    }

        //    // Get the file extension
        //    string fileExtension = Path.GetExtension(filePath);

        //    // Set the appropriate content type based on the file extension
        //    string contentType;
        //    switch (fileExtension)
        //    {
        //        case ".pdf":
        //            contentType = "application/pdf";
        //            break;
        //        case ".txt":
        //            contentType = "text/plain";
        //            break;
        //        // Add more cases for other file types if needed
        //        default:
        //            contentType = "application/octet-stream";
        //            break;
        //    }

        //    // Read the file into a byte array
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

        //    // Return the file content to the browser
        //    return File(fileBytes, contentType, Path.GetFileName(filePath));
        //}




        public IActionResult MyContribution(string? status = "", string? search = "")
        {
            var userId = _userManager.GetUserId(this.User);

            var myContributions = _unitOfWork.ContributionRepository.GetMyContribution(userId, status, search);

            MyContributionVM mcVM = new MyContributionVM()
            {
                Contributions = myContributions,
                Status = status,
                Search = search
            };

            return View(mcVM);
        }

    }
}

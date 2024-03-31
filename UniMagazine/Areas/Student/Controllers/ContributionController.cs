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

namespace UniMagazine.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class ContributionController : Controller
    {

        private IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public ContributionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            return View();
        }
        public async Task<IActionResult> Add(int id)
        {

            Contribution con = new Contribution();
            con.MagazineId = id;
            var magazine = _unitOfWork.MagazineRepository.Get(x => x.Id == id);

            ViewData["MagazineTitle"] = magazine.Title;
            ViewData["MagazineDescription"] = magazine.Detail;
            ViewData["MagazineStatus"] = magazine.Status;
            ViewData["PostedDate"] = magazine.PostedDate.ToString();
            ViewData["ImgUrl"] = magazine.ImageUrl;

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
                    var conTri = new Contribution()
                    {
                        Content = con.Content,
                        Status = "Pending",
                        UserId = currentUser.Id,
                        MagazineId = con.MagazineId,
                        // Assuming Contribution has a UserId property

                    };
                    _unitOfWork.ContributionRepository.Add(conTri);
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
                        if (_unitOfWork != null && _unitOfWork.MaterialContributionRepository != null)
                        {
                            var MaCon = new MaterialContribution()
                            {
                                CreatedDate = DateTime.Now,
                                ImageUrl = @"upload\Student\" + fileName,
                                ContributionId = conTri.Id // Set ContributionId with the generated Id of Contribution entity
                            };
                            _unitOfWork.MaterialContributionRepository.Add(MaCon);
                        }
                    }
                    TempData["success"] = "Request successfully! Your contribution is pending!";
                    _unitOfWork.Save();
                    // Save MaterialContribution entities
                    return RedirectToAction("MagazineDetail", "Home", new { id = con.MagazineId, area = "" });
                }
            }
            return View(con);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var contribution = _unitOfWork.ContributionRepository.Get(id);
            return View(contribution);
        }

    }
}

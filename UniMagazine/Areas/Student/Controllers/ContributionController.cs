using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using UniMagazine.Models;
using UniMagazine.Models.ViewModels;
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

            return View(con);
        }
        [HttpPost]
        public async Task<IActionResult> Add(Contribution con, List<IFormFile> files)
        {
            var con1 = _unitOfWork.ContributionRepository.GetAll();
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
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
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
                            ImageUrl = @"\upload\Magazine\" + fileName,
                            ContributionId = conTri.Id // Set ContributionId with the generated Id of Contribution entity
                        };
                        _unitOfWork.MaterialContributionRepository.Add(MaCon);
                    }
                }
                
                var con2 = new List<Contribution>();
                foreach(var c in con1)
                {
                    if (c.Id == con.MagazineId)
                    {
                        con2.Add(c);
                    }
                }
                var x = new MaConVM()
                {
                    Magazine = _unitOfWork.MagazineRepository.Get(x => x.Id == con.MagazineId),
                    Contributions = con2
                };
                _unitOfWork.Save();
                // Save MaterialContribution entities
                return RedirectToAction("MagazineDetail", "Home", new { id = con.MagazineId, area = "" });
            }
            return View(con);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var con = _unitOfWork.ContributionRepository.Get(x => x.Id == id);
            var Material = _unitOfWork.MaterialContributionRepository.GetAll();
            var MaterialCon = new List<MaterialContribution>();
            foreach (var c in Material)
            {
                if (c.ContributionId == id)
                {
                    MaterialCon.Add(c);
                }
            }

            ConMaVM conMaVM = new ConMaVM()
            {
                Contribution = con,
                MaterialContribution = MaterialCon
            };
            
            return View(conMaVM);
        }


    }
}

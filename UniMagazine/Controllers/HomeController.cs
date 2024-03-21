using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.NetworkInformation;
using UniMagazine.Models;
using UniMagazine.Models.ViewModels;
using UniMagazine.Repository;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index(string? status = "")
        {
            var magazines = _unitOfWork.MagazineRepository.GetActiveMagazines(0, status);
            var userId = _userManager.GetUserId(this.User);

            if (userId != null) {
                var user = _unitOfWork.UserRepository.GetUserById(userId);
                magazines = _unitOfWork.MagazineRepository.GetActiveMagazines(user.FacultyId, status);
                return View(magazines);
            }

            
            return View(magazines);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult MagazineDetail(int id)
        {
            var userId = _userManager.GetUserId(this.User);
            var user = _unitOfWork.UserRepository.GetUserById(userId);
            var ma = _unitOfWork.MagazineRepository.Get(x => x.Id == id);
            var con = _unitOfWork.ContributionRepository.GetAll();
            var contri = new List<Contribution>();
            var contri2 = new List<Contribution>();
            foreach (var x in con)
            {
                if (x.MagazineId == id && x.Status == "Published")
                {
                    contri2.Add(x);
                    contri.Add(x);
                }
                if(userId != null)
                {
                    if (x.UserId == user.Id)
                    {
                        contri.Add(x);
                    }
                }
                
            }
            foreach (var x in contri)
            {

                x.User = _unitOfWork.UserRepository.GetUserById(x.UserId);
            }
            MaConVM magazineVM = new MaConVM()
            {
                Magazine = ma,
                Contributions = contri
            };
            if (userId == null)
            {
                MaConVM magazineVM2 = new MaConVM()
                {
                    Magazine = ma,
                    Contributions = contri2
                };
                return View(magazineVM2);
            }
            return View(magazineVM);

        }

    }
}

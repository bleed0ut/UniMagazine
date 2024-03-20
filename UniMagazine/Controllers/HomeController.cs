using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UniMagazine.Models;
using UniMagazine.Models.ViewModels;
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

        public IActionResult Index(string status = "")
        {
            var maga = _unitOfWork.MagazineRepository.GetAll();
            foreach (var mag in maga)
            {
                _unitOfWork.MagazineRepository.UpdateStatus(mag);
            }
            _unitOfWork.Save();

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
    }
}

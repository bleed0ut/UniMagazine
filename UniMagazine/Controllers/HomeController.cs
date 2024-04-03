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

        public IActionResult Index(MagazineFilterVM magaFilterVM, string? status = "", string? search = "", int academicYearId = 0, int facultyId = 0)
        {
           var maga = _unitOfWork.MagazineRepository.GetActiveMagazines(0, status, 0, search);
            
            _unitOfWork.MagazineRepository.UpdateStatusMany(maga);

            _unitOfWork.Save();

            maga = _unitOfWork.MagazineRepository.GetActiveMagazines(facultyId, status, academicYearId, search);
            
            var userId = _userManager.GetUserId(this.User);
            ViewBag.UserId = userId;


            magaFilterVM.Magazines = maga;
            if(!string.IsNullOrEmpty(magaFilterVM.Status))
                magaFilterVM.StatusTemp = magaFilterVM.Status;
            magaFilterVM.Status = status;
            magaFilterVM.AcademicYearId = academicYearId;
            magaFilterVM.FacultyId = facultyId;
            magaFilterVM.Search = search;
            magaFilterVM.AcademicYearsDisplay = _unitOfWork.AcademicYearRepository.GetAllAcademicYear();
            magaFilterVM.FacultiesDisplay = _unitOfWork.FacultyRepository.GetAllFaculty();
            

            if (userId != null && !this.User.IsInRole("Manager")) {
                var user = _unitOfWork.UserRepository.GetUserById(userId);
                magaFilterVM.Magazines = _unitOfWork.MagazineRepository.GetActiveMagazines(user.FacultyId, status, academicYearId, search);
                return View(magaFilterVM);
            }

            
            return View(magaFilterVM);
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
        public IActionResult MagazineDetail(int id, string? search = "", string? userId = "")
        {
            var ma = _unitOfWork.MagazineRepository.Get(x => x.Id == id);

            _unitOfWork.MagazineRepository.Update(ma);
            _unitOfWork.Save();
            ma = _unitOfWork.MagazineRepository.Get(x => x.Id == id);


            var contributions = _unitOfWork.ContributionRepository.GetAllPublishedContribution(id, search, userId);
            MagazineDetailVM magazineVM = new MagazineDetailVM()
            {
                Magazine = ma,
                Contributions = contributions,
                Search = search
            };

            return View(magazineVM);
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stimulsoft.Report.Components;
using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class DashController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _dbContext;
        public DashController(AppDbContext dbContext, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public List<object> AllConInFa(string? year)
        {
            List<object> data = new List<object>();
            data = _unitOfWork.DashRepository.GetAllConInAllFaByAca(year);
            return data;

        }
        public List<object> PercentageByFa(string? year)
        {
            List<object> data = new List<object>();
            data = _unitOfWork.DashRepository.GetPerConInAllFaByAca(year);
            return data;
        }
        [HttpGet]
        public IActionResult GetAcademicYears()
        {
            List<AcademicYear> academicYears = _unitOfWork.AcademicYearRepository.GetAll().ToList();
            return Json(academicYears.Select(year => new { Id = year.Id, Text = year.YearDate.ToString("yyyy") }));
        }

    }
}

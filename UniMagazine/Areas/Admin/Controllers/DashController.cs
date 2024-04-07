using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stimulsoft.Report.Components;
using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public List<object> AllConInFa()
        {
            return _unitOfWork.DashRepository.GetAllConInAllFaByAca();
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniMagazine.Data;
using UniMagazine.Repository;
using UniMagazine.Repository.IRepository;
using UniMagazine.Models.ViewModels;

namespace UniMagazine.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _Unit;
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUnitOfWork unit, AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _Unit = unit;
             _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        
    }
}

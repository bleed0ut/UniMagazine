using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniMagazine.Repository;
using UniMagazine.Repository.IRepository;
using UniMagazine.Models.ViewModels;
using UniMagazine.Models;

namespace UniMagazine.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult UserIndex(string search = "", string role = "")
        {
            IEnumerable<ApplicationUser> users = _unitOfWork.UserRepository.GetAllUser(search, role);
            
            UserVM userVM = new UserVM() { 
                Users = users,
                SearchByEmail = search,
                Role = role
            };

            return View(userVM);
        }
    }
}

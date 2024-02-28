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
        public IActionResult RoleManagement(string userId)
        {
            // Ensure userId is not null or empty
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is missing.");
            }

            // Retrieve the user's current role 
            var userRole = _db.UserRoles.FirstOrDefault(u => u.UserId == userId);
            string roleId = userRole?.RoleId;

            // Ensure the user has a role
            if (roleId == null)
            {
                return NotFound("User not found or does not have a role.");
            }

            // Create a new RoleManagementVM
            RoleManageVM roleVM = new RoleManageVM()
            {
                ApplicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId),
                RoleList = _db.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name,
                    Selected = i.Id == roleId // Select the current user role in the dropdown
                }),
            };

            // Set the current role name in ApplicationUser
            roleVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(u => u.Id == roleId)?.Name;
            return View(roleVM);

            
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                List<ApplicationUser> objUserList = _db.ApplicationUsers.ToList();
                var userRoles = _db.UserRoles.ToList();
                var roles = _db.Roles.ToList();

                foreach (var user in objUserList)
                {
                    var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id)?.RoleId;
                    var roleName = roles.FirstOrDefault(u => u.Id == roleId)?.Name;
                    user.Role = roleName;
                }

                return Json(new { data = objUserList });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return BadRequest("An error occurred while retrieving user data.");
            }
        }
        [HttpPost]
        public IActionResult RoleManagement(RoleManageVM roleManagementVM)
        {
            // Retrieve the current role of the user
            string userId = roleManagementVM.ApplicationUser.Id;
            string currentRoleId = _db.UserRoles.FirstOrDefault(u => u.UserId == userId)?.RoleId;
            string currentRole = _db.Roles.FirstOrDefault(u => u.Id == currentRoleId)?.Name;

            if (currentRole != null && !roleManagementVM.ApplicationUser.Role.Equals(currentRole))
            {
                // A role was updated
                ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);

                // Update the user's role in the database
                _userManager.RemoveFromRoleAsync(applicationUser, currentRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }



            return RedirectToAction("Index");
        }
        #endregion
    }
}

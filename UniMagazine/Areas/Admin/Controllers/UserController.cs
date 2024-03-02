using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniMagazine.Repository;
using UniMagazine.Repository.IRepository;
using UniMagazine.Models.ViewModels;
using UniMagazine.Models;
using System.Data;

namespace UniMagazine.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult UserIndex(string search = "", string role = "", int facultyId = 0)
        {
            IEnumerable<ApplicationUser> users = _unitOfWork.UserRepository.GetAllUser(search, role, facultyId);
            
            UserVM userVM = new UserVM() { 
                Users = users,
                SearchByEmail = search,
                Role = role,
                Faculties = _unitOfWork.FacultyRepository.GetAllFaculty(),
                FacultyId = facultyId
            };

            return View(userVM);
        }

        public IActionResult CreateAccount()
        {
            RegisterModel rm = new RegisterModel()
            {
                Faculties = _unitOfWork.FacultyRepository.GetAllFaculty().Select(f => new SelectListItem
                {
                    Text = f.Name,
                    Value = f.Id.ToString()
                }),

                Roles = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
            };
           
            return View(rm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(RegisterModel rm)
        {
            
            if (ModelState.IsValid && await _userManager.FindByEmailAsync(rm.Email) == null)
            {
                var user = new ApplicationUser()
                {
                    FullName = rm.FullName,
                    DateOfBirth = rm.DateOfBirth,
                    Email = rm.Email,
                    UserName = rm.Email,
                    Address = rm.Address,
                    FacultyId = rm.FacultyId,
                    Role = rm.Role,
                    PhoneNumberConfirmed = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                await _userManager.CreateAsync(user, rm.Password);
                await _unitOfWork.SaveAsync();
                await _userManager.AddToRoleAsync(user, rm.Role);

                return RedirectToAction("UserIndex");
            }
            else
            {
                rm.Faculties = _unitOfWork.FacultyRepository.GetAllFaculty().Select(f => new SelectListItem
                {
                    Text = f.Name,
                    Value = f.Id.ToString()
                });

                rm.Roles = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                });
            }
            return View(rm);
        }
    }
}

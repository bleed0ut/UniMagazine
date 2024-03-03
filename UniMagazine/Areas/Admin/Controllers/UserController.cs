using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniMagazine.Repository;
using UniMagazine.Repository.IRepository;
using UniMagazine.Models.ViewModels;
using UniMagazine.Models;
using System.Data;
using Microsoft.DiaSymReader;

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

            UserVM userVM = new UserVM()
            {
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

        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            UpdateUserModel um = new UpdateUserModel()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                DateOfBirth = user.DateOfBirth,
                Address = user.Address,
                FacultyId = user.FacultyId,
                Faculty = _unitOfWork.FacultyRepository.Get(user.FacultyId),

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


            return View(um);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserModel um)
        {
            var user = await _userManager.FindByIdAsync(um.Id);
            if (user == null) return NotFound();

            user.FullName = um.FullName;
            user.Address = um.Address;
            user.DateOfBirth = um.DateOfBirth;
            user.FacultyId = um.FacultyId;

            if (user.Role != um.Role)
            {
                user.Role = um.Role;
                await _userManager.AddToRoleAsync(user, user.Role);
            }

            await _userManager.UpdateAsync(user);
            return RedirectToAction("UserIndex");
        }

        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            ResetPasswordVM rvm = new ResetPasswordVM()
            {
                Email = user.Email,
                Password = user.PasswordHash,
                ConfirmPassword = user.PasswordHash
            };
            return View(rvm);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM rvm)
        {

            var email = Request.Form["email"];
            var password = Request.Form["password"];
            var user = await _userManager.FindByEmailAsync(email);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (user == null)
                return NotFound();
            var result = await _userManager.ResetPasswordAsync(user, code, password);
            if (result.Succeeded)
            {
                TempData["success"] = $"Password reset successfully! for (Email:  {user.Email})";
                return RedirectToAction("UserIndex");
            }

            //error
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            rvm = new ResetPasswordVM()
            {
                Email = user.Email,
                Password = user.PasswordHash,
                ConfirmPassword = user.PasswordHash,
                Code = user.Id
            };
            TempData["error"] = "Something wrong!";
            return View(rvm);
        }
    }
}
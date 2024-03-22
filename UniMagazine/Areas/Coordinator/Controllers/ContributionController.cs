using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniMagazine.Models;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]
    [Authorize(Roles = "Coordinator")]
    public class ContributionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _userManager;

        public ContributionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult ContributionModeration()
        {
            var userId = _userManager.GetUserId(this.User);
            var user = _unitOfWork.UserRepository.GetUserById(userId);

            var pendingContributions = _unitOfWork.ContributionRepository.GetAllPendingContribution(user.FacultyId);

            return View(pendingContributions);
        }

        public IActionResult Detail(int id)
        {
            return View();
        }
    }
}

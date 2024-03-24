using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniMagazine.Models;
using UniMagazine.Models.ViewModels;
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

        [HttpGet]
        public IActionResult Detail(int id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var con = _unitOfWork.ContributionRepository.Get(x=> x.Id == id);
            if (con == null)
            {
                return NotFound();
            }
            var feedbackVM = new FeedBackVM()
            {
                Id = id,
                Contribution = con
            };
            return View(feedbackVM);
        }

        [HttpPost]
        public IActionResult Detail(FeedBackVM feedbackVM)
        {
            var con = _unitOfWork.ContributionRepository.Get(x=> x.Id == feedbackVM.Id);
            if (con == null)
            {
                return NotFound();
            }
            con.Status = feedbackVM.Status;
            _unitOfWork.Save();

            var feedback = new FeedbackComment()
            {
                Comment = feedbackVM.Comment,
                ContributionID = feedbackVM.Id,
                UserID = _userManager.GetUserId(this.User),
                Status = feedbackVM.Status,
            };

            _unitOfWork.FeedBackCommentRepository.Add(feedback);
            _unitOfWork.Save();

            return RedirectToAction("ContributionModeration");
        }
    }
}


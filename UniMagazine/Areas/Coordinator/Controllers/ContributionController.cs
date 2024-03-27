using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniMagazine.Models;
using UniMagazine.Models.ViewModels;
using UniMagazine.Repository.IRepository;
using UniMagazine.Utility;

namespace UniMagazine.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]
    [Authorize(Roles = "Coordinator")]
    public class ContributionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ContributionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public IActionResult ContributionModeration(string? searchT ="", string? searchC ="")
        {
            var userId = _userManager.GetUserId(this.User);
            var user = _unitOfWork.UserRepository.GetUserById(userId);

            PendingContributionVM pcVM = new PendingContributionVM()
            { 
                Contributions = _unitOfWork.ContributionRepository.GetAllPendingContribution(user.FacultyId, searchT, searchC),
                SearchByTitle = searchT,
                SearchByContributorEmail = searchC
        };

            return View(pcVM);
        }

        [HttpGet]
        public IActionResult GiveFeedback(int id)
        {
            if (id == null || id == 0)
                return NotFound();
            
            var con = _unitOfWork.ContributionRepository.Get(id);
            if (con == null)
                return NotFound();
            
            var feedbackVM = new FeedBackVM()
            {
                Id = id,
                Contribution = con
            };
            return View(feedbackVM);
        }

        [HttpPost]
        public IActionResult GiveFeedback(FeedBackVM feedbackVM, string? status)
        {
            var con = _unitOfWork.ContributionRepository.Get(feedbackVM.Id);
            if (con == null)
                return NotFound();
            
            con.Status = status;
            _unitOfWork.Save();

            var feedback = new FeedbackComment()
            {
                Comment = feedbackVM.Comment,
                ContributionID = feedbackVM.Id,
                UserID = _userManager.GetUserId(this.User),
                Status = status,
            };

            _unitOfWork.FeedBackCommentRepository.Add(feedback);
            _unitOfWork.Save();

            TempData["succes"] = "Give feedback successfully!. An email notification will be sent to this student";
            _emailSender.SendFeedBackEmail(con,feedback);
            
            return RedirectToAction("ContributionModeration");
        }
    }
}


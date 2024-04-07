using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ContributionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult ContributionModeration(string? status = "", string? searchT ="", string? searchC ="")
        {
            var userId = _userManager.GetUserId(this.User);
            var user = _unitOfWork.UserRepository.GetUserById(userId);

            var contributions = _unitOfWork.ContributionRepository.GetAllPendingContribution(user.FacultyId, searchT, searchC, status);
            _unitOfWork.ContributionRepository.CheckManyPendingContribution(contributions);
            _unitOfWork.Save();

            PendingContributionVM pcVM = new PendingContributionVM()
            { 
                Contributions = _unitOfWork.ContributionRepository.GetAllPendingContribution(user.FacultyId, searchT, searchC, status),
                SearchByTitle = searchT,
                SearchByContributorEmail = searchC,
                Status = status
            };

            return View(pcVM);
        }

        [HttpGet]
        public IActionResult GiveFeedback(int id)
        {
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

            if(con.Magazine.Status == "Closed" && con.Status == "Pending")
            {
                feedbackVM.Contribution = con;
                TempData["error"] = "Cannot moderate this contribution, due to the Magazine has been ended!";
                return View(feedbackVM);
            }
                        
            DateTime today = DateTime.Now;
            TimeSpan ts = today - con.CreatedDate;

            if (ts.Days >= 14)
            { 
                feedbackVM.Contribution = con;
                TempData["error"] = "Cannot give a feedback comment.This submission been expired in 14 days.";
                return View(feedbackVM);
            }
            //rejected/publish for pending or published for pendingupdate
            if(status == "Published")
            {
                if (con.Status == "PendingUpdate")
                {
                    _unitOfWork.MaterialContributionRepository.PublishFileStatusForUpdating(con.Id);
                }

                con.Status = status;
                con.UpdatedDate = today;
            }

            if (status == "Rejected")
            {
                if (con.Status == "PendingUpdate")
                {
                    con.Status = "Published";
                    con.Content = con.TempContent;
                    _unitOfWork.MaterialContributionRepository.DeleteFileForRejectUpdating(con.Id, _webHostEnvironment.WebRootPath);
                }
                else //Pending for resubmission or first new submission
                    con.Status = status;
            }


            var feedback = new FeedbackComment()
            {
                Comment = feedbackVM.Comment,
                ContributionID = feedbackVM.Id,
                UserID = _userManager.GetUserId(this.User),
                Status = status,
            };


            _unitOfWork.FeedBackCommentRepository.Add(feedback);

            _unitOfWork.Save();

            TempData["success"] = "Give feedback successfully!. An email notification will be sent to this student";
            _emailSender.SendFeedBackEmail(con, feedback);

            return RedirectToAction("ContributionModeration");
        }
    }
}


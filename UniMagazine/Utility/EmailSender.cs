using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using UniMagazine.Models;
using UniMagazine.Repository;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Utility
{
    public class EmailSender : IEmailSender
    {
        private string sender = "chubedan6424@gmail.com";
        private string pwd = "wwhvsauojsiwthdn";
        private UserManager<ApplicationUser> _userManager;

        public EmailSender( UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public void SendRegistrationEmail(ApplicationUser user, string userPwd)
        {
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(sender);
            mm.To.Add(user.Email);

            mm.Subject = "Online Registration Information";
            mm.IsBodyHtml = true;

            string content = $"<h1>Account Information</h1>";
            content += $"<p>Dear {user.Role},</p>";
            content += "<p><i>Please using email and password to login!</i></p>";
            content += $"<p><strong>Email: </strong>{user.Email}</p>";
            content += $"<p><strong>Password: </strong>{userPwd}</p>";
            content += "<p><i> The personal information</i></p>";
            content += $"<p><strong>Full Name: </strong>{user.FullName}</p>";
            content += $"<p><strong>Date of birth: </strong>{user.DateOfBirth.ToString("MM-dd-yyyy")}</p>";
            content += $"<p><strong>Home Address: </strong>{user.Address}</p>";

            mm.Body = content;

            SmtpClient smtp = GetSmtpClient();

            smtp.Send(mm);
        }

        public void SendFeedBackEmail(Contribution con,FeedbackComment feedback)
        {
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(sender);


            mm.To.Add(con.User.Email);

            mm.Subject = "Feedback of your submisstion";
            mm.IsBodyHtml = true;

            string content = $"<h1>About your Contributionn</h1>";
            if (feedback.Status == "Published")
                content += $"<p>Your contribution ha been published! Please check</p>";
            else
                content += $"<p>Your contribution ha been rejected! Please check</p>";

            content += $"<p><strong>Topic Magazine: {con.Magazine.Title}</strong></p>";
            content += $"<p>{con.Content}</p>";
            content += $"<p>Coordinator has give u a feedback:<i>>{feedback.Comment}</i></p>";
           

            mm.Body = content;

            SmtpClient smtp = GetSmtpClient();

            smtp.Send(mm);
        }

        public void AnounceSubmission(IEnumerable<ApplicationUser> coordinators, Contribution contribution)
        {
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(sender);

            mm.IsBodyHtml = true;

            mm.Subject = "New pending submissions";

            string content = $"<h1>New pending submissions is waiting for you</h1>";
            content += $"<p>Student {contribution.User.FullName}<i>({contribution.User.Email})</i> has just submitted a contribution at {contribution.Magazine.Title}</p>";
            mm.Body= content;

            SmtpClient smtp = GetSmtpClient();
            foreach (ApplicationUser coordinator in coordinators)
            {
                mm.To.Add(coordinator.Email);
                smtp.Send(mm);
            }
        }

        private SmtpClient GetSmtpClient()
        {
            return new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sender, pwd),
            };
        }
    }
}

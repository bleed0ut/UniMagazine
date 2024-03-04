using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using UniMagazine.Models;
using UniMagazine.Repository;

namespace UniMagazine.Utility
{
    public class EmailSender : IEmailSender
    {
        private string sender = "chubedan6424@gmail.com";
        private string pwd = "wwhvsauojsiwthdn";


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

            SmtpClient smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sender, pwd),
            };

            smtp.Send(mm);
        }
    }
}

using UniMagazine.Models;

namespace UniMagazine.Utility
{
    public interface IEmailSender
    {
        public void SendRegistrationEmail(ApplicationUser user, string userPwd);
        public void SendFeedBackEmail(Contribution con, FeedbackComment feedback);
    }
}

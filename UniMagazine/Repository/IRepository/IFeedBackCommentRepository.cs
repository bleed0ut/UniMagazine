using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IFeedBackCommentRepository : IRepository<FeedbackComment>
    {
        void Update(FeedbackComment feedBackComment);
    }
}

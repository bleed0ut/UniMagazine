using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class FeedbackCommentRepository : Repository<FeedbackComment>, IFeedBackCommentRepository
    {
        private readonly AppDbContext _dbContext;

        public FeedbackCommentRepository(AppDbContext dbContext) : base(dbContext) { 
            _dbContext = dbContext;
        }

        public void Update(FeedbackComment feedBackComment)
        {
            _dbContext.Update(feedBackComment);
        }
    }
}

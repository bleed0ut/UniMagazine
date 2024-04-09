using UniMagazine.Data;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IUserRepository UserRepository { get; private set; }
        public IFacultyRepository FacultyRepository { get; private set; }
        public IContributionRepository ContributionRepository { get; private set; }
        public IMaterialContributionRepository  MaterialContributionRepository { get; private set; }    
        public IAcademicYearRepository AcademicYearRepository { get; private set; }

        public IMagazineRepository MagazineRepository { get; private set; }
        public IDashRepository DashRepository { get; private set; } 

        public IFeedBackCommentRepository FeedBackCommentRepository { get; private set; }

        public UnitOfWork(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            DashRepository = new DashRepository(dbContext);
            MagazineRepository = new MagazineRepository(dbContext, webHostEnvironment);
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            MaterialContributionRepository = new MaterialContributionRepository(dbContext);
            ContributionRepository = new ContributionRepository(dbContext);
            UserRepository = new UserRepository(dbContext);
            FacultyRepository = new FacultyRepository(dbContext);
            AcademicYearRepository = new AcademicYearRepository(dbContext, webHostEnvironment);
            FeedBackCommentRepository = new FeedbackCommentRepository(dbContext);
        }

        public void Save()
        {
           _dbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

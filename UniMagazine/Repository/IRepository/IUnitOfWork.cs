namespace UniMagazine.Repository.IRepository
{
    public interface IUnitOfWork
    {
        //IABCRepository
        public IAcademicYearRepository AcademicYearRepository { get; }
        public IUserRepository UserRepository { get; }
        public IContributionRepository ContributionRepository { get; }
        public IMaterialContributionRepository MaterialContributionRepository { get; }
        public IFacultyRepository FacultyRepository { get; }
        public IMagazineRepository MagazineRepository { get; }
        public IFeedBackCommentRepository FeedBackCommentRepository { get; }

        public void Save();

        public Task SaveAsync();
    }
}

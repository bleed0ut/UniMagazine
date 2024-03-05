namespace UniMagazine.Repository.IRepository
{
    public interface IUnitOfWork
    {
        //IABCRepository
        public IAcademicYearRepository AcademicYearRepository { get; }
        public IUserRepository UserRepository { get; }
        public IFacultyRepository FacultyRepository { get; }

        public void Save();

        public Task SaveAsync();
    }
}

using UniMagazine.Data;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public IUserRepository UserRepository { get; private set; }
        public IFacultyRepository FacultyRepository { get; private set; }

        public IAcademicYearRepository AcademicYearRepository { get; private set; }

        public IMagazineRepository MagazineRepository { get; private set; }

        public UnitOfWork(AppDbContext dbContext)
        {
            MagazineRepository = new MagazineRepository(dbContext);
            _dbContext = dbContext;
            UserRepository = new UserRepository(dbContext);
            FacultyRepository = new FacultyRepository(dbContext);
            AcademicYearRepository = new AcademicYearRepository(dbContext);
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

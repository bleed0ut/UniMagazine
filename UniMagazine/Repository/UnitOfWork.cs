using UniMagazine.Data;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public IUserRepository UserRepository { get; private set; }


        public UnitOfWork(AppDbContext dbContext)
        { 
            _dbContext = dbContext;
            UserRepository = new UserRepository(dbContext);
        }

        public void Save()
        {
           _dbContext.SaveChanges();
        }
    }
}

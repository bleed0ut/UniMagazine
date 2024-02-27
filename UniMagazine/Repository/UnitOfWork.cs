using UniMagazine.Data;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _dbContext {  get; set; }

        public UnitOfWork(AppDbContext dbContext)
        { 
            _dbContext = dbContext;
        }

        public void Save()
        {
           _dbContext.SaveChanges();
        }
    }
}

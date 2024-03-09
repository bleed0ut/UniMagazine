using System.Linq.Expressions;
using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class MagazineRepository : IMagazineRepository
    {
        private readonly AppDbContext _dbContext;
        public MagazineRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public void Add(Magazine entity)
        {
            _dbContext.Magazines.Add(entity);
        }

        public void Delete(Magazine entity)
        {
            _dbContext.Magazines.Remove(entity);
        }

        /*public Magazine Get(int id)
        {
            return _dbContext.Magazines.FirstOrDefault(x => x.Id == id);
        }*/

        public IEnumerable<Magazine>? GetAll(string? includeProperty = null)
        {
            return _dbContext.Magazines.ToList();
            
        }

        public void Update(Magazine magazine)
        {
            throw new NotImplementedException();
        }

        public Magazine Get(Expression<Func<Magazine, bool>> filter, string? includeProperty = null)
        {
            throw new NotImplementedException();
        }
    }
}

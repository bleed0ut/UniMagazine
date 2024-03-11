using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Models.ViewModels;
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
            return _dbContext.Magazines.Include(f => f.Faculty).ToList();
            
        }

        public void Update(Magazine magazine)
        {
            _dbContext.Magazines.Update(magazine);
        }

        public Magazine Get(Expression<Func<Magazine, bool>> filter, string? includeProperty = null)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatus(Magazine magazine)
        {
            DateTime today = DateTime.Now;
            if(today >= magazine.ClosedDate)
            {
                magazine.Status = "Closed";
            }
            else
            {
                if(today > magazine.OpenedDate)
                {
                    magazine.Status = "Opening";
                }
                
            }

            Update(magazine);
        }
    }
}

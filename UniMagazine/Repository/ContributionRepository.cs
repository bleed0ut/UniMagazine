using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class ContributionRepository : Repository<Contribution>, IContributionRepository
    {
        private readonly AppDbContext _dbContext;
        public ContributionRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Contribution> GetAllPublishedContribution(int magazineId)
        {
            return _dbContext.Contributions.Where(m => m.MagazineId == magazineId).Where(s => s.Status == "Published").Include(u => u.User).ToList();
        }

        public void Update(Contribution contribution)
        {
            throw new NotImplementedException();
        }



    }
}

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

        public IEnumerable<Contribution> GetAllPendingContribution(int facultyId)
        {
            var contributions = _dbContext.Contributions.Where(s => s.Status == "Pending").Include(m => m.Magazine).Include(u => u.User).ToList();
            contributions = contributions.Where(c => c.Magazine.FacultyId  == facultyId).OrderByDescending(c => c.CreatedDate).ToList();

            return contributions;
        }

        public IEnumerable<Contribution> GetAllPublishedContribution(int magazineId)
        {
            var contributions = _dbContext.Contributions.Where(m => m.MagazineId == magazineId)
                                                        .Where(s => s.Status == "Published")
                                                        .Include(u => u.User)
                                                        .ThenInclude(f => f.Faculty)
                                                        .Include(m => m.Files)
                                                        .OrderByDescending(c => c.CreatedDate)
                                                        .ToList();


            return contributions;
        }


        public void Update(Contribution contribution)
        {
            _dbContext.Contributions.Update(contribution);
        }
        
        public Contribution Get(int id)
        {
            return _dbContext.Contributions.Include(u => u.User)
                                           .Include(m => m.Files)
                                           .FirstOrDefault(x => x.Id == id);
        }


    }
}

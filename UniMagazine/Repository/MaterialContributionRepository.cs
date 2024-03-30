using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class MaterialContributionRepository : Repository<MaterialContribution>, IMaterialContributionRepository
    {
        private readonly AppDbContext _dbContext;
        public MaterialContributionRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(MaterialContribution macon)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatus(MaterialContribution macon)
        {
            throw new NotImplementedException();
        }
        public MaterialContribution Get(int id)
        {
            return _dbContext.MaterialContributions.FirstOrDefault(mc => mc.Id == id);
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
            _dbContext.SaveChanges();
        }

        public void UpdateStatus(MaterialContribution macon)
        {
            throw new NotImplementedException();
        }
        public MaterialContribution Get(int id)
        {
            return _dbContext.MaterialContributions.FirstOrDefault(mc => mc.Id == id);
        }
        public IEnumerable<MaterialContribution> GetMaterial(int conId)
        {
            var materialContribution = _dbContext.MaterialContributions.Where(x => x.ContributionId == conId).ToList();
                                                        

            return materialContribution;
        }

        public void DeleteFileForRejectUpdating(int contributionId, string rootPath)
        {
            var newUpdateFiles = _dbContext.MaterialContributions.Where(c => c.ContributionId == contributionId)
                                                                 .Where(s => s.Status == "Updating")
                                                                 .ToList();
            _dbContext.RemoveRange(newUpdateFiles);
            
            foreach(var file in newUpdateFiles)
            {
                if(File.Exists(Path.Combine(rootPath, file.ImageUrl)))
                {
                    File.Delete(Path.Combine(rootPath, file.ImageUrl));
                }
            }
        }

        public void PublishFileStatusForUpdating(int contributionId)
        {
            var newUpdateFiles = _dbContext.MaterialContributions.Where(c => c.ContributionId == contributionId)
                                                                 .Where(s => s.Status == "Updating")
                                                                 .ToList();
            if(newUpdateFiles.Count > 0)
            {
                foreach(var file in newUpdateFiles)
                {
                    file.Status = "Published";
                    Update(file);
                }
            }
        }
    }
}

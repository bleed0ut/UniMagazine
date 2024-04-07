using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IMaterialContributionRepository : IRepository<MaterialContribution>
    {
        void Update(MaterialContribution maCon);
        void UpdateStatus(MaterialContribution maCon);
        public IEnumerable<MaterialContribution> GetMaterial(int conId);
        MaterialContribution Get(int id);

        void DeleteFileForRejectUpdating(int contributionId, string rootPath);
        void PublishFileStatusForUpdating(int contributionId);
    }
}

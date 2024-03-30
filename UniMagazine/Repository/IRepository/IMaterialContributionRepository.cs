using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IMaterialContributionRepository : IRepository<MaterialContribution>
    {
        void Update(MaterialContribution maCon);
        void UpdateStatus(MaterialContribution maCon);
        MaterialContribution Get(int id);
    }
}

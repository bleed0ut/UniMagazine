using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IContributionRepository : IRepository<Contribution>
    {
        IEnumerable<Contribution> GetAllContribution(int magazineId);
    }
}

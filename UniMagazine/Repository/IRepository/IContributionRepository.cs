using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IContributionRepository : IRepository<Contribution>
    {
        IEnumerable<Contribution> GetAllPublishedContribution(int magazineId);
        IEnumerable<Contribution> GetAllPendingContribution(int facultyId);

        Contribution Get(int id);

    }
}

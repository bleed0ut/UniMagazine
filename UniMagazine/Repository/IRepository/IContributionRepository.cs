using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IContributionRepository : IRepository<Contribution>
    {
        IEnumerable<Contribution> GetAllPublishedContribution(int magazineId, string? search, string? userId);
        IEnumerable<Contribution> GetAllPendingContribution(int facultyId, string? searchByTitle, string? searchByContibutorEmail);
        Contribution Get(int id);
    }
}

using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IContributionRepository : IRepository<Contribution>
    {
        IEnumerable<Contribution> GetAllPublishedContribution(int magazineId, string? search, string? userId);
        IEnumerable<Contribution> GetAllPendingContribution(int facultyId, string? searchByTitle, string? searchByContibutorEmail, string? status);
        IEnumerable<Contribution> GetMyContribution(string userId, string? status, string? search);
        Contribution? Get(int id);
        IEnumerable<Contribution> GetByYear(int academicYearId);
        IEnumerable<Contribution> GetByMagazine(int magazineId);
        void Update(Contribution con);
        void CheckAPendingContribution(Contribution contribution);
        void CheckManyPendingContribution(IEnumerable<Contribution> contributions);
        IEnumerable<Contribution> GetExpiredContributions();
    }
}

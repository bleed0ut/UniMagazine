using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Plugins;
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

        public IEnumerable<Contribution> GetAllPendingContribution(int facultyId, string? searchByTitle = "", string? searchByContibutorEmail = "", string? status = "")
        {
            if (status.IsNullOrEmpty())
                status = "Pending";
            var contributions = _dbContext.Contributions.Where(s => s.Status == status)
                                                        .Include(m => m.Magazine)
                                                        .Include(u => u.User)
                                                        .ToList();
            contributions = contributions.Where(c => c.Magazine.FacultyId == facultyId)
                                         .OrderByDescending(c => c.CreatedDate)
                                         .ToList();

            if (!string.IsNullOrEmpty(searchByTitle))
                contributions = contributions.Where(s => s.Magazine.Title.ToLower().Contains(searchByTitle.ToLower())).ToList();

            if (!string.IsNullOrEmpty(searchByContibutorEmail))
                contributions = contributions.Where(s => s.User.Email.ToLower().Contains(searchByContibutorEmail.ToLower())).ToList();

            return contributions;
        }

        public IEnumerable<Contribution> GetAllPublishedContribution(int magazineId, string? search = "", string? userId = "")
        {
            var contributions = _dbContext.Contributions.Where(m => m.MagazineId == magazineId)
                                                        .Where(s => s.Status == "Published")
                                                        .Include(u => u.User)
                                                        .ThenInclude(f => f.Faculty)
                                                        .Include(m => m.Files)
                                                        .OrderByDescending(c => c.CreatedDate)
                                                        .ToList();

            if (!string.IsNullOrEmpty(search))
                contributions = contributions.Where(e => e.User.Email.ToLower().Contains(search.ToLower())).ToList();
            if (!string.IsNullOrEmpty(userId))
                contributions = contributions.Where(e => e.User.Id == userId).ToList();

            return contributions;
        }


        public void Update(Contribution contribution)
        {
            _dbContext.Contributions.Update(contribution);
        }

        public Contribution? Get(int id)
        {
            var contribution = _dbContext.Contributions.Include(u => u.User).ThenInclude(f => f.Faculty)
                                           .Include(m => m.Files)
                                           .Include(m => m.Magazine)
                                           .Include(m => m.FeedBacks)
                                           .FirstOrDefault(x => x.Id == id);
            contribution.FeedBacks = contribution.FeedBacks.OrderByDescending(x => x.CreatedDate).ToList();
            return contribution;
        }

        public void CheckAPendingContribution(Contribution contribution)
        {
            DateTime today = DateTime.Now;
            TimeSpan ts = today - contribution.CreatedDate;
            if (ts.Days >= 14)
                contribution.Status = "Expired";

            Update(contribution);
        }
        
        public void CheckManyPendingContribution(IEnumerable<Contribution> contributions)
        {
            DateTime today = DateTime.Now;
            TimeSpan ts;
            foreach(var con in contributions)
            {
                ts = today - con.CreatedDate;
                if (ts.Days >= 14)
                    con.Status = "Expired";
                Update(con);
            }
        }

        public IEnumerable<Contribution> GetMyContribution(string userId, string? status = "", string? search = "")
        {
            if (string.IsNullOrEmpty(status))
                status = "Published";
            var contributions = _dbContext.Contributions.Where(x => x.UserId == userId)
                                                        .Where(x => x.Status == status)
                                                        .Include(u => u.User)
                                                        .ThenInclude(f => f.Faculty)
                                                        .Include(m => m.Magazine)
                                                        .OrderByDescending(c => c.CreatedDate)
                                                        .ToList();

            if (!string.IsNullOrEmpty(search))
                contributions = contributions.Where(s => s.Magazine.Title.ToLower().Contains(search.ToLower())).ToList();
        
            return contributions;
        }
        public IEnumerable<Contribution> GetByYear(int academicYearId)
        {
            var contributions = _dbContext.Contributions.Include(m => m.Magazine)
                                                        .ThenInclude(m => m.Academic)
                                                        .Include(u => u.User)
                                                        .ThenInclude(f => f.Faculty)
                                                        .Include(f => f.Files)
                                                        .Where(m => m.Magazine.AcademicYearId == academicYearId)
                                                        .Where(s => s.Status == "Published")
                                                        .Where(f => f.User.Faculty.Name != "Sample Faculty")
                                                        .Where(c => c.Files.Count() > 0)
                                                        .OrderByDescending(c => c.CreatedDate)
                                                        .ToList();

            return contributions;
        }

        public IEnumerable<Contribution> GetByMagazine(int magazineId) {
            var contributions = _dbContext.Contributions.Where(m => m.MagazineId == magazineId)
                                                        .Where(s => s.Status == "Published")
                                                        .Include(f => f.Files)
                                                        .Include(u => u.User)
                                                        .ToList();

            return contributions;
        }

        public IEnumerable<Contribution> GetExpiredContributions()
        {
            var contributions = _dbContext.Contributions.Where(s => s.Status == "Expired")
                                                        .OrderByDescending(c => c.CreatedDate)
                                                        .Include(u => u.User)
                                                        .Include(m => m.Magazine)
                                                        .ThenInclude(f => f.Faculty)
                                                        .Where(f => f.Magazine.Faculty.Name != "Sample Faculty")
                                                        .ToList();
            return contributions;
        }
    }
}

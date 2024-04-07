
using Microsoft.EntityFrameworkCore;
using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class DashRepository: IDashRepository
    {
        private readonly AppDbContext _dbContext;
        public DashRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public List<object> GetAllConInAllFaByAca()
        {
            List<object> data = new List<object>();
            var faculties = _dbContext.Faculties.ToList();
            var academicYears = _dbContext.AcademicYears.ToList();

            var contributionsPerFacultyPerYear =
                from faculty in faculties
                from year in academicYears
                select new
                {
                    FacultyId = faculty.Id,
                    FacultyName = faculty.Name,
                    AcademicYearId = year.Id,
                    AcademicYear = year.YearDate.ToString("yyyy"),
                    ContributionCount = _dbContext.Magazines
                        .Where(m => m.FacultyId == faculty.Id && m.AcademicYearId == year.Id)
                        .SelectMany(m => m.Contributions)
                        .Count()
                };

            foreach (var contribution in contributionsPerFacultyPerYear)
            {
                data.Add(new
                {
                    FacultyId = contribution.FacultyId,
                    FacultyName = contribution.FacultyName,
                    AcademicYearId = contribution.AcademicYearId,
                    AcademicYear = contribution.AcademicYear,
                    ContributionCount = contribution.ContributionCount
                });
            }
            return data;
        }

        public List<object> GetAllContributerInAllFaByAca()
        {
            throw new NotImplementedException();
        }

        public List<object> GetPerConInAllFaByAca()
        {
            throw new NotImplementedException();
        }
    }
}

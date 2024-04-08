
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Stimulsoft.Data.Extensions;
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

        public List<object> GetAllConInAllFaByAca(string? year)
        {
            List<object> data = new List<object>();
            if (year != null)
            {
                if (int.TryParse(year, out int yearValue))
                {
                    var year2 = _dbContext.AcademicYears.FirstOrDefault(k => k.Id == yearValue);

                    List<string> label = _dbContext.Faculties.Select(m => m.Name).ToList();
                    var contributionsPerFaculty = _dbContext.Faculties
                        .Select(f => _dbContext.Magazines.Where(m => m.FacultyId == f.Id && m.AcademicYearId == year2.Id).SelectMany(m => m.Contributions).Count())
                        .ToList(); // Count the characters in the detail
                    data.Add(label);
                    data.Add(contributionsPerFaculty);
                    return data;
                }
                else
                {
                    // Handle invalid year format
                    // For example:
                    throw new ArgumentException("Invalid year format. Please provide a valid integer year.");
                }
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

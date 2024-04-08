
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

                    List<string> label = new List<string>();
                    List<int> count = new List<int>();  
                    var publishedContributions = _dbContext.Faculties
                        .Select(f => _dbContext.Magazines.Where(m => m.FacultyId == f.Id && m.AcademicYearId == year2.Id).SelectMany(m => m.Contributions).Count())
                        .ToList(); // Count the characters in the detail
                    var query = from c in _dbContext.Contributions
                                join m in _dbContext.Magazines on c.MagazineId equals m.Id
                                join f in _dbContext.Faculties on m.FacultyId equals f.Id
                                where c.Status == "Published"
                                where m.AcademicYearId == year2.Id
                                group c by new { f.Id, f.Name } into g
                                select new
                                {
                                    FacultyName = g.Key.Name,
                                    PublishedContributionsNumber = g.Count()
                                };

                    //List<object> facultiesWithNumber = query.ToList<object>();
                    //foreach (var faculty in facultiesWithNumber)
                    //{
                    //    label.Add(faculty.FacultyName)
                    //}
                    //data.Add(label);
                    //data.Add(publishedContributions);
                    foreach(var item in query)
                    {
                        label.Add(item.FacultyName);
                        count.Add(item.PublishedContributionsNumber);
                    }
                    data.Add(label);
                    data.Add(count);

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

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Models.ViewModels;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class MagazineRepository : Repository<Magazine>, IMagazineRepository
    {
        private readonly AppDbContext _dbContext;
        public MagazineRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Magazine entity)
        {
            _dbContext.Magazines.Add(entity);
        }

        public void Delete(Magazine entity)
        {
            _dbContext.Magazines.Remove(entity);
        }

        ///*public Magazine Get(int id)
        //{
        //    return _dbContext.Magazines.FirstOrDefault(x => x.Id == id);
        //}*/

        public IEnumerable<Magazine>? GetAll(string? includeProperty = null)
        {
            return _dbContext.Magazines.Where(f => f.Status != "Not Assigned").Include(f => f.Faculty).Include(a => a.Academic).ToList();

        }

    public IEnumerable<Magazine> GetAllMagazines(int facultyId, int academicYearId, string status = "")
        {
            var magazines = _dbContext.Magazines.Where(f => f.Status != "Not Assigned").Include(f => f.Faculty).Include(a => a.Academic).ToList();

            if (facultyId > 0)
                magazines = magazines.Where(f => f.FacultyId == facultyId).ToList();
            if (academicYearId > 0)
                magazines = magazines.Where(a => a.AcademicYearId ==  academicYearId).ToList();
            if (!string.IsNullOrEmpty(status))
                magazines = magazines.Where(s=> s.Status == status).ToList();

            return magazines;
        }

        public IEnumerable<Magazine>? GetActiveMagazines(int facultyId = 0, string? status = "", int? academicYearId = 0, string? search = "")
        {
            var magazines = _dbContext.Magazines.OrderByDescending(o => o.OpenedDate)
                                        .Include(f => f.Faculty)
                                        .ToList();

            if (string.IsNullOrEmpty(status) || status == "Opening")
                magazines = _dbContext.Magazines.Where(s => s.Status == "Opening").ToList();
            else
                magazines = _dbContext.Magazines.Where(s => s.Status == "Closed").ToList();

            if (academicYearId > 0)
                magazines = magazines.Where(a => a.AcademicYearId == academicYearId).ToList();

            if (!string.IsNullOrEmpty(search))
                magazines = magazines.Where(s => s.Title.ToLower().Contains(search.ToLower())).ToList();

            if (facultyId > 0)
                magazines = magazines.Where(f => f.FacultyId == facultyId).ToList();

            return magazines;
        }

        public IEnumerable<Magazine> GetNotAssigned()
        {
            return _dbContext.Magazines.Where(f => f.Status == "Not Assigned").Include(f => f.Faculty).ToList();
        }
        public void Update(Magazine magazine)
        {
            _dbContext.Magazines.Update(magazine);
        }


        public void UpdateStatus(Magazine magazine)
        {
            DateTime today = DateTime.Now;
            if (magazine.OpenedDate == null)
                magazine.Status = "Not Assigned";
            if(today >= magazine.ClosedDate)
                magazine.Status = "Closed";
            else
            {
                if (today > magazine.OpenedDate)
                    magazine.Status = "Opening";
                else if (today < magazine.OpenedDate)
                    magazine.Status = "Not Started";
            }

            Update(magazine);
        }

        public void UpdateStatusMany(IEnumerable<Magazine> magazines)
        {
            foreach (var magazine in magazines)
            {
                DateTime today = DateTime.Now;
                if (magazine.OpenedDate == null)
                    magazine.Status = "Not Assigned";
                if (today >= magazine.ClosedDate)
                    magazine.Status = "Closed";
                else
                {
                    if (today > magazine.OpenedDate)
                        magazine.Status = "Opening";
                    else if (today < magazine.OpenedDate)
                        magazine.Status = "Not Started";
                }

                Update(magazine);
            }
        }
    }
}

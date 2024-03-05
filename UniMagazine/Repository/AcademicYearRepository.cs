using Microsoft.EntityFrameworkCore;
using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class AcademicYearRepository: IAcademicYearRepository
    {
        private readonly AppDbContext _dbContext;
        

        public AcademicYearRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public AcademicYear Add(AcademicYear academicYear)
        {
            _dbContext.AcademicYears.Add(academicYear);
            _dbContext.SaveChanges();
            return academicYear;
        }


        

        public AcademicYear Get(int id)
        {
            return _dbContext.AcademicYears.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<AcademicYear> GetAllAcademicYear()
        {
            return _dbContext.AcademicYears.ToList();
        }

        public void Update(AcademicYear academicYear)
        {
            var existingAcademic = _dbContext.AcademicYears.Find(academicYear.Id);
            if (existingAcademic != null)
            {
                existingAcademic.YearDate = academicYear.YearDate;
                existingAcademic.OpenDate = academicYear.OpenDate;
                existingAcademic.CloseDate = academicYear.CloseDate;
                existingAcademic.Status = academicYear.Status;
            }
            _dbContext.AcademicYears.Update(existingAcademic);
            _dbContext.SaveChanges();

        }
        public AcademicYear Delete(int id) {
            var deleteAcademic = _dbContext.AcademicYears.Find(id);
            if (deleteAcademic != null)
            {
                _dbContext.AcademicYears.Remove(deleteAcademic);
                _dbContext.SaveChanges();
                return deleteAcademic;
            }
            return null;
        }


    }
}

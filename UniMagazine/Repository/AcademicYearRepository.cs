using Microsoft.EntityFrameworkCore;
using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class AcademicYearRepository: Repository<AcademicYear>, IAcademicYearRepository
    {
        private readonly AppDbContext _dbContext;
        

        public AcademicYearRepository(AppDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }


        public IEnumerable<AcademicYear> GetClosedAcademicYear()
        {
            return _dbContext.AcademicYears.Where(x => x.Status == "Closed").ToList();
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
                existingAcademic.OpenedDate = academicYear.OpenedDate;
                existingAcademic.ClosedDate = academicYear.ClosedDate;
                existingAcademic.Status = academicYear.Status;
            }
            _dbContext.AcademicYears.Update(existingAcademic);
        }
        public void Delete(AcademicYear deleteAcademic)
        {
            var Magazine = _dbContext.Magazines.Where(x => x.AcademicYearId == deleteAcademic.Id).ToList();

            foreach (var magazine in Magazine)
            {
                var Contri1 = _dbContext.Contributions.Where(x => x.MagazineId == magazine.Id);
                _dbContext.Contributions.RemoveRange(Contri1);
            }
            _dbContext.Magazines.RemoveRange(Magazine);
            if (deleteAcademic != null)
                _dbContext.AcademicYears.Remove(deleteAcademic);
        }
    }
}

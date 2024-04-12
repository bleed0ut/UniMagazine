using Microsoft.EntityFrameworkCore;
using System.Linq;
using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class AcademicYearRepository: Repository<AcademicYear>, IAcademicYearRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AcademicYearRepository(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment) : base(dbContext)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
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
                var contributions = _dbContext.Contributions.Where(x => x.MagazineId == magazine.Id).ToList();
                
                string wwwRootPath1 = _webHostEnvironment.WebRootPath;
                var oldImagePath1 = Path.Combine(wwwRootPath1, magazine.ImageUrl.TrimStart('\\'));

                if (File.Exists(oldImagePath1))
                {
                    File.Delete(oldImagePath1);
                }
                foreach (var contribution in contributions)
                {
                    // Find MaterialContributions indirectly associated with the current contribution
                    var materialContributions = _dbContext.MaterialContributions.Where(mc => mc.ContributionId == contribution.Id).ToList();
                    foreach (var fil in materialContributions)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        var oldImagePath = Path.Combine(wwwRootPath, fil.ImageUrl.TrimStart('\\'));

                        if (File.Exists(oldImagePath))
                        {
                            File.Delete(oldImagePath);
                        }
                    }
                        _dbContext.MaterialContributions.RemoveRange(materialContributions);
                }

                _dbContext.Contributions.RemoveRange(contributions);
                
            }
            _dbContext.Magazines.RemoveRange(Magazine);
            if (deleteAcademic != null)
                _dbContext.AcademicYears.Remove(deleteAcademic);
        }
    }
}

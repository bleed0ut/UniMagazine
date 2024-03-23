using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IAcademicYearRepository : IRepository<AcademicYear>
    {
        public void Update(AcademicYear academicYear);
        public IEnumerable<AcademicYear> GetAllAcademicYear();

        public IEnumerable<AcademicYear> GetClosedAcademicYear();
    }
}

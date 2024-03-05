using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IAcademicYearRepository
    {
        AcademicYear Add(AcademicYear faculty);
        void Update(AcademicYear faculty);
        AcademicYear Delete(int id);
        AcademicYear Get(int id);
        public IEnumerable<AcademicYear> GetAllAcademicYear();
    }
}

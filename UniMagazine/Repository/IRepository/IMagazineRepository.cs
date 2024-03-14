using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IMagazineRepository: IRepository<Magazine>
    {
       IEnumerable<Magazine> GetNotAssigned();
       IEnumerable<Magazine> GetAllMagazines(int facultyId, int academicYearId, string status);
       void Update(Magazine magazine);
       void UpdateStatus(Magazine magazine);
    }
}

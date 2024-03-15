using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IMagazineRepository: IRepository<Magazine>
    {
       IEnumerable<Magazine> GetNotAssigned();
       IEnumerable<Magazine> GetAllMagazines(int facultyId, int academicYearId, string status);

       IEnumerable<Magazine>? GetActiveMagazines(int facultyId);
       void Update(Magazine magazine);
       void UpdateStatus(Magazine magazine);
    }
}

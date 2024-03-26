using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IMagazineRepository: IRepository<Magazine>
    {
       IEnumerable<Magazine> GetNotAssigned();
       IEnumerable<Magazine> GetAllMagazines(int facultyId, int academicYearId, string status);
       
       IEnumerable<Magazine>? GetActiveMagazines(int facultyId, string? status, int? academicYearId, string? search);

       void Update(Magazine magazine);
       void UpdateStatus(Magazine magazine);

       void UpdateStatusMany(IEnumerable<Magazine> magazines);

    }
}

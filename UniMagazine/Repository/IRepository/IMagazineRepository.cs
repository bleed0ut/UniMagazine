using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IMagazineRepository: IRepository<Magazine>
    {
       void Update(Magazine magazine);
       void UpdateStatus(Magazine magazine);
    }
}

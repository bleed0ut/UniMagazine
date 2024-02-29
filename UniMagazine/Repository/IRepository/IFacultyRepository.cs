using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IFacultyRepository
    {
        Faculty Add(Faculty faculty);
        void Update(Faculty faculty);
        Faculty Delete(int id);
        Faculty Get(int id);
        public IEnumerable<Faculty> GetAllFaculty();
    }
}

using UniMagazine.Models;

namespace UniMagazine.Repository.IRepository
{
    public interface IUserRepository
    {
        void CreateUser();
        public IEnumerable<ApplicationUser> GetAllUser(string search, string role, int facultyId);
    }
}

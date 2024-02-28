using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository (AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateUser()
        {
            throw new NotImplementedException();
        }
  

        public IEnumerable<ApplicationUser> GetAllUser(string search, string role)
        {
            search = search.ToLower().Trim();

            var users = (from user in _dbContext.Users
                         where string.IsNullOrEmpty(search) || user != null && user.Email.ToLower().Contains(search)
                         select new ApplicationUser
                         {
                             Id = user.Id,
                             FullName = user.FullName,
                             Address = user.Address,
                             DateOfBirth = user.DateOfBirth,
                             Email = user.Email,
                             Role = user.Role,
                         }).ToList();

            if (!string.IsNullOrEmpty(role))
                users = users.Where(u => u.Role == role).ToList();
            else
                users = users.Where(u => u.Role != "Admin").ToList();

            return users;
        }
    }
}

using Azure;
using Microsoft.EntityFrameworkCore;
using UniMagazine.Data;
using UniMagazine.Models;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Repository
{
    public class FacultyRepository : IFacultyRepository
    {
        private readonly AppDbContext _dbContext;

        public FacultyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Faculty Add(Faculty faculty)
        {
            _dbContext.Faculties.Add(faculty);
            _dbContext.SaveChanges();
            return faculty;
        }

        public Faculty Delete(int id)
        {
            var existingFaculty = _dbContext.Faculties.Find(id);
            if (existingFaculty != null)
            {
                _dbContext.Faculties.Remove(existingFaculty);
                _dbContext.SaveChanges();
                return existingFaculty;
            }

            return null;
        }

        public Faculty Get(int id)
        {
            return _dbContext.Faculties.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Faculty> GetAllFaculty()
        {
            return _dbContext.Faculties.ToList();
        }

        public void Update(Faculty faculty)
        {
            var existingFaculty = _dbContext.Faculties.Find(faculty.Id);
            if (existingFaculty != null)
            {
                existingFaculty.Name = faculty.Name;
                existingFaculty.Description= faculty.Description;
            }
            _dbContext.Faculties.Update(existingFaculty);
            _dbContext.SaveChanges();

        }

    }
}

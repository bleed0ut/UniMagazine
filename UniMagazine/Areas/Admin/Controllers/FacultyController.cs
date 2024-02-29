using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using UniMagazine.Models;
using UniMagazine.Repository;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FacultyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FacultyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(Faculty faculty)
        {
            var fal = new Faculty
            {
                Name = faculty.Name,
                Description = faculty.Description,
                CreateDate = DateTime.Now,
            };
            _unitOfWork.FacultyRepository.Add(fal);
            return RedirectToAction("List");

        }

        [HttpGet]
        public IActionResult List()
        {
            var fal = _unitOfWork.FacultyRepository.GetAllFaculty();
            return View(fal);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var fac = _unitOfWork.FacultyRepository.Get(id);
            if (fac == null )
            {
                return NotFound();
            }
            return View(fac);
        }

        [HttpPost]
        public IActionResult Update(Faculty faculty)
        {
            var fac = _unitOfWork.FacultyRepository.Get(faculty.Id);

            if (fac == null)
            {
                return NotFound();
            }
            fac.Name = faculty.Name;
            fac.Description = faculty.Description;
            _unitOfWork.FacultyRepository.Update(fac);
            _unitOfWork.Save();
            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var existingFaculty = _unitOfWork.FacultyRepository.Delete(id);
            if (existingFaculty == null)
            {
                return NotFound();
            }
            return RedirectToAction("List");
        }
    }
}

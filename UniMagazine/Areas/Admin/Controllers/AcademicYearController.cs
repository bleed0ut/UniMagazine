using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniMagazine.Repository;
using UniMagazine.Repository.IRepository;
using UniMagazine.Models;
using NuGet.Protocol.Plugins;
using System.Net.NetworkInformation;

namespace UniMagazine.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AcademicYearController : Controller
    {
        
        public IUnitOfWork _unitOfWork { get; set; }
        public AcademicYearController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            var aca = _unitOfWork.AcademicYearRepository.GetAllAcademicYear();
            return View(aca);
        }
        public IActionResult Add() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AcademicYear academicY)
        {
            if (ModelState.IsValid)
            {
                // Validate opened date against closed date
                if (academicY.OpenedDate > academicY.ClosedDate)
                {
                    TempData["Error"] = "Open Date cannot exceed closure date";
                    return View(academicY); // Return to the view with validation error
                }
                var date = DateTime.Now;
                if (date >= academicY.ClosedDate)
                {
                    academicY.Status = "Closed";
                }
                if(date.Year >= academicY.OpenedDate.Year)
                {
                    TempData["Error"] = "Can't create academic year that is in the past";
                    return View(academicY);
                }
                else if (date.Year < academicY.OpenedDate.Year)
                {
                    academicY.Status = "Not Started";
                }
                else
                {
                    academicY.Status = "Opening";
                }

                _unitOfWork.AcademicYearRepository.Add(academicY);
                return RedirectToAction("Index");
            }

            // If model state is not valid, return to the view with validation error
            return View(academicY);
        }


        public IActionResult Update(int id)
        {
            if (id == null || id == 0)
            {    
                return NotFound();
            }
            var aca = _unitOfWork.AcademicYearRepository.Get(id);
            if (aca == null)
            {
                return NotFound();
            }
            return View(aca);
        }

        [HttpPost]
        public IActionResult Update(AcademicYear academicY)
        {
            if (ModelState.IsValid)
            {

                var date = DateTime.Now;
                if (date >= academicY.ClosedDate)
                {
                    academicY.Status = "Closed";
                }
                else if (date.Year >= academicY.OpenedDate.Year)
                {
                    TempData["Error"] = "Can't create academic year that is in the past";
                    return View(academicY);
                }
                else if (date.Year < academicY.OpenedDate.Year)
                {
                    academicY.Status = "Not Started";
                }
                else
                {
                    academicY.Status = "Opening";
                }

                if (academicY == null)
                {
                    return NotFound();
                }
                if (date >= academicY.ClosedDate)
                {
                    academicY.Status = "Closed";
                }
                else
                {
                    academicY.Status = "Opening";
                }
                _unitOfWork.AcademicYearRepository.Update(academicY);
                _unitOfWork.Save();
            }
            
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var existingAY = _unitOfWork.AcademicYearRepository.Delete(id);
            if (existingAY == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
    }
}

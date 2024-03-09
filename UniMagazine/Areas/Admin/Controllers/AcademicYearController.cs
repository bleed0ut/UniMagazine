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
        public IActionResult Add(AcademicYear academicY)
        {
            var aca = new AcademicYear
            {
                YearDate = academicY.OpenedDate,
                OpenedDate = academicY.OpenedDate,
                ClosedDate = academicY.ClosedDate,
                Status = academicY.Status,
            };
            _unitOfWork.AcademicYearRepository.Add(aca);
            return RedirectToAction("Index"); ;
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
            var aca = _unitOfWork.AcademicYearRepository.Get(academicY.Id);

            if (aca == null)
            {
                return NotFound();
            }
            aca.YearDate = academicY.OpenedDate;
            aca.OpenedDate = academicY.OpenedDate;
            aca.ClosedDate = academicY.ClosedDate;
            aca.Status = academicY.Status;
            _unitOfWork.AcademicYearRepository.Update(aca);
            _unitOfWork.Save();
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

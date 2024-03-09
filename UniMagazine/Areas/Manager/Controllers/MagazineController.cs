using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UniMagazine.Migrations;
using Microsoft.AspNetCore.Http;
using UniMagazine.Models;
using UniMagazine.Models.ViewModels;
using UniMagazine.Repository.IRepository;

namespace UniMagazine.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class MagazineController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MagazineController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var maga = _unitOfWork.MagazineRepository.GetAll();
            return View(maga);
        }
        [HttpGet]
        public IActionResult Add()
        {
            MagazineVM magazineVm = new MagazineVM()
            {
                Faculties = _unitOfWork.FacultyRepository.GetAllFaculty().Select(f => new SelectListItem
                {
                    Text = f.Name,
                    Value = f.Id.ToString()
                }),
                AcademicYears = _unitOfWork.AcademicYearRepository.GetAllAcademicYear().Select(A => new SelectListItem
                {
                    Text = A.YearDate.ToString("yyyy"),
                    Value = A.Id.ToString()
                }),
            };
            return View(magazineVm);
        }

        [HttpPost]
        public IActionResult Add(MagazineVM magazineVm,IFormFile? file)
        {
            if(ModelState.IsValid ) 
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string MagazinePath = Path.Combine(wwwRootPath, @"images\Book");
                    if (!Directory.Exists(MagazinePath))
                    {
                        Directory.CreateDirectory(MagazinePath);
                    }
                    //if (!string.IsNullOrEmpty(MagazineVM.Magazine.ImageUrl))
                    //{
                    //    // Delete the old image
                    //    var oldImagePath = Path.Combine(wwwRootPath, BookVM.Book.ImageUrl.TrimStart('\\'));

                    //    if (System.IO.File.Exists(oldImagePath))
                    //    {
                    //        System.IO.File.Delete(oldImagePath);
                    //    }
                    //}

                    using (var fileStream = new FileStream(Path.Combine(MagazinePath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    magazineVm.Magazine.ImageUrl = @"\images\Book\" + fileName;
                }
                _unitOfWork.MagazineRepository.Add(magazineVm.Magazine);
                _unitOfWork.Save();

            }
            else
            {
                magazineVm.Faculties = _unitOfWork.FacultyRepository.GetAllFaculty().Select(f => new SelectListItem
                {
                    Text = f.Name,
                    Value = f.Id.ToString()
                });
                magazineVm.AcademicYears = _unitOfWork.AcademicYearRepository.GetAllAcademicYear().Select(A => new SelectListItem
                {
                    Text = A.YearDate.ToString("yyyy"),
                    Value = A.Id.ToString()
                });
                return View(magazineVm);
            }
            return View("Index");
        }
    }
}

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
            foreach (var mag in maga)
            {
                _unitOfWork.MagazineRepository.UpdateStatus(mag);
            }
            _unitOfWork.Save();
            maga = _unitOfWork.MagazineRepository.GetAll();
            return View(maga);
        }
        [HttpGet]
        public IActionResult Add()
        {
            MagazineVM magazineVm = GetMagazineVM();
            return View(magazineVm);
        }

        [HttpPost]
        public IActionResult Add(MagazineVM magazineVm, IFormFile? file)
        {
            AcademicYear academicYear = _unitOfWork.AcademicYearRepository.Get(magazineVm.Magazine.AcademicYearId);
            bool isInAcademicYearDate =false;
            if (magazineVm.Magazine.OpenedDate != null)
                isInAcademicYearDate = magazineVm.Magazine.OpenedDate >= academicYear.OpenedDate && magazineVm.Magazine.OpenedDate <= academicYear.ClosedDate;

            if (magazineVm.Magazine.OpenedDate == null || isInAcademicYearDate)
            {
                if (ModelState.IsValid && magazineVm.Magazine.FacultyId != 0 && magazineVm.Magazine.AcademicYearId != 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string MagazinePath = Path.Combine(wwwRootPath, @"img\Magazine");
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

                        magazineVm.Magazine.ImageUrl = @"\img\Magazine\" + fileName;
                    }
                    magazineVm.Magazine.ClosedDate = CalculateClosedDate(magazineVm.Magazine.OpenedDate);
                    if (magazineVm.Magazine.PostedDate >= magazineVm.Magazine.ClosedDate)
                    {
                        magazineVm.Magazine.Status = "Closed";
                    }
                    else
                    {
                        if (magazineVm.Magazine.PostedDate < magazineVm.Magazine.OpenedDate)
                            magazineVm.Magazine.Status = "Not Started";
                        else
                            magazineVm.Magazine.Status = "Opening";
                        if (magazineVm.Magazine.OpenedDate == null)
                            magazineVm.Magazine.Status = "Not Assigned";
                    }
                    _unitOfWork.MagazineRepository.Add(magazineVm.Magazine);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            else
                TempData["Error"] = "The opened/closed date of magazine have to in range of its academic year";
           
             MagazineVM magazineVM2 = GetMagazineVM();
             return View(magazineVM2);
        }

        public IActionResult Update(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            else {
                var mag = _unitOfWork.MagazineRepository.Get(x => x.Id == id);
                var ma = new MagazineVM()
                {
                    Magazine = mag,
                    Faculties = _unitOfWork.FacultyRepository.GetAllFaculty().Select(f => new SelectListItem
                    {
                        Text = f.Name,
                        Value = f.Id.ToString()
                    }),
                    AcademicYears = _unitOfWork.AcademicYearRepository.GetAllAcademicYear().Select(A => new SelectListItem
                    {
                        Text = A.YearDate.ToString("yyyy"),
                        Value = A.Id.ToString()
                    })
                };
                return View(ma);
            }
                
        }
        [HttpPost]
        public IActionResult Update(MagazineVM magazineVm, IFormFile? file)
        {
                if (ModelState.IsValid && magazineVm.Magazine.FacultyId != 0 && magazineVm.Magazine.AcademicYearId != 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string MagazinePath = Path.Combine(wwwRootPath, @"img\Magazine");
                        if (!Directory.Exists(MagazinePath))
                        {
                            Directory.CreateDirectory(MagazinePath);
                        }
                    if (!string.IsNullOrEmpty(magazineVm.Magazine.ImageUrl))
                    {
                        // Delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, magazineVm.Magazine.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(MagazinePath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        magazineVm.Magazine.ImageUrl = @"\img\Magazine\" + fileName;
                    }
                    magazineVm.Magazine.ClosedDate = CalculateClosedDate(magazineVm.Magazine.OpenedDate);
                    
                    _unitOfWork.MagazineRepository.Update(magazineVm.Magazine);
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
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



        }

        public IActionResult DeadlineIndex()
        {
            DeadlineModel dlModel = new DeadlineModel();
            dlModel.Magazines = _unitOfWork.MagazineRepository.GetNotAssigned();

            return View(dlModel);
        }

        [HttpPost]
        public IActionResult DeadlineIndex(DeadlineModel deadlineModel)
        {
            var magazine = _unitOfWork.MagazineRepository.Get(m => m.Id == deadlineModel.Id);
            magazine.OpenedDate = deadlineModel.OpenedDate;
            if (ModelState.IsValid)
            {
                magazine.ClosedDate = CalculateClosedDate(magazine.OpenedDate);
                _unitOfWork.MagazineRepository.UpdateStatus(magazine);
                _unitOfWork.Save();

                TempData["success"] = "Assign a deadline successfully!";
                return RedirectToAction("DeadlineIndex");
            }
            else
                deadlineModel = new DeadlineModel()
                {
                    Magazines = _unitOfWork.MagazineRepository.GetNotAssigned()
                };
                

            return RedirectToAction("DeadlineIndex"); ;
        }
        [HttpPost]
        public IActionResult Delete(MagazineVM magazineVM)
        {
            if (magazineVM.Magazine == null)
            {
                return NotFound();
            }
            else
            {
                if (!string.IsNullOrEmpty(magazineVM.Magazine.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, magazineVM.Magazine.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                    
                }
                _unitOfWork.MagazineRepository.Delete(magazineVM.Magazine);
                _unitOfWork.Save();
            }
            return RedirectToAction("Index"); ;
        }


        private DateTime? CalculateClosedDate(DateTime? openedDate)
        {
            if (openedDate == null)
                return null;

            DateTime closedDate = openedDate.Value.AddDays(14);
            return closedDate;
        }

        private MagazineVM GetMagazineVM()
        {
            return new MagazineVM()
            {
                Magazine = new Magazine(),
                Faculties = _unitOfWork.FacultyRepository.GetAllFaculty().Select(f => new SelectListItem
                {
                    Text = f.Name,
                    Value = f.Id.ToString()
                }),
                AcademicYears = _unitOfWork.AcademicYearRepository.GetAllAcademicYear().Select(A => new SelectListItem
                {
                    Text = A.YearDate.ToString("yyyy"),
                    Value = A.Id.ToString()
                })
            };
        }
    }
}

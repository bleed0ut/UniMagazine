using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace UniMagazine.Models.ViewModels
{
    public class MagazineVM
    {
        public Magazine? Magazine { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? Faculties { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? AcademicYears { get; set; }
    }
}

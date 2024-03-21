using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace UniMagazine.Models.ViewModels
{
    public class MagazineVM
    {
        [ValidateNever]
        public Magazine? Magazine { get; set; }
        
        public IEnumerable<SelectListItem>? Faculties { get; set; }
        
        [ValidateNever]
        public IEnumerable<SelectListItem>? AcademicYears { get; set; }

        [ValidateNever]
        public IEnumerable<Magazine> Magazines { get; set; }
    }
}

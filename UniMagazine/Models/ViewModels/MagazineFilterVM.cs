using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UniMagazine.Models.ViewModels
{
    public class MagazineFilterVM
    {
        public IEnumerable<Magazine> Magazines { get; set; }
        
        [ValidateNever]
        public IEnumerable<Faculty>? FacultiesDisplay { get; set; }

        [ValidateNever]
        public IEnumerable<AcademicYear>? AcademicYearsDisplay { get; set; }

        public int FacultyId = 0;

        public int AcademicYearId = 0;

        public string Status = "";

        public string StatusTemp = "Closed";

        public string Search = "";
    }
}

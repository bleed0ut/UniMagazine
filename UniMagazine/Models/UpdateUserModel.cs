using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace UniMagazine.Models
{
    public class UpdateUserModel
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public int FacultyId { get; set; }
        public string FacultyName { get; set; }
        public string Role { get; set; } = "";

        public Faculty Faculty { get; set; }



        [ValidateNever]
        public IEnumerable<SelectListItem>? Faculties { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}

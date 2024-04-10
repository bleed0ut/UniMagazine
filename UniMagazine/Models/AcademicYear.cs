using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace UniMagazine.Models
{
    public class AcademicYear
    {
        [Key]
    public int Id   { get; set; }
    [ValidateNever]
    public DateTime YearDate { get; set; }
    
    public DateTime OpenedDate { get; set; }
    public DateTime ClosedDate { get; set; }
    [ValidateNever]
    public string Status { get; set; }
  
    }
}

using System.ComponentModel.DataAnnotations;

namespace UniMagazine.Models
{
    public class AcademicYear
    {
        [Key]
    public int Id   { get; set; }
    public DateTime YearDate { get; set; }
    public DateTime OpenedDate { get; set; }
    public DateTime ClosedDate { get; set; }
    public string Status { get; set; }

        // Custom validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OpenedDate > ClosedDate)
            {
                yield return new ValidationResult("Opened date cannot be greater than closed date", new[] { nameof(OpenedDate), nameof(ClosedDate) });
            }
        }
    }
}

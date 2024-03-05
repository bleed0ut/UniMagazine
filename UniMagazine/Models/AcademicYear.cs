using System.ComponentModel.DataAnnotations;

namespace UniMagazine.Models
{
    public class AcademicYear
    {
        [Key]
        public int Id   { get; set; }
        public DateTime YearDate { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string Status { get; set; }
    }
}

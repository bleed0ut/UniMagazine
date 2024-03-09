using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniMagazine.Models
{
    public class Magazine
    {
        [Key]
        public int Id {  get; set; }
        public string Title { get; set; }
        public string Detail {  get; set; }
        public string Status { get; set; } = "Opening";
        public string ImageUrl { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.Now;
        public DateTime OpenedDate { get; set; }
        public DateTime ClosedDate { get; set;}
        public int FacultyId { get; set; }
        //[Required]

        [ForeignKey("FacultyId")]
        public virtual Faculty Faculty { get; set; }
        //[Required]
        public int AcademicYearId { get; set; }

        [ForeignKey("AcademicYearId")]
        public virtual AcademicYear? Academic { get; set; }
    }
}

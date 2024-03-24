using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniMagazine.Models
{
    public class Magazine
    {
        [Key]
        public int Id {  get; set; }

        [Required(ErrorMessage = "Title cannot be empty!")]
        //[Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Detail cannot be empty!")]
        public string Detail {  get; set; }
        public string Status { get; set; } = "Opening";
        public string ?ImageUrl { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.Now;
        public DateTime ?OpenedDate { get; set; }
        public DateTime ?ClosedDate { get; set;}


        public int FacultyId { get; set; }
        //[Required]
        
        [ForeignKey("FacultyId")]
        public virtual Faculty? Faculty { get; set; }
        //[Required]
        public int AcademicYearId { get; set; }

        [ForeignKey("AcademicYearId")]
        public virtual AcademicYear? Academic { get; set; }

        public virtual IEnumerable<Contribution> Contributions { get; set;}
    }
}

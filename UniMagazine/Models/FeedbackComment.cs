using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniMagazine.Models
{
    public class FeedbackComment
    {
        [Key]
        public int Id { get; set; }
        public string? Status { get; set; }
        public string? Comment { get; set; } = "It's acceptable";
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public int ContributionID { get; set; }
        //[Required]
        [ForeignKey("ContributionID")]
        public virtual Contribution? Contribution { get; set; }
        public string UserID { get; set; }
        //[Required]
        [ForeignKey("UserID")]
        public virtual ApplicationUser? User { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniMagazine.Models
{
    public class Contribution
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        [ValidateNever]
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        [ValidateNever]
        public string UserId { get; set; }
        [ValidateNever]
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        public int MagazineId { get; set; }
        [ValidateNever]
        [ForeignKey("MagazineId")]
        public virtual Magazine? Magazine { get; set; }
        [ValidateNever]

        public virtual IEnumerable<MaterialContribution> Files { get; set; }
        [ValidateNever]

        public virtual IEnumerable<FeedbackComment> FeedBacks { get; set; }


    }
}

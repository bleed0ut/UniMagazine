using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniMagazine.Models
{
    public class Contribution
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Content cannot be empty!")]
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

        public virtual IEnumerable<MaterialContribution> Files { get; set; }
        public virtual ICollection<FeedbackComment> FeedBacks { get; set; }
    }
}

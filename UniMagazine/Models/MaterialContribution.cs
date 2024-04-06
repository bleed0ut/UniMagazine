using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniMagazine.Models
{
    public class MaterialContribution
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? ImageUrl { get; set; }

        public string? Status { get; set; } = "Published";
        public int ContributionId { get; set; }
        //[Required]

        [ForeignKey("ContributionId")]
        public virtual Contribution? Contribution { get; set; }
    }
}

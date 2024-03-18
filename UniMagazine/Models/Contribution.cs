using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniMagazine.Models
{
    public class Contribution
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        public int MagazineId { get; set; }
        [ForeignKey("MagazineId")]
        public virtual Magazine? Magazine { get; set; }
    }
}

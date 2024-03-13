using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniMagazine.Models.ViewModels
{
    public class DeadlineModel
    {
        public int Id { get; set; }
        public DateTime? OpenedDate { get; set; }
        [ValidateNever]
        public IEnumerable<Magazine> Magazines { get; set; }
    }
}

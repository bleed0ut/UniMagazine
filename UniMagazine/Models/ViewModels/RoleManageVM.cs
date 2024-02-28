using Microsoft.AspNetCore.Mvc.Rendering;
using UniMagazine.Data;

namespace UniMagazine.Models.ViewModels
{
    public class RoleManageVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }

        // New properties for password change
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

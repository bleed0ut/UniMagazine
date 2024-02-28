using Microsoft.AspNetCore.Mvc.Rendering;
namespace UniMagazine.Models.ViewModels
{
    public class UserVM
    {
       public IEnumerable<ApplicationUser> Users { get; set; }
       public string SearchByEmail { get; set; } = "";

       public string Role = "";
    }
}

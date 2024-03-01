using Microsoft.AspNetCore.Mvc.Rendering;
namespace UniMagazine.Models.ViewModels
{
    public class UserVM
    {
       public IEnumerable<ApplicationUser> Users { get; set; }

       public IEnumerable<Faculty> Faculties { get; set; }

        public int FacultyId { get; set; } = 0;
       public string SearchByEmail { get; set; } = "";

       public string Role = "";
    }
}

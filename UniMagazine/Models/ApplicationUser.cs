using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UniMagazine.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Required(ErrorMessage = "Name cannot be empty!")]
    [Column(TypeName = "nvarchar(100)")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "DateOfBirth cannot be empty!")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Address cannot be empty!")]
    [Column(TypeName = "nvarchar(150)")]
    public string Address { get; set; }

    [Column(TypeName = "nvarchar(40)")]
    public string? Role { get; set; }

    //Faculty
    public int FacultyId { get; set; }

    [ForeignKey("FacultyId")]
    public virtual Faculty? Faculty { get; set; }
    public virtual ICollection<Contribution> Contributions { get; set; }
}


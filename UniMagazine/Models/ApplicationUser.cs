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
    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string FullName { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(150)")]
    public string Address { get; set; }

    [Column(TypeName = "nvarchar(40)")]
    public string? Role { get; set; }

    //Faculty
}


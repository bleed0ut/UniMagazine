using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UniMagazine.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public String FullName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public String Address { get; set; }

    public String Role { get; set; }

    //Faculty
}


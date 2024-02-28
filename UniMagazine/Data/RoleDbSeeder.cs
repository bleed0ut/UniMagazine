using Microsoft.AspNetCore.Identity;
using UniMagazine.Models;

namespace UniMagazine.Data
{
    public static class RoleDbSeeder
    {
        public async static Task CreateRole(IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Manager", "Coordinator", "Student" };

            foreach (var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public async static Task CreateSampleUser(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
            //admin
            string email = "admin@gmail.com";
            string pwd = "Admin@123";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser();
                
                user.FullName = "Admin Sample";
                user.Email = email;
                user.UserName = email;
                user.Address = "Greenwich basement";
                user.EmailConfirmed = true;
                user.DateOfBirth = new DateTime(1999, 1, 1);
                user.Role = "Admin";

                await userManager.CreateAsync(user, pwd);
                await userManager.AddToRoleAsync(user, user.Role);
            }

            //marketing manager
            email = "manager@gmail.com";
            pwd = "Manager@123";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser();
                user.FullName = "Manager Sample";
                user.Email = email;
                user.UserName = email;
                user.Address = "Greenwich basement";
                user.EmailConfirmed = true;
                user.DateOfBirth = new DateTime(1999, 1, 2);
                user.Role = "Manager";

                await userManager.CreateAsync(user, pwd);
                await userManager.AddToRoleAsync(user, user.Role);
            }

            //marketing coordinator
            email = "coordinator@gmail.com";
            pwd = "Coordinator@123";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser();
                user.FullName = "Coordinator Sample";
                user.Email = email;
                user.UserName = email;
                user.Address = "Greenwich basement";
                user.EmailConfirmed = true;
                user.DateOfBirth = new DateTime(1999, 1, 3);
                user.Role = "Coordinator";

                await userManager.CreateAsync(user, pwd);
                await userManager.AddToRoleAsync(user, user.Role);
            }

            //student
            email = "student@gmail.com";
            pwd = "Student@123";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser();
                user.FullName = "Student Sample";
                user.Email = email;
                user.UserName = email;
                user.Address = "Greenwich basement";
                user.EmailConfirmed = true;
                user.DateOfBirth = new DateTime(1999, 1, 4);
                user.Role = "Student";

                await userManager.CreateAsync(user, pwd);
                await userManager.AddToRoleAsync(user, user.Role);
            }
        }
    }
}

// RoleInitializer.cs
using Microsoft.AspNetCore.Identity;
using ST10082744_PROG7311_POE_.Areas.Identity.Data;
using System;
using System.Threading.Tasks;

public static class RoleInitializer
{
    public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager, UserManager<ST10082744_PROG7311_POE_User> userManager)
    {
        var roles = new[] { "Farmer", "Employee" };

        foreach (var role in roles)
        {
            var roleExist = await roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        //creating default employee
        var adminUser = await userManager.FindByEmailAsync("admin@agri.co.za");
        if (adminUser == null)
        {
            var newAdmin = new ST10082744_PROG7311_POE_User
            {
                UserName = "admin@agri.co.za",
                Email = "admin@agri.co.za",

                EmailConfirmed = true
            };
            //admin password
            var result = await userManager.CreateAsync(newAdmin, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Employee");
            }
        }
    }
}

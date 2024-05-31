using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ST10082744_PROG7311_POE_.Areas.Identity.Data;
using ST10082744_PROG7311_POE_.Data;
using ST10082744_PROG7311_POE_.Models;

namespace ST10082744_PROG7311_POE_
{
    public class DbInitializer
    {
        /// <summary>
        /// method to populate farmer information and details
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var context = new ST10082744_PROG7311_POE_Context(
                serviceProvider.GetRequiredService<DbContextOptions<ST10082744_PROG7311_POE_Context>>());

            var userManager = serviceProvider.GetRequiredService<UserManager<ST10082744_PROG7311_POE_User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate();

           
            var roles = new[] { "Employee", "Farmer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //create default farmer user
            var farmerEmail = "koosie@box.com";
            var farmerPassword = "AppelKoos1!";
            if (userManager.Users.All(u => u.Email != farmerEmail))
            {
                var farmerUser = new ST10082744_PROG7311_POE_User
                {
                    UserName = farmerEmail,
                    Email = farmerEmail,
                    Name = "Koos",
                    Surname = "Boks"
                };

                var result = await userManager.CreateAsync(farmerUser, farmerPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(farmerUser, "Farmer");

                    //create default product for the farmer
                    var product = new Product
                    {
                        ProductName = "Milk",
                        Category = "Dairy",
                        ProductionDate = DateTime.UtcNow,
                        UserId = farmerUser.Id
                    };

                    context.Products.Add(product);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
//======================================================================END OF FILE====================================================================//
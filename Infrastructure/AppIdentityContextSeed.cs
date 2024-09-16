using Core.IdentityEntities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure
{
    public class AppIdentityContextSeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Kamal",
                    Email = "kamal@gmail.com",
                    UserName = "KamalAshraf",
                    Address = new Address
                    {
                        FirstName = "Kamal",
                        LastName = "Ashraf",
                        Street = "10",
                        State = "Cairo",
                        City = "Maadi",
                        ZipCode = "11828"
                    }
                };

                var result = await userManager.CreateAsync(user, "Kam00#");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}

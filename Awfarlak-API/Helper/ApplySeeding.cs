using Core;
using Core.IdentityEntities;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Awfarlak_API.Helper
{
    public class ApplySeeding
    {
        public static async Task ApplySeedingAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                try
                {
                    var storeContext = services.GetRequiredService<StoreDbContext>();
                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await storeContext.Database.MigrateAsync();
                    await StoreContextSeed.SeedAsync(storeContext, loggerFactory);
                    await identityContext.Database.MigrateAsync();
                    await AppIdentityContextSeed.SeedAsync(userManager, roleManager);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<ApplySeeding>();
                    logger.LogError(ex, "An error occurred during seeding.");
                }
            }
        }
    }
}

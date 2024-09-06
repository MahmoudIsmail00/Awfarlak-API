using Core;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Awfarlak_API.Extensions
{
    public static class DbContexServiceExtensions
    {
        public static IServiceCollection AddDBContextServices(this IServiceCollection services, IConfiguration _config)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(_config.GetConnectionString("IdentityConnection"));
            });

            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });
            return services;
        }
    }
}

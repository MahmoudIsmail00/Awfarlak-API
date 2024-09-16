
using Awfarlak_API.Extensions;
using Awfarlak_API.Helper;
using Awfarlak_API.Middlewares;

namespace Awfarlak_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            builder.Services.AddDBContextServices(builder.Configuration);

            builder.Services.AddApplicationServices();

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerDocumentation();

            var app = builder.Build();

            await ApplySeeding.ApplySeedingAsync(app);

            app.UseCors("AllowSpecificOrigin");


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiDemo v1");
                });

                app.UseMiddleware<ExceptionMiddleware>();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();


            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseMiddleware<ApiKeyValidationMiddleware>();

            app.MapControllers();


            app.Run();
        }
    }
}

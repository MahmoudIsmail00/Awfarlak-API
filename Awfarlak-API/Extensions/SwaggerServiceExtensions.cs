using Microsoft.OpenApi.Models;

namespace Awfarlak_API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentaion(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiDemo", Version = "v1" });

                var securityschema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer schema. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearer"
                    }
                };

                c.AddSecurityDefinition("bearer", securityschema);

                var securityRequirment = new OpenApiSecurityRequirement
                {
                    {securityschema, new[] { "bearer" } }
                };

                c.AddSecurityRequirement(securityRequirment);
            });

            return services;
        }
    }
}

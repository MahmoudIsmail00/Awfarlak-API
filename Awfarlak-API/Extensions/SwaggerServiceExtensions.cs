using Microsoft.OpenApi.Models;

namespace Awfarlak_API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        private const string Bearer = "Bearer";
        private const string Authorization = "Authorization";
        private const string ApiKeyHeaderName = "X-API-KEY";

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ApiDemo",
                    Version = "v1",
                    Description = "API documentation for the Awfarlak project."
                });

                // JWT Bearer Token
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Description = $"JWT Authorization header using the {Bearer} scheme. Example: \"{Authorization}: {Bearer} {{token}}\"",
                    Name = Authorization,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = Bearer,
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = Bearer
                    }
                };

                // API Key
                //var apiKeySecurityScheme = new OpenApiSecurityScheme
                //{
                //    Description = $"API Key needed to access the endpoints. {ApiKeyHeaderName}: {ApiKeyHeaderName}",
                //    Name = ApiKeyHeaderName,
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    Scheme = ApiKeyHeaderName,
                //    Reference = new OpenApiReference
                //    {
                //        Type = ReferenceType.SecurityScheme,
                //        Id = ApiKeyHeaderName
                //    }
                //};

                c.AddSecurityDefinition(Bearer, jwtSecurityScheme);
                //c.AddSecurityDefinition(ApiKeyHeaderName, apiKeySecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, new List<string>() }
                    //{ apiKeySecurityScheme, new List<string>() }
                });
            });

            return services;
        }
    }
}

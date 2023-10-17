using MicroElements.Swashbuckle.NodaTime;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ImageGallery.API.Infrastructure
{
    public static class SwaggerExtensions
    {
        #region class SwaggerDocumentFilter
        private class SwaggerDocumentFilter : IDocumentFilter
        {
            private readonly string _pathBase;

            public SwaggerDocumentFilter(string pathBase)
            {
                _pathBase = pathBase;
            }
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                swaggerDoc.Servers.Add(new OpenApiServer() { Url = _pathBase });
            }
        }
        #endregion

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration config, string version = "v1")
        {
            services.AddSwaggerGen(o =>
            {
                o.CustomSchemaIds(type => type.ToString());

                var pathBase = config["PATH_BASE"];

                if (!string.IsNullOrEmpty(pathBase))
                {
                    o.DocumentFilter<SwaggerDocumentFilter>(pathBase);
                }

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter your JWT token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // Если используете другую схему, измените на нее
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                o.AddSecurityDefinition("Bearer", securityScheme);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        securityScheme,
                        Array.Empty<string>()
                    }
                };
                o.AddSecurityRequirement(securityRequirement);
           
                o.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = $"{nameof(BookStore)} API",
                    Version = version,
                });
                
                o.ConfigureForNodaTimeWithSystemTextJson();
            });

            return services;
        }
    }
}

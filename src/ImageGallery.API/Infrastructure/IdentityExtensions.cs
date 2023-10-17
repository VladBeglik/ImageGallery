using System.Reflection;
using System.Text;
using EntityFrameworkCore.SqlServer.NodaTime.Extensions;
using ImageGallery.App.Infrastructure;
using ImageGallery.Domain;
using ImageGallery.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ImageGallery.API.Infrastructure;

public static class IdentityExtensions
{
    public static IServiceCollection AddCustomIdentity(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        var connectionString = configuration.GetConnectionString("SqlServer");
        var pathBase = configuration.GetValue<string>("PATH_BASE");

        var migrationsAssembly = typeof(ImageGalleryDbContext).GetTypeInfo().Assembly.GetName().Name;

        var authOptionsConfiguration = configuration.GetSection("Auth");
        services.Configure<AuthOptions>(authOptionsConfiguration);
        var authOptions = authOptionsConfiguration.Get<AuthOptions>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = authOptions.Issuer,

                    ValidateAudience = false,
                    ValidAudience = authOptions.Audience,

                    ValidateLifetime = false,

                    IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
                options.SaveToken = true;
            });
            services
            .AddIdentity<User, IdentityRole>(_ =>
            {
                _.Password.RequireDigit = false;
                _.Password.RequireUppercase = false;
                _.Password.RequireNonAlphanumeric = false;
                _.Password.RequiredLength = 4;
                _.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ImageGalleryDbContext>()
            .AddDefaultTokenProviders();
            
        
                
            
        services
            .AddDbContext<ImageGalleryDbContext>(_ =>
            {
                _.UseSqlServer(connectionString, _ =>
                {
                    _.MigrationsAssembly(migrationsAssembly);
                    _.UseNodaTime();
                    _.EnableRetryOnFailure(2);
                });
                //o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });


        services
            .AddScoped<IImageGalleryDbContext, ImageGalleryDbContext>()
            .AddTransient<ITokenService, TokenService>();

        return services;
    }
}
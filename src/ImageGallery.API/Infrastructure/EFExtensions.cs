using EntityFrameworkCore.SqlServer.NodaTime.Extensions;
using ImageGallery.App.Infrastructure;
using ImageGallery.Persistence;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace ImageGallery.API.Infrastructure
{
    public static class EFExtensions
    {
        public static IServiceCollection AddProjectDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var migrationsAssembly = typeof(IImageGalleryDbContext).Assembly;

            var connectionString = configuration.GetConnectionString("SqlServer");

            services.AddDbContext<ImageGalleryDbContext>(options =>
            {
                options.UseSqlServer(connectionString, o =>
                {
                    o.MigrationsAssembly(migrationsAssembly.GetName().Name);
                    o.EnableRetryOnFailure(15);
                    o.UseNodaTime();
                });

                if (!LogSqlToConsole) return;

                //options.EnableSensitiveDataLogging();
                //options.UseLoggerFactory(GetConsoleLoggerFactory());
            }
            );

            services.AddScoped<IImageGalleryDbContext, ImageGalleryDbContext>();

            return services;
        }

        public static void DatabaseMigrate(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ImageGalleryDbContext>();
            var clock = scope.ServiceProvider.GetRequiredService<IClock>();
            context.Database.Migrate();
            Seed(context);

        }

        private static async void Seed(IImageGalleryDbContext ctx)
        {
            return;

        }

        #region private methods
        private static ILoggerFactory GetConsoleLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
            {
                builder.AddConsole()
                    .AddFilter(level => level >= LogLevel.Warning);
            });
        }
        private static bool LogSqlToConsole = false;

        #endregion
    }
}

using NodaTime;
using NodaTime.Serialization.SystemTextJson;

namespace BookStore.API.Infrastructure;

public static class JsonExtensions
{
    public static IMvcBuilder AddCustomJsonOptions(
        this IMvcBuilder builder,
        IWebHostEnvironment hostingEnvironment) =>
        builder.AddJsonOptions(
            options =>
            {
                options.JsonSerializerOptions.WriteIndented = hostingEnvironment.IsDevelopment();
                options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            });
}
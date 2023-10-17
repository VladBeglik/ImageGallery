using ImageGallery.App.Infrastructure;

namespace ImageGallery.API.Infrastructure;

public static class ImageServiceExtensions
{
    public static IServiceCollection AddLocalStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILocalImageService, LocalImageService>();
        services.Configure<FileServiceOptions>(configuration.GetSection("FileServiceOptions"));

        var storageDirectory = configuration["FileServiceOptions:StorageDirectory"];

        if (Directory.Exists(storageDirectory)) return services;
        
        if (storageDirectory != null) Directory.CreateDirectory(storageDirectory);

        return services;
    }
}

public class FileServiceOptions
{
    public string StorageDirectory { get; set; } 
}
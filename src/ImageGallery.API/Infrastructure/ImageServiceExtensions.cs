using ImageGallery.App.Infrastructure;

namespace ImageGallery.API.Infrastructure;

public static class ImageServiceExtensions
{
    public static IServiceCollection AddLocalStorage(this IServiceCollection services, IConfiguration configuration)
    {

        var fileServiceOptionsConfigurationSection = configuration.GetSection("FileServiceOptions");
        services.Configure<FileOptions>(fileServiceOptionsConfigurationSection);
        var fileOptions = fileServiceOptionsConfigurationSection.Get<FileOptions>();
        var startDirectory = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
        var path = Path.Combine(startDirectory.FullName, fileOptions.StorageDirectory);
        
        if (Directory.Exists(fileOptions.StorageDirectory)) return services;

        if (fileOptions.StorageDirectory != null) Directory.CreateDirectory(path);

        services.AddScoped<ILocalImageService>(_ => new LocalImageService(path));
        
        return services;
    }
}

public class FileOptions
{
    public string StorageDirectory { get; set; } 
}
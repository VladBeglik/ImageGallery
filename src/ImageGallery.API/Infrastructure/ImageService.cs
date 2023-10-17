using ImageGallery.App.Infrastructure;
using Microsoft.Extensions.Options;

namespace ImageGallery.API.Infrastructure;

public class LocalImageService : ILocalImageService
{
    private readonly string _fileDirectory;

    public LocalImageService(IOptions<FileServiceOptions> fileServiceOptions)
    {
        _fileDirectory = fileServiceOptions.Value.StorageDirectory;
    }

    public async Task UploadImage(IFormFile imageFile)
    {
        var filePath = Path.Combine(_fileDirectory, imageFile.FileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
    }

    public async Task<Stream> GetImageAsync(string fileName)
    {
        var filePath = Path.Combine(_fileDirectory, fileName);

        if (!File.Exists(filePath)) return null;
        
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var memoryStream = new MemoryStream();

        await fileStream.CopyToAsync(memoryStream);
        fileStream.Close();
        memoryStream.Position = 0; 
        return memoryStream;

    }
}
using ImageGallery.App.Infrastructure;

namespace ImageGallery.API.Infrastructure;

public class LocalImageService : ILocalImageService
{
    private readonly string _fileDirectory;

    public LocalImageService(string fileDirectory)
    {
        _fileDirectory = fileDirectory;
    }

    public async Task UploadImage(IFormFile imageFile)
    {
        var filePath = Path.Combine(_fileDirectory, imageFile.FileName);
        await using var stream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
    }

    public async Task<string> GetImageAsync(string fileName)
    {
        var filePath = Path.Combine(_fileDirectory, fileName);

        if (!File.Exists(filePath)) return null;

        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
    
        byte[] imageBytes = memoryStream.ToArray();
        string base64String = Convert.ToBase64String(imageBytes);

        return base64String;

    }
}
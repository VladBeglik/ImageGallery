using Microsoft.AspNetCore.Http;

namespace ImageGallery.App.Infrastructure;

public interface ILocalImageService
{
    Task UploadImage(IFormFile imageFile);
    Task<string> GetImageAsync(string fileName);
}
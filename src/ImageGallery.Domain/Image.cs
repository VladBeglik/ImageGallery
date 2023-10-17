using NodaTime;

namespace ImageGallery.Domain;

public class Image
{
    public string Id { get; set; } = null!;
    public string FileName { get; set; }= null!;
    
    public LocalDateTime UploadDate { get; set; }
    public string Description { get; set; }= null!;
    public string UserId { get; set; }= null!;
    public User User { get; set; }= null!;
}
using Microsoft.AspNetCore.Identity;
using NodaTime;

namespace ImageGallery.Domain;

public class User : IdentityUser
{
    public string? RefreshToken { get; set; }
    public LocalDateTime RefreshTokenExpiryTime { get; set; }
    private List<Image> _pictures { get; set; }
    public IReadOnlyCollection<Image> Images => _pictures.AsReadOnly();
    public ICollection<Friendship>? Friendships { get; set; }
}
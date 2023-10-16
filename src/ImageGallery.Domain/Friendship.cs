using Identity.Domain.Enums;
using NodaTime;

namespace ImageGallery.Domain;

public class Friendship
{
    public string UserId { get; set; } = null!;
    public string? FriendId { get; set; }
    public User? Friend { get; set; }
    public FriendshipStatus Status { get; set; }
    public LocalDateTime DateTime { get; set; }
}
using ImageGallery.Domain.Enums;
using NodaTime;

namespace ImageGallery.Domain;

public class Friendship
{
    public string UserId { get; set; }
    public User User { get; set; }

    public string FriendId { get; set; }

    public FriendshipStatus FriendshipStatus { get; set; }
    public LocalDateTime DateTime { get; set; }
}
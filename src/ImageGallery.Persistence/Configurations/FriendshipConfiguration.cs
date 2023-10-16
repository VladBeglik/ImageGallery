using ImageGallery.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageGallery.Persistence.Configurations;

public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder
            .HasKey(f => new { f.UserId, f.FriendId });

        builder
            .HasOne(f => f.User)
            .WithMany(u => u.Friendships)
            .HasForeignKey(f => f.UserId);

        builder
            .HasOne(f => f.Friend)
            .WithMany()
            .HasForeignKey(f => f.FriendId);
    }
}

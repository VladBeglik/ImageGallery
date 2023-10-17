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
            .HasOne(x => x.User)
            .WithMany(x => x.Friendships)
            .HasForeignKey(x => x.FriendId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.User)
            .WithMany(e => e.Friendships)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

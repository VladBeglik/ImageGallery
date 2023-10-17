using ImageGallery.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageGallery.Persistence.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(image => image.Id);

        builder.Property(image => image.FileName)
            .IsRequired();

        builder.Property(image => image.Description)
            .IsRequired();

        builder.Property(image => image.UserId)
            .IsRequired();

        builder.HasOne(image => image.User)
            .WithMany(user => user.Images)
            .HasForeignKey(image => image.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
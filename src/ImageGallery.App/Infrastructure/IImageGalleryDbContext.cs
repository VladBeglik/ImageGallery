using System.Net.Mime;
using ImageGallery.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ImageGallery.App.Infrastructure;

public interface IImageGalleryDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Image> Images { get; set; }
    DbSet<Domain.Friendship> Friendships { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default);
    IExecutionStrategy CreateExecutionStrategy();
}
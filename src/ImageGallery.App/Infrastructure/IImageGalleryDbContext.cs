using System.Net.Mime;
using ImageGallery.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ImageGallery.App.Infrastructure;

public interface IImageGalleryDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default);
    IExecutionStrategy CreateExecutionStrategy();
}
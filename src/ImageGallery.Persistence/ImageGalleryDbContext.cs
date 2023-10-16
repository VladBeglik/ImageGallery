using System.Reflection;
using ImageGallery.App.Infrastructure;
using ImageGallery.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace ImageGallery.Persistence;

public class ImageGalleryDbContext: DbContext, IImageGalleryDbContext
{
    
    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    
    #region Fields

    private readonly IClock _clock;
    private readonly ILogger<ImageGalleryDbContext> _logger;

    #endregion
    
    #region Ctors

    public ImageGalleryDbContext(ILogger<ImageGalleryDbContext> logger, IClock clock)
    {
        _logger = logger;
        _clock = clock;
    }    
    
    public ImageGalleryDbContext(DbContextOptions<ImageGalleryDbContext> options, ILogger<ImageGalleryDbContext> logger, IClock clock) : base(options)
    {
        _logger = logger;
        _clock = clock;
    }

    #endregion

    #region Methods

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = new())
    {
        return Database.BeginTransactionAsync(token);
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return Database.CreateExecutionStrategy();
    }
    
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    #endregion

}

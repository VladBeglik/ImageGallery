using ImageGallery.App.Infrastructure;
using ImageGallery.App.Infrastructure.Exceptions;
using ImageGallery.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace ImageGallery.App.Images.Commands;

public class UploadImageCommand : IRequest
{
    public IFormFile File { get; set; } = null!;
    public string? Description { get; set; }
}

public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand>
{
    private readonly IImageGalleryDbContext _ctx;
    private readonly ICurrentUserService _currentUser;
    private readonly IClock _clock;
    private readonly ILocalImageService _localImageService;


    public UploadImageCommandHandler(IImageGalleryDbContext ctx, ICurrentUserService currentUser, IClock clock, ILocalImageService localImageService)
    {
        _ctx = ctx;
        _currentUser = currentUser;
        _clock = clock;
        _localImageService = localImageService;
    }

    public async Task<Unit> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenException();
        }

        var image = new Image
        {
            UserId = _currentUser.UserId,
            Id = Guid.NewGuid().ToString(),
            Description = request.Description,
            FileName = request.File.FileName,
            UploadDate = _clock.GetNow()
        };

        _ctx.Images.Add(image);
        
        await _ctx.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            await using var tr = await _ctx.BeginTransactionAsync(cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            _localImageService.UploadImage(request.File);
            await tr.CommitAsync(cancellationToken);
        });

        return Unit.Value;

    }
}
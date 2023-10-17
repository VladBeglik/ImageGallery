using ImageGallery.App.Infrastructure;
using ImageGallery.App.Infrastructure.Exceptions;
using MediatR;
using NodaTime;

namespace ImageGallery.App.Images.Commands;

public class GetImageCommand : IRequest<Stream> 
{
    public string Id { get; set; }
}

public class GetImageCommandHandler : IRequestHandler<GetImageCommand, Stream>
{
    private readonly IImageGalleryDbContext _ctx;
    private readonly ICurrentUserService _currentUser;
    private readonly IClock _clock;
    private readonly ILocalImageService _localImageService;

    public GetImageCommandHandler(IImageGalleryDbContext ctx, ICurrentUserService currentUser, IClock clock, ILocalImageService localImageService)
    {
        _ctx = ctx;
        _currentUser = currentUser;
        _clock = clock;
        _localImageService = localImageService;
    }

    public async Task<Stream> Handle(GetImageCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenException();
        }

        var image = await _localImageService.GetImageAsync(request.Id);
        return image;
    }
}
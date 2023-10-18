using ImageGallery.App.Infrastructure;
using ImageGallery.App.Infrastructure.Exceptions;
using ImageGallery.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace ImageGallery.App.Images.Commands;

public class GetImageCommand : IRequest<string> 
{
    public string Id { get; set; }
    public string? FriendId { get; set; }
}

public class GetImageCommandHandler : IRequestHandler<GetImageCommand, string>
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

    public async Task<string> Handle(GetImageCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenException();
        }

        if (!string.IsNullOrWhiteSpace(request.FriendId))
        {
            var friend = await _ctx.Friendships
                .Include(_ => _.User)
                .Where(_ => _.UserId == userId && _.FriendId == request.FriendId &&
                            _.FriendshipStatus == FriendshipStatus.Accepted)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (friend == default)
            {
                throw new ForbiddenException();
            }
        }
        
        var image = await _ctx.Images.FirstOrDefaultAsync(_ => _.Id == request.Id && _.UserId == userId, cancellationToken: cancellationToken);
        
        if (image == default)
            throw new CustomException();
        
        var base64Image = await _localImageService.GetImageAsync(image.FileName);
        return base64Image;
    }
}
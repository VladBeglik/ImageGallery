using ImageGallery.App.Infrastructure;
using ImageGallery.App.Infrastructure.Exceptions;
using ImageGallery.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace ImageGallery.App.Friendship.Commands;

public class DeclineAddToFriendCommand : IRequest
{
    public string FriendId { get; set; } = null!;
}

public class DeclineAddToFriendCommandHandler : IRequestHandler<DeclineAddToFriendCommand>
{
    private readonly IImageGalleryDbContext _ctx;
    private readonly ICurrentUserService _currentUser;
    private readonly IClock _clock;

    public DeclineAddToFriendCommandHandler(IImageGalleryDbContext ctx, ICurrentUserService currentUser, IClock clock)
    {
        _ctx = ctx;
        _currentUser = currentUser;
        _clock = clock;
    }

    public async Task Handle(DeclineAddToFriendCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenException();
        }

        var user = await _ctx.Users.FirstOrDefaultAsync(_ => _.Id == userId, cancellationToken: cancellationToken);
        var friend = await _ctx.Users.FirstOrDefaultAsync(_ => _.Id == request.FriendId, cancellationToken: cancellationToken);

        if (friend == default)
            throw new CustomException();

        var friendRequest = await _ctx.Friendships.FirstOrDefaultAsync(_ =>
            _.UserId == request.FriendId && _.FriendId == userId && _.FriendshipStatus == FriendshipStatus.Pending, cancellationToken: cancellationToken);

        if (friendRequest == default)
            throw new CustomException();

        friendRequest.FriendshipStatus = FriendshipStatus.Declined;
        friendRequest.DateTime = _clock.GetNow();

        await _ctx.SaveChangesAsync(cancellationToken);
    }
}
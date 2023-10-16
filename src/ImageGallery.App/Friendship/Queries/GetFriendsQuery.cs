using ImageGallery.App.Infrastructure;
using ImageGallery.App.Infrastructure.Exceptions;
using ImageGallery.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ImageGallery.App.Friendship.Queries;

public class GetFriendsQuery : IRequest<string[]>
{
    
}

public class GetFriendsQueryHandler : IRequestHandler<GetFriendsQuery, string[]>
{
    private readonly IImageGalleryDbContext _ctx;
    private readonly ICurrentUserService _currentUser;

    public GetFriendsQueryHandler(IImageGalleryDbContext ctx, ICurrentUserService currentUser)
    {
        _ctx = ctx;
        _currentUser = currentUser;
    }

    public async Task<string[]> Handle(GetFriendsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenException();
        }

        var friendIds = await _ctx.Friendships
            .Where(_ => _.UserId == userId && _.FriendshipStatus == FriendshipStatus.Accepted)
            .Select(_ => _.FriendId)
            .ToArrayAsync(cancellationToken);

        return friendIds;
    }
}
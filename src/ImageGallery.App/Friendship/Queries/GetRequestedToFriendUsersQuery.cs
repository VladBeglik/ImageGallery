﻿using ImageGallery.App.Infrastructure;
using ImageGallery.App.Infrastructure.Exceptions;
using ImageGallery.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ImageGallery.App.Friendship.Queries;

public class GetRequestedToFriendUsersQuery : IRequest<string[]>
{
    
}

public class GetRequestedToFriendUsersQueryHandler : IRequestHandler<GetRequestedToFriendUsersQuery, string[]>
{
    private readonly IImageGalleryDbContext _ctx;
    private readonly ICurrentUserService _currentUser;

    public GetRequestedToFriendUsersQueryHandler(IImageGalleryDbContext ctx, ICurrentUserService currentUser)
    {
        _ctx = ctx;
        _currentUser = currentUser;
    }

    public async Task<string[]> Handle(GetRequestedToFriendUsersQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenException();
        }

        var friendIds = await _ctx.Friendships
            .Where(_ => _.FriendId == userId && _.FriendshipStatus == FriendshipStatus.Pending)
            .Select(_ => _.FriendId)
            .ToArrayAsync(cancellationToken);

        return friendIds;

    }
}
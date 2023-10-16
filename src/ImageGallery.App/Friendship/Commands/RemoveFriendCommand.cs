using ImageGallery.App.Infrastructure;
using ImageGallery.App.Infrastructure.Exceptions;
using ImageGallery.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace ImageGallery.App.Friendship.Commands;

public class RemoveFriendCommand : IRequest
{
    public string FriendId { get; set; } = null!;
}

public class RemoveFriendCommandHandler : IRequestHandler<RemoveFriendCommand>
{
    private readonly IImageGalleryDbContext _ctx;
    private readonly ICurrentUserService _currentUser;
    private readonly IClock _clock;

    public RemoveFriendCommandHandler(IImageGalleryDbContext ctx, ICurrentUserService currentUser, IClock clock)
    {
        _ctx = ctx;
        _currentUser = currentUser;
        _clock = clock;
    }

    public async Task Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
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

        if (!await _ctx.Friendships.AnyAsync(_ => _.UserId == userId && _.FriendId == friend.Id && _.FriendshipStatus == FriendshipStatus.Pending, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException("Заявка в друзья уже отправлена этому пользователю.");
        }
        
        if (friend.Id == userId)
        {
            throw new InvalidOperationException("Вы не можете отправить заявку в друзья самому себе.");
        }

        var friendship = await _ctx.Friendships.FirstOrDefaultAsync(_ =>
            _.FriendshipStatus == FriendshipStatus.Accepted && _.UserId == userId && _.FriendId == friend.Id, cancellationToken: cancellationToken);

        if (friendship == default)
            throw new CustomException();

        _ctx.Friendships.Remove(friendship);

        await _ctx.SaveChangesAsync(cancellationToken);
    }
}
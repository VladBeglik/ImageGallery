using ImageGallery.App.Infrastructure.Exceptions;
using ImageGallery.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ImageGallery.App.Auth;

public class RevokeCommand : IRequest
{
    public string UserId { get; set; } = null!;
}

public class RevokeCommandHandler : IRequestHandler<RevokeCommand>
{
    private readonly UserManager<User> _userManager;

    public RevokeCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }


    public async Task<Unit> Handle(RevokeCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null) throw new CustomException("Invalid user name");

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
        return Unit.Value;
    }
}
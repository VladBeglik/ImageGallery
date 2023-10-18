using System.Security.Claims;
using IdentityModel;
using ImageGallery.App.Infrastructure;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ImageGallery.API.Infrastructure;

public class CurrentUserService : ICurrentUserService
{

    #region Ctor
    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IImageGalleryDbContext ctx)
    {
        var user = httpContextAccessor?.HttpContext?.User;
        if (user != default && (user.Identity?.IsAuthenticated ?? false))
        {
            IsAuthenticated = true;
            UserName = user.FindFirstValue(JwtClaimTypes.Name)!;
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
    #endregion

    #region Props
    public string? UserId { get; }
    public string? UserName { get; }
    public bool IsAuthenticated { get; }

    #endregion

}

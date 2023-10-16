using System.Security.Claims;
using IdentityModel;
using ImageGallery.App.Infrastructure;

namespace ImageGallery.API.Infrastructure;

public class CurrentUserService : ICurrentUserService
{
    #region Fields
    private readonly IImageGalleryDbContext _ctx;
    #endregion

    #region Ctor
    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IImageGalleryDbContext ctx)
    {
        _ctx = ctx;
        var user = httpContextAccessor?.HttpContext?.User;
        if (user != default && (user.Identity?.IsAuthenticated ?? false))
        {
            IsAuthenticated = true;
            UserName = user.FindFirstValue(JwtClaimTypes.PreferredUserName)!;
            UserId = user.FindFirstValue(JwtClaimTypes.Subject)!;
        }

    }
    #endregion

    #region Props
    public string? UserId { get; }
    public string? UserName { get; }
    public bool IsAuthenticated { get; }

    #endregion

}

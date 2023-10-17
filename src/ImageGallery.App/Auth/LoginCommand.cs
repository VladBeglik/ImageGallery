using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
using ImageGallery.App.Infrastructure;
using ImageGallery.Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NodaTime;

namespace ImageGallery.App.Auth;

public class LoginCommand : IRequest<IActionResult>
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, IActionResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IClock _clock;
    private readonly ITokenService _tokenService; 

    public LoginCommandHandler(UserManager<User> userManager, IConfiguration configuration, IClock clock, ITokenService tokenService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _clock = clock;
        _tokenService = tokenService;
    }

    public async Task<IActionResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username!);
        
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password!))
            return new UnauthorizedResult();
        
        var authClaims = new List<Claim>
        {
            new(JwtClaimTypes.Subject, user.Id),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
        };
            
        var token = _tokenService.GenerateAccessToken(authClaims);
        var refreshToken = _tokenService.GenerateRefreshToken();


        user.RefreshToken = refreshToken;

        await _userManager.UpdateAsync(user);

        return new OkObjectResult(new
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
        });
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ImageGallery.App.Infrastructure;

public interface ITokenService
{
    JwtSecurityToken  GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}

public class TokenService : ITokenService
{
    private readonly IOptions<AuthOptions> _authOptions;
    public TokenService(IOptions<AuthOptions> authOptions)
    {
        _authOptions = authOptions;
    }

    public JwtSecurityToken  GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var authSigningKey = _authOptions.Value.GetSymmetricSecurityKey();

        var token = new JwtSecurityToken(

            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
    
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _authOptions.Value.GetSymmetricSecurityKey(),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}
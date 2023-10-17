using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ImageGallery.App.Infrastructure;

public class AuthOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Secret { get; set; }
    public TimeSpan ExpireTime { get; set; }
    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
    }
}
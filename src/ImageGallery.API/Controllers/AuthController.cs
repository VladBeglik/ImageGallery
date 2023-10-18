using ImageGallery.App.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.API.Controllers;

public class AuthController : MediatrController
{

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginCommand model)
    {
        var result = await Mediator.Send(model);
        return result;
    }

    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]

    public async Task<IActionResult> Register([FromBody] RegistrationCommand model)
    {
        var result = await Mediator.Send(model);
        return result;
    }

    [HttpPost]
    [Route("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]

    public async Task<IActionResult> RefreshToken(RefreshTokenCommand tokenModel)
    {
        var result = await Mediator.Send(tokenModel);
        return result;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string userId)
    {
        await Mediator.Send(new RevokeCommand{UserId = userId});
        return NoContent();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    [Route("revoke-all")]
    public async Task<IActionResult> RevokeAll()
    {
        await Mediator.Send(new RevokeAllCommand());
        return NoContent();
    }
}
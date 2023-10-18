using ImageGallery.App.Images.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.API.Controllers;


[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ImageController : MediatrController
{
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadImage(UploadImageCommand r)
    {
        await Mediator.Send(r);
        return Ok();
    }

    [HttpPost]
    [Route("/getImage")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetImage(GetImageCommand r)
    {
        var res = await Mediator.Send(r);
        return new JsonResult(res);
    }
}
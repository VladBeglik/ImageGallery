using ImageGallery.App.Images.Commands;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.API.Controllers;

public class ImageController : MediatrController
{
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadImage(UploadImageCommand r)
    {
        await Mediator.Send(r);
        return Ok();
    }

    [HttpGet("{imageId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetImage(GetImageCommand r)
    {
        var res = await Mediator.Send(r);
        return Ok(res);
    }
}
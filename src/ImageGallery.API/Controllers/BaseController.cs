using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.API.Controllers;


[Route("api/[controller]")]
public class MediatrController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}
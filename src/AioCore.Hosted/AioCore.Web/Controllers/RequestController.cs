using AioCore.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AioCore.Web.Controllers;

[ApiController]
[Route("[action]")]
public class RequestController : ControllerBase
{
    private readonly IMediator _mediator;

    public RequestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Pixel([FromQuery] PixelRequest request)
    {
        var response = await _mediator.Send(request);
        return File(response, "image/gif");
    }
}
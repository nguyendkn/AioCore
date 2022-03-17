using AioCore.Web.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AioCore.Web;

[Route("/")]
public class Startup : ControllerBase
{
    private readonly IMediator _mediator;

    public Startup(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return await Get("index");
    }

    [HttpGet("{router}")]
    public async Task<IActionResult> Get(string? router)
    {
        var response = await _mediator.Send(new RenderRequest(router));

        return new ContentResult
        {
            Content = response,
            ContentType = "text/html",
            StatusCode = 200
        };
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Objects.MediatrRequests.Commands;

namespace AioCore.Web.Controllers;

[Route("/")]
public class IndexController : ControllerBase
{
    private readonly IMediator _mediator;

    public IndexController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return new ContentResult
        {
            Content = await _mediator.Send(new LoadTemplateCommand("/")),
            ContentType = "text/html"
        };
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        return new ContentResult
        {
            Content = await _mediator.Send(new LoadTemplateCommand(slug)),
            ContentType = "text/html"
        };
    }
}
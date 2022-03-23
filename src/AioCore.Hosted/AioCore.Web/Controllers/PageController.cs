using AioCore.Web.Application.Commands;
using AioCore.Web.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AioCore.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PageController : ControllerBase
{
    private readonly IMediator _mediator;

    public PageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] GetPageQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ListPageQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePageCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}
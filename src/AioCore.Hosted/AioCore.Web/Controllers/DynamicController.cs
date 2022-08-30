using AioCore.Read.DynamicQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AioCore.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DynamicController : ControllerBase
{
    private readonly IMediator _mediator;

    public DynamicController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // [HttpPost("create")]
    // public async Task<IActionResult> CreateEntity([FromBody] CreateDynamicEntityCommand request)
    // {
    //     return Ok(await _mediator.Send(request));
    // }

    // [HttpPost("update")]
    // public async Task<IActionResult> UpdateEntity([FromBody] UpdateDynamicEntityCommand request)
    // {
    //     return Ok(await _mediator.Send(request));
    // }

    // [HttpPost("remove")]
    // public async Task<IActionResult> DeleteEntity(RemoveDynamicEntityCommand request)
    // {
    //     return Ok(await _mediator.Send(request));
    // }

    // [HttpGet("entity")]
    // public async Task<IActionResult> GetEntity([FromQuery] Guid id)
    // {
    //     return Ok(await _mediator.Send(new GetDynamicEntityQuery(id)));
    // }

    [HttpPost("filter")]
    public async Task<IActionResult> FilterEntity([FromBody] FilterDynamicEntityQuery request)
    {
        return Ok(await _mediator.Send(request));
    }
}
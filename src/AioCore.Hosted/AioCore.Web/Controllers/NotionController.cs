using System.Net;
using AioCore.Read.DynamicQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AioCore.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class NotionController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{database}")]
    public async Task<IActionResult> Database(string database)
    {
        return Ok(await _mediator.Send(new GetNotionEntityQuery(database)));
    }

    [HttpGet("{page}")]
    public async Task<IActionResult> Page(string page)
    {
        var response = await _mediator.Send(new GetNotionPageQuery(page));
        return await Task.FromResult(new ContentResult
        {
            ContentType = "text/html",
            StatusCode = (int)HttpStatusCode.OK,
            Content = response.Data
        });
    }
}
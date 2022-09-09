using AioCore.Services.GraphQueries;
using Microsoft.AspNetCore.Mvc;

namespace AioCore.Web.Controllers;

[ApiController]
public class DynamicController : ControllerBase
{
    private readonly IGraphService _graphService;

    public DynamicController(IGraphService graphService)
    {
        _graphService = graphService;
    }

    [HttpPost("graph")]
    public async Task<IActionResult> Submit([FromBody] GraphRequest request)
    {
        return Ok(await _graphService.ExecuteAsync(request));
    }
}
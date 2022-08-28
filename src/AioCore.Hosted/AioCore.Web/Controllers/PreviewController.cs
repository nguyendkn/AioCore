using System.Net;
using AioCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace AioCore.Web.Controllers;

[ApiController]
public class PreviewController : ControllerBase
{
    private readonly IPreviewService _previewService;

    public PreviewController(IPreviewService previewService)
    {
        _previewService = previewService;
    }

    [HttpGet("preview")]
    public async Task<ContentResult> Preview([FromQuery] string? path = null)
    {
        return await Task.FromResult(new ContentResult
        {
            ContentType = "text/html",
            StatusCode = (int)HttpStatusCode.OK,
            Content = await _previewService.Preview(path, indexPage: string.IsNullOrEmpty(path))
        });
    }
}
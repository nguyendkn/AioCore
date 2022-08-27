using System.Net;
using AioCore.Blazor.Template;
using AioCore.Shared.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace AioCore.Web.Controllers;

[ApiController]
[Route("render")]
public class IndexController : ControllerBase
{
    private readonly IRazorEngine _razorEngine;
    private readonly AppSettings _appSettings;

    public IndexController(IRazorEngine razorEngine, AppSettings appSettings)
    {
        _razorEngine = razorEngine;
        _appSettings = appSettings;
    }

    [HttpGet]
    public async Task<ContentResult> Render()
    {
        var template = await _razorEngine.CompileAsync("<h1>Hello @Model.Name</h1>");

        var actual = await template.RunAsync(new
        {
            Name = "Alex"
        });
        return await Task.FromResult(new ContentResult
        {
            ContentType = "text/html",
            StatusCode = (int)HttpStatusCode.OK,
            Content = actual
        });
    }
}
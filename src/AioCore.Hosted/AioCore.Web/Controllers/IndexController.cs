using System.Net;
using AioCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace AioCore.Web.Controllers;

[ApiController]
[Route("/")]
public class IndexController : ControllerBase
{
    private readonly ITemplateService _templateService;

    public IndexController(ITemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var htmlCode = await _templateService.Render("index", indexPage: true);
        return await Task.FromResult(new ContentResult
        {
            ContentType = "text/html",
            StatusCode = (int)HttpStatusCode.OK,
            Content = htmlCode
        });
    }

    // [HttpGet("{first}/{second}")]
    // public async Task<IActionResult> Index(string first, string second)
    // {
    //     var htmlCode = await _templateService.Render(first, second);
    //     return await Task.FromResult(new ContentResult
    //     {
    //         ContentType = "text/html",
    //         StatusCode = (int)HttpStatusCode.OK,
    //         Content = htmlCode
    //     });
    // }
}
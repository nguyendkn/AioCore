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
        var htmlCode = await _templateService.Render("index", true);
        return await Task.FromResult(new ContentResult
        {
            ContentType = "text/html",
            StatusCode = (int)HttpStatusCode.OK,
            Content = htmlCode
        });
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Index(string slug)
    {
        var pathType = slug.Split("/").FirstOrDefault();
        var htmlCode = await _templateService.Render(pathType);
        return await Task.FromResult(new ContentResult
        {
            ContentType = "text/html",
            StatusCode = (int)HttpStatusCode.OK,
            Content = htmlCode
        });
    }
}
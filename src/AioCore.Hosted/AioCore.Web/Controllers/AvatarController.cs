using AioCore.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace AioCore.Web.Controllers;

[ApiController]
[Route("avatar")]
public class AvatarController : ControllerBase
{
    private readonly IAvatarService _avatarService;

    public AvatarController(IAvatarService avatarService)
    {
        _avatarService = avatarService;
    }

    [HttpGet("{dimension:range(10,500)=50}/{text?}")]
    public IActionResult Avatar(int dimension, string text)
    {
        return File(_avatarService.Generate(dimension, text), "image/jpeg");
    }
}
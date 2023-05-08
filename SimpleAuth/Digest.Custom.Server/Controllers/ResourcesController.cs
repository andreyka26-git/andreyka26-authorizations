using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Digest.Custom.Server.Controllers;

[ApiController]
public class ResourcesController : ControllerBase
{
    [HttpGet("api/resources")]
    [Authorize(AuthenticationSchemes = "Digest")]
    public IActionResult GetResources()
    {
        return Ok($"protected resources, username: {User.Identity!.Name}");
    }
}

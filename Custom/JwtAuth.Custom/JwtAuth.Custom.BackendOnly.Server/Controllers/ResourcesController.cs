using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.Custom.BackendOnly.Server.Controllers
{
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        [HttpGet("api/resources")]
        // [Authorize()]
        public IActionResult GetResources()
        {
            Request.Cookies.TryGetValue("auth_token", out var token);
            Console.WriteLine(token);
            return Ok($"protected resources, username: {User?.Identity?.Name}, token: {token}");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerWIthIdentity.AuthorizationClient.Controllers
{
    [ApiController]
    [Authorize]
    [Route("resource-api")]
    public class ResourceController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var request = Request;
            return Ok("Got message from protected ednpoint");
        }
    }
}
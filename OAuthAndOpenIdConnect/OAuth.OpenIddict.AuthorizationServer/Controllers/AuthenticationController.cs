using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

namespace OAuth.OpenIddict.AuthorizationServer.Controllers
{
    /// <summary>
    /// This is commented section for reason
    /// Actually we can play with html and leave kind of "plain backend"
    /// but it is hard to play with antiforgery token with this kind of approach
    /// So I did this behavior in Razor Pages.
    /// </summary>
    // [ApiController]
    // [Route("backend")]
    // public class AuthenticationController : Controller
    // {
    //     [HttpGet("html")]
    //     public async Task<IActionResult> GetAuthenticateHtml()
    //     {
    //         var html = 
    //             $"<html>" +
    //             $"<form action=\"/backend/authenticate\" method=\"post\">" +
    //                 $"<input name=\"email\" value=\"{Consts.Email}\"/>" +
    //                 $"<input name=\"password\" value=\"{Consts.Password}\" />" +
    //                 $"<input type=\"submit\" />" +
    //             $"</form>" +
    //             $"</html>";
    //
    //         return Content(html, MediaTypeNames.Text.Html, Encoding.UTF8);
    //     }
    //     
    //     [HttpPost("authenticate")]
    //     public async Task<IActionResult> Authenticate([FromForm] IFormCollection formCollection)
    //     {
    //         var email = formCollection["email"];
    //         var password = formCollection["password"];
    //
    //         if (email != Consts.Email || password != Consts.Password)
    //         {
    //             return Unauthorized();
    //         }
    //
    //         var claims = new List<Claim>
    //         {
    //             new Claim(ClaimTypes.Email, email),
    //         };
    //
    //         var principal = new ClaimsPrincipal(
    //             new List<ClaimsIdentity> 
    //             {
    //                 new ClaimsIdentity(claims)
    //             });
    //
    //         await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    //
    //         return Ok();
    //     }
    // }
}

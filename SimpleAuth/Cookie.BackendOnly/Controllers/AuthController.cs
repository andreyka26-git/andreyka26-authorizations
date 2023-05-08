using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cookie.BackendOnly.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    
    [HttpPost("auth/login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto req)
    {
        var user = await AuthenticateUser(req.UserName, req.Password);

        if (user == null)
        {
            return BadRequest("Cannot authenticate user");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim("FullName", user.FullName),
            new Claim(ClaimTypes.Role, "Administrator"),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties();

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Ok();
    }

    [HttpGet("auth/logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }

    private async Task<User?> AuthenticateUser(string email, string password)
    {
        // For demonstration purposes, authenticate a user
        // with a static email address. Ignore the password.
        // Assume that checking the database takes 500ms

        await Task.Delay(500);

        if (email == "andreyka26_" && password == "Mypass1*")
        {
            return new User()
            {
                Email = "andreyka26_",
                FullName = "Maria Rodriguez"
            };
        }
        else
        {
            return null;
        }
    }
}

public class LoginDto 
{
    public string UserName { get; set; } = "andreyka26_";
    public string Password { get; set; } = "Mypass1*";
}
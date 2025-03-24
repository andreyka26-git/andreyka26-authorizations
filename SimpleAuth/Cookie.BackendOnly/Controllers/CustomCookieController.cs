using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cookie.BackendOnly.Controllers
{
    [Route("api/cookie")]
    [ApiController]
    public class CustomCookieController : ControllerBase
    {
        private const string JwtIssuer = "andreyka26";
        private const string JwtAudience = "audience";
        private const string Key = "some_super_secret_key_over_here_andreyka26_ensuring_enough_bits";

        [HttpPost("customcookie/login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto req)
        {
            var user = await BackendOnly.User.AuthenticateUser(req.UserName, req.Password);
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

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JwtIssuer,
                audience: JwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            Response.Cookies.Append("myAuthCookie", tokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddHours(1)
            });

            return Ok();
        }

        [HttpGet("auth/validate")]
        public IActionResult ValidateToken()
        {
            if (!Request.Cookies.TryGetValue("myAuthCookie", out var tokenString))
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = JwtIssuer,
                ValidAudience = JwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key))
            };

            try
            {
                var principal = tokenHandler.ValidateToken(tokenString, validationParameters, out var validatedToken);

                var claims = principal.Claims.Select(c => new
                {
                    Type = c.Type,
                    Value = c.Value
                });

                return Ok(new
                {
                    Message = "Token is valid",
                    Claims = claims
                });
            }
            catch
            {
                return Unauthorized();
            }
        }
    }
}

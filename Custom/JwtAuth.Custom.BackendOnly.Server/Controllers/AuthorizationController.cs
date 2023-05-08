using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuth.Custom.BackendOnly.Server.Dto;
using JwtAuth.Custom.BackendOnly.Server.Exceptions;

namespace JwtAuth.Custom.BackendOnly.Server.Controllers
{
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly AuthContext _authContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;

        private readonly IConfiguration _configuration;

        public AuthorizationController(UserManager<IdentityUser> userManager,
            AuthContext authContext,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            IUserStore<IdentityUser> userStore)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _authContext = authContext;

            _emailStore = (IUserEmailStore<IdentityUser>)userStore;
            _userStore = userStore;
        }

        [HttpPost("authorization/token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] GetTokenRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            
            if (user == null)
            {
                //401 or 404
                return Unauthorized();
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordValid)
            {
                //401 or 400
                return Unauthorized();
            }

            var resp = await GenerateAuthorizationTokenAsync(user.Id, user.UserName);

            return Ok(resp);
        }

        [HttpPost("authorization/refresh")]
        public async Task<ActionResult<AuthorizationResponse>> GetAuthorizationTokenFromRefreshAsync([FromBody] RefreshTokenRequest request,
            CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var refreshToken = await _authContext.RefreshTokens
                    .Include(r => r.User)
                    .SingleOrDefaultAsync(r => r.Value == request.RefreshToken, cancellationToken);

            if (refreshToken == null)
                throw new Exception("refresh token is not found");

            var response = await GenerateAuthorizationTokenAsync(refreshToken.User.Id, refreshToken.User.UserName);

            return Ok(response);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("authorization/external-login")]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl, CancellationToken cancellationToken)
        {
            var redirectUrl = $"https://localhost:7000/authorization/external-auth-callback?returnUrl={returnUrl}";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            properties.AllowRefresh = true;
            properties.RedirectUri = redirectUrl;

            return Challenge(properties, provider);
        }

        [HttpGet]
        [Route("authorization/external-auth-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl, string? remoteError = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return Unauthorized();
            }

            var emailClaim = info.Principal.HasClaim(c => c.Type == ClaimTypes.Email) ? info.Principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value : string.Empty;
            
            if (string.IsNullOrEmpty(emailClaim))
            {
                return BadRequest();
            }

            var uri = new Uri(returnUrl);

            IdentityUser user;
            try
            {
                user = await ExternalSignInAsync(emailClaim, info.LoginProvider, info.ProviderKey);
            }
            catch (ExternalUserDoesNotExistException)
            {
                try
                {
                    await CreateUserFromExternalAuthAsync(emailClaim, info);

                    user = await ExternalSignInAsync(emailClaim, info.LoginProvider, info.ProviderKey);
                }
                catch (Exception ex)
                {
                    returnUrl = AddQueryParameter(returnUrl, $"errorCode={ex.Message}");
                    return Redirect(returnUrl);
                }
            }

            var token = await GenerateAuthorizationTokenAsync(user.Id, user.UserName);

            var tokenJson = JsonConvert.SerializeObject(token);
            var bytes = Encoding.UTF8.GetBytes(tokenJson);
            var encodedToken = Convert.ToBase64String(bytes);
            returnUrl = AddQueryParameter(returnUrl, $"token={encodedToken}");

            return Redirect(returnUrl);

            string AddQueryParameter(string url, string parameter)
            {
                return uri.Query.Length == 0 ? $"{url}?{parameter}" : $"{url}&{parameter}";
            }
        }

        private async Task CreateUserFromExternalAuthAsync(string email, UserLoginInfo info)
        {
            var user = new IdentityUser();

            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, email, CancellationToken.None);

            var createResult = await _userManager.CreateAsync(user);

            if (!createResult.Succeeded)
            {
                throw new Exception($"User manager cannot create user.");
            }

            await _userManager.AddClaimAsync(user, new Claim("email", email));
            var result = await _userManager.AddLoginAsync(user, info);

            if (!result.Succeeded)
            {
                throw new Exception($"Cannot add external login to user.");
            }
        }

        private async Task<IdentityUser> ExternalSignInAsync(string email, string loginProvider, string providerKey)
        {
            var signInResult = await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, false);

            if (!signInResult.Succeeded)
            {
                throw new ExternalUserDoesNotExistException();
            }

            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        private async Task<AuthorizationResponse> GenerateAuthorizationTokenAsync(string userId, string userName)
        {
            var now = DateTime.UtcNow;

            var secret = _configuration.GetValue<string>("Secret");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            var userClaims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimTypes.NameIdentifier, userId),
            };

            //userClaims.AddRange(roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));

            var expires = now.Add(TimeSpan.FromMinutes(60));

            var jwt = new JwtSecurityToken(
                    notBefore: now,
                    claims: userClaims,
                    expires: expires,
                    audience: "https://localhost:7000/",
                    issuer: "https://localhost:7000/",
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            //we don't know about thread safety of token handler
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var refreshToken = await _authContext.RefreshTokens.SingleOrDefaultAsync(r => r.ApplicationUserId == userId);

            if (refreshToken != null)
            {
                _authContext.RefreshTokens.Remove(refreshToken);
            }

            var user = await _authContext.Users.SingleOrDefaultAsync(u => u.Id == userId);

            var newRefreshToken = new RefreshToken(Guid.NewGuid().ToString(), TimeSpan.FromDays(1000), userId);
            newRefreshToken.User = user;

            _authContext.RefreshTokens.Add(newRefreshToken);

            await _authContext.SaveChangesAsync();

            var resp = new AuthorizationResponse
            {
                UserId = userId,
                AuthorizationToken = encodedJwt,
                RefreshToken = newRefreshToken.Value
            };

            return resp;
        }
    }
}

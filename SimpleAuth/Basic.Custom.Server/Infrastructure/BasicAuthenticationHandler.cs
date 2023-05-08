using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Basic.Custom.Server.Application;

namespace Basic.Custom.Server.Infrastructure;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
        ) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Response.Headers.Add(Consts.AuthenticationInfoHeaderName, Consts.Scheme);

        if (!Request.Headers.ContainsKey(Consts.AuthorizationHeaderName))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authorization header missing."));
        }

        var authorizationHeader = Request.Headers[Consts.AuthorizationHeaderName].ToString();
        var authHeaderValues = authorizationHeader.Split(' ', 2);

        var base64Value = authHeaderValues[1];

        var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(base64Value));
        var authSplit = authBase64.Split(':', 2);

        var username = authSplit[0];
        var password = authSplit[1];

        EnsureAuthenticated(username, password);

        var authenticatedUser = new AuthenticatedUser(Consts.Scheme, true, username);
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));

        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
    }

    private void EnsureAuthenticated(string userName, string password)
    {
        if (userName != "andreyka26_" || password != "mypass1")
        {
            throw new Exception("Unknown user");
        }
    }
}

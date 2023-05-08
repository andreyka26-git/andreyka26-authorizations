using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Digest.Custom.Server.Application;

namespace Digest.Custom.Server.Infrastructure;

internal class DigestAuthenticationHandler : AuthenticationHandler<DigestAuthenticationOptions>
{
    private readonly HeaderService _headerService;
    private readonly DigestAuthService _digestAuth;

    private readonly DigestAuthenticationOptions _options;

    public DigestAuthenticationHandler(IOptionsMonitor<DigestAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        HeaderService headerService,
        DigestAuthService digestAuth)
        : base(options, logger, encoder, clock)
    {
        _headerService = headerService;
        _digestAuth = digestAuth;
        _options = options.CurrentValue;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(Consts.AuthorizationHeaderName, out var headerValue))
        {
            return AuthenticateResult.NoResult();
        }

        DigestValue? digestValue;
        try
        {
            var digestDict = _headerService.ParseDigestHeaderValue(headerValue);
            digestValue = new DigestValue(digestDict[Consts.RealmNaming],
                digestDict[Consts.UriNaming],
                digestDict[Consts.UserNameNaming],
                digestDict[Consts.NonceNaming],
                digestDict[Consts.NonceCounterNaming],
                digestDict[Consts.ClientNonceNaming],
                digestDict[Consts.ResponseNaming],
                digestDict[Consts.OpaqueNaming]);
        }
        catch
        {
            return AuthenticateResult.NoResult();
        }

        await _digestAuth.EnsureDigestValueValid(digestValue, Request.Method);

        var authenticatedUser = new AuthenticatedUser(Consts.Scheme, true, digestValue.Username);
        var principal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));

        if (_options.UseAuthenticationInfoHeader)
        {
            Response.Headers[Consts.AuthenticationInfoHeaderName] = await _digestAuth.GetAuthInfoHeaderAsync(digestValue);
        }

        return AuthenticateResult.Success(new AuthenticationTicket(principal, new AuthenticationProperties(), Scheme.Name));
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        await base.HandleChallengeAsync(properties);

        if (Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            Response.Headers[Consts.AuthenticationInfoHeaderName] = _digestAuth.GetUnauthorizedDigestHeaderValue();
    }
}
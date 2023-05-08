using Microsoft.AspNetCore.Authentication;

namespace Digest.Custom.Server.Application;

internal class DigestAuthenticationOptions : AuthenticationSchemeOptions
{
    public string ServerNonceSecret { get; set; } = "VerySecret";
    public string Realm { get; set; } = "some-realm";
    public long MaxNonceAgeSeconds { get; set; } = 3600;
    public long DeltaSecondsToNextNonce { get; set; } = 10;
    public bool UseAuthenticationInfoHeader { get; set; } = false;
}

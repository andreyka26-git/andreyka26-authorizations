using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text;
using Digest.Custom.Server.Application;

namespace Digest.Custom.Server.Infrastructure;

internal class DigestAuthService
{
    private readonly IUsernameHashedSecretProvider _usernameHashedSecretProvider;
    private readonly IHashService _hashService;
    private readonly HeaderService _headerService;
    private readonly DigestAuthenticationOptions _options;

    public DigestAuthService(IUsernameHashedSecretProvider usernameHashedSecretProvider,
        IHashService hashService,
        HeaderService headerService,
        IOptions<DigestAuthenticationOptions> options)
    {
        _options = options.Value;
        _usernameHashedSecretProvider = usernameHashedSecretProvider;
        _hashService = hashService;
        _headerService = headerService;
    }

    public bool UseAuthenticationInfoHeader => _options.UseAuthenticationInfoHeader;

    public string GetUnauthorizedDigestHeaderValue()
    {
        var parts = new List<DigestValueSubItem> {
                new DigestValueSubItem(Consts.RealmNaming, _options.Realm, true),
                new DigestValueSubItem(Consts.NonceNaming, CreateNonce(DateTime.UtcNow), true),
                new DigestValueSubItem(Consts.QopNaming, Consts.QopMode, true),
                new DigestValueSubItem(Consts.OpaqueNaming, Consts.Opaque, true),
                new DigestValueSubItem(Consts.AlgorithmNaming, Consts.Algorithm, false),
            };

        return _headerService.BuildDigestHeaderValue(parts);
    }

    public async Task<string> GetAuthInfoHeaderAsync(DigestValue clientDigestValue)
    {
        var timestampStr = clientDigestValue.Nonce[..Consts.NonceTimestampFormat.Length];
        var timestamp = ParseTimestamp(timestampStr);

        var delta = DateTime.UtcNow - timestamp;
        var deltaSeconds = Math.Abs(delta.TotalSeconds);

        var a1Hash = await _usernameHashedSecretProvider.GetA1Md5HashForUsernameAsync(clientDigestValue.Username, _options.Realm);
        var a2Hash = _hashService.ToMd5Hash($":{clientDigestValue.Uri}");

        var resp = _hashService.ToMd5Hash($"{a1Hash}:{clientDigestValue.Nonce}:{clientDigestValue.NonceCounter}:{clientDigestValue.ClientNonce}:{Consts.QopMode}:{a2Hash}");

        var digestValueParts = new List<DigestValueSubItem>();

        if (Math.Abs(deltaSeconds - _options.MaxNonceAgeSeconds) < _options.DeltaSecondsToNextNonce)
        {
            digestValueParts.Add(new DigestValueSubItem("nextnonce", CreateNonce(DateTime.UtcNow), true));
        }

        digestValueParts.Add(new("qop", Consts.QopMode, true));
        digestValueParts.Add(new("rspauth", resp, true));
        digestValueParts.Add(new("cnonce", clientDigestValue.ClientNonce, true));
        digestValueParts.Add(new("nc", clientDigestValue.NonceCounter, false));

        return _headerService.BuildDigestHeaderValue(digestValueParts, string.Empty);
    }

    public async Task EnsureDigestValueValid(DigestValue clientDigestValue, string requestMethod)
    {
        EnsureNonceValid(clientDigestValue);

        if (clientDigestValue.Opaque != Consts.Opaque)
            throw new Exception("Opaque values are not the same");

        var a1Hash = await _usernameHashedSecretProvider.GetA1Md5HashForUsernameAsync(clientDigestValue.Username, _options.Realm);
        var a2Hash = _hashService.ToMd5Hash($"{requestMethod}:{clientDigestValue.Uri}");

        var expectedHash = _hashService
            .ToMd5Hash($"{a1Hash}:{clientDigestValue.Nonce}:{clientDigestValue.NonceCounter}:{clientDigestValue.ClientNonce}:{Consts.QopMode}:{a2Hash}");

        if (expectedHash != clientDigestValue.Response)
            throw new Exception("Hashes are not equal");
    }

    private void EnsureNonceValid(DigestValue challengeResponse)
    {
        var timestampStr = challengeResponse.Nonce.Substring(0, Consts.NonceTimestampFormat.Length);
        var timestamp = ParseTimestamp(timestampStr);

        var delta = DateTime.UtcNow - timestamp;

        if (Math.Abs(delta.TotalSeconds) > _options.MaxNonceAgeSeconds)
            throw new Exception("time exceeded MaxNonceAge");

        var currentNonce = CreateNonce(timestamp.DateTime);

        if (challengeResponse.Nonce != currentNonce)
            throw new Exception("Nonce doesn't match.");
    }

    private string CreateNonce(DateTime timestamp)
    {
        var sb = new StringBuilder();
        var timestampStr = timestamp.ToString(Consts.NonceTimestampFormat, CultureInfo.InvariantCulture);

        sb.Append(timestampStr);
        sb.Append(" ");
        sb.Append(_hashService.ToMd5Hash($"{timestampStr}:{_options.ServerNonceSecret}"));

        return sb.ToString();
    }

    private static DateTimeOffset ParseTimestamp(string timestampStr)
    {
        return DateTimeOffset.ParseExact(timestampStr, Consts.NonceTimestampFormat, CultureInfo.InvariantCulture);
    }
}
using System.Text.RegularExpressions;

namespace Digest.Custom.Server.Application;

internal record DigestValue(string Realm, string Uri, string Username, string Nonce, string NonceCounter, string ClientNonce, string Response, string Opaque);
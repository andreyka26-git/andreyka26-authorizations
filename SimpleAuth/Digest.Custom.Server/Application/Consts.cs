namespace Digest.Custom.Server.Application
{
    public class Consts
    {
        public const string Scheme = "Digest";
        public const string AuthenticationInfoHeaderName = "WWW-Authenticate";
        public const string AuthorizationHeaderName = "Authorization";

        //public const string AuthenticationInfoHeaderName = "Authentication-Info";
        public const string DigestAuthenticationClaimName = "DIGEST_AUTHENTICATION_NAME";

        public const string QopMode = "auth";
        public const string Opaque = "1f36bf2dae9ddb750a644c9994ffffe1";
        public const string Algorithm = "MD5";

        public const string NonceTimestampFormat = "yyyy-MM-dd HH:mm:ss.ffffffZ";

        public const string RealmNaming = "realm";
        public const string NonceNaming = "nonce";
        public const string NonceCounterNaming = "nc";
        public const string QopNaming = "qop";
        public const string OpaqueNaming = "opaque";
        public const string UriNaming = "uri";
        public const string UserNameNaming = "username";
        public const string ClientNonceNaming = "cnonce";
        public const string ResponseNaming = "response";
        public const string AlgorithmNaming = "algorithm";
    }
}

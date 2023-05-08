using System.Net;
using Digest.Custom.Server.Application;
using Digest.Custom.Server.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Digest.WebClient.Pages
{
    public class IndexModel : PageModel
    {
        private const string ServerUrl = "https://localhost:7000/api/resources";
        private readonly HttpClient _httpClient;
        private readonly IHashService _hashService;
        private readonly HeaderService _headerService;

        public IndexModel(IHttpClientFactory factory,
            IHashService hashService,
            HeaderService headerService)
        {
            _httpClient = factory.CreateClient();
            _hashService = hashService;
            _headerService = headerService;
        }

        public string UserName { get; set; }
        public string Password { get; set; }

        public string Realm { get; set; }
        public string Qop { get; set; }
        public string Nonce { get; set; }
        public string Opaque { get; set; }

        public string ResourceServerResponse { get; set; }

        public async Task OnGetAsync()
        {
            using (var req = new HttpRequestMessage(HttpMethod.Get, ServerUrl))
            {
                var resp = await _httpClient.SendAsync(req);
                
                if (resp.StatusCode != HttpStatusCode.Unauthorized)
                {
                    throw new Exception($"First call to api without auth should return 401 Unauthorized, but it is {resp.StatusCode}");
                }

                var digestHeaderValue = resp.Headers.GetValues(Consts.AuthenticationInfoHeaderName).Single();

                var valuesDict = _headerService.ParseDigestHeaderValue(digestHeaderValue);

                //for this simple example we don't support anything except MD5 so we don't parse it
                Realm = valuesDict[Consts.RealmNaming];
                Nonce = valuesDict[Consts.NonceNaming];
                Qop = valuesDict[Consts.QopNaming];
                Opaque = valuesDict[Consts.OpaqueNaming];
            }

            UserName = "andreyka26_";
            Password = "mypass1";
        }

        public async Task OnPostAsync(string userName, string password, string realm, string qop, string nonce, string opaque)
        {
            var a1Hash = _hashService.ToMd5Hash($"{userName}:{realm}:{password}");
            var a2Hash = _hashService.ToMd5Hash($"GET:{ServerUrl}");
            var nc = "00000001";
            var cnonce = "0a4f113b";

            var response = _hashService.ToMd5Hash($"{a1Hash}:{nonce}:{nc}:{cnonce}:{qop}:{a2Hash}");

            var parts = new List<DigestValueSubItem> {
                new DigestValueSubItem(Consts.UserNameNaming, userName, true),
                new DigestValueSubItem(Consts.RealmNaming, realm, true),
                new DigestValueSubItem(Consts.NonceNaming, nonce, true),
                new DigestValueSubItem(Consts.UriNaming, ServerUrl, true),
                new DigestValueSubItem(Consts.QopNaming, qop, true),
                new DigestValueSubItem(Consts.NonceCounterNaming, nc, true),
                new DigestValueSubItem(Consts.ClientNonceNaming, cnonce, true),
                new DigestValueSubItem(Consts.ResponseNaming, response, true),
                new DigestValueSubItem(Consts.OpaqueNaming, opaque, true),
                new DigestValueSubItem(Consts.AlgorithmNaming, Consts.Algorithm, false),
            };

            using (var req = new HttpRequestMessage(HttpMethod.Get, ServerUrl))
            {
                req.Headers.Add(Consts.AuthorizationHeaderName, _headerService.BuildDigestHeaderValue(parts));
                var resp = await _httpClient.SendAsync(req);

                ResourceServerResponse = await resp.Content.ReadAsStringAsync();
            }
        }
    }
}
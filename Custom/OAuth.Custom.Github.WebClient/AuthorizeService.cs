using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace OAuth.Custom.Github.WebClient
{
    public class AuthorizeService
    {
        // we need to validate this state is the same per client's oauth flow
        private const string State = "qwerty";

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUrl;

        private IConfiguration _config;
        private HttpClient _httpClient;

        public AuthorizeService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClient = httpClientFactory.CreateClient();

            _clientId = _config.GetValue<string>("ClientId");
            _clientSecret = _config.GetValue<string>("ClientSecret");
            _redirectUrl = _config.GetValue<string>("RedirectUrl");
        }

        public async Task<string?> GetAuthTokenAsync(CallbackResponse callback, CancellationToken cancellationToken = default)
        {
            var url = "https://github.com/login/oauth/access_token";

            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var requestDto = new TokenRequestDto
                {
                    ClientId = _clientId,
                    ClientSecret = _clientSecret,
                    Code = callback.AuthCode,
                    RedirectUri = _redirectUrl
                };

                var requestJson = JsonConvert.SerializeObject(requestDto);

                request.Content = new StringContent(requestJson, Encoding.UTF8, MediaTypeNames.Application.Json);

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

                using (var response = await _httpClient.SendAsync(request, cancellationToken))
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var tokenDto = JsonConvert.DeserializeObject<TokenResponseDto>(responseJson);

                    return tokenDto.Token;
                }
            }
        }

        public string GenerateAuthorizeLink()
        {
            var link = $"https://github.com/login/oauth/authorize?client_id={_clientId}&redirect_uri={_redirectUrl}&scope=user&state={State}";

            return link;
        }
    }
}

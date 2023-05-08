using Newtonsoft.Json;

namespace OAuth.Custom.Github.WebClient
{
    public class TokenResponseDto
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}

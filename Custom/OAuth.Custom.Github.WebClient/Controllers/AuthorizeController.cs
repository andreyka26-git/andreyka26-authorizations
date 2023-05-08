using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace OAuth.Custom.Github.WebClient.Controllers
{
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly AuthorizeService _authorizeService;
        private readonly HttpClient _httpClient;

        public AuthorizeController(AuthorizeService authorizeService, IHttpClientFactory httpClientFactory)
        {
            _authorizeService = authorizeService;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("/signin-github")]
        public async Task<IActionResult> HandleGetCallbackAsync([FromQuery] string code, [FromQuery] string state)
        {
            var callback = new CallbackResponse
            {
                AuthCode = code,
                State = state
            };

            var token = await _authorizeService.GetAuthTokenAsync(callback);

            using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("andreyka26-git")));

            var response = await _httpClient.SendAsync(request);

            var responseJson = await response.Content.ReadAsStringAsync();
            return Ok(responseJson);
        }

        [HttpPost("/signin-github")]
        public async Task<IActionResult> HandlePostCallbackAsync([FromQuery] string code, [FromQuery] string state)
        {
            var callback = new CallbackResponse
            {
                AuthCode = code,
                State = state
            };

            var token = await _authorizeService.GetAuthTokenAsync(callback);

            using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("andreyka26-git")));

            var response = await _httpClient.SendAsync(request);

            var responseJson = await response.Content.ReadAsStringAsync();
            return Ok(responseJson);
        }
    }
}

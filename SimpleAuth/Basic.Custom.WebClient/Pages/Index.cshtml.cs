using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;

namespace Basic.Custom.WebClient.Pages
{
    public class IndexModel : PageModel
    {
        private const string ServerUrl = "https://localhost:7000";
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient();
        }
        
        public string UserName { get; set; }
        public string Password { get; set; }

        public string ResourceServerResponse { get; set; }

        public void OnGet()
        {
            UserName = "andreyka26_";
            Password = "mypass1";
        }

        public async Task OnPostAsync(string userName, string password)
        {
            var url = $"{ServerUrl}/api/resources";
            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));

            using (var req = new HttpRequestMessage(HttpMethod.Get, url))
            {
                req.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);

                var resp = await _httpClient.SendAsync(req);
                ResourceServerResponse = await resp.Content.ReadAsStringAsync();
            }
        }
    }
}
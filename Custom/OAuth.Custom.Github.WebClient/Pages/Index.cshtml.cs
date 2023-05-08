using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAuth.Custom.Github.WebClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AuthorizeService _authorizeService;

        public string AuthorizeLink { get; private set; }

        public IndexModel(ILogger<IndexModel> logger, AuthorizeService authorizeService)
        {
            _logger = logger;
            _authorizeService = authorizeService;
        }

        public void OnGet()
        {
            var link = _authorizeService.GenerateAuthorizeLink();
            AuthorizeLink = link;
        }
    }
}
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookie.Google.Server.Pages.Secret
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public async void OnGet()
        {
            var principal = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}

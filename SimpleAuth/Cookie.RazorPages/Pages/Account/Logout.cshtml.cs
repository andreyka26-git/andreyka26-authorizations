using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookie.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            await LogoutAsync();
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPost()
        {
            await LogoutAsync();
            return RedirectToPage("/Index");
        }

        private async Task LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);   
        }
    }
}

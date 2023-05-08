using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookie.Google.Server.Pages.Authentication
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPost()
        {
            // using Microsoft.AspNetCore.Authentication;
            await HttpContext.SignOutAsync();
            return RedirectToPage();
        }
    }
}

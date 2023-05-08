using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Oidc.OpenIddict.AuthorizationServer.Pages;

[Authorize]
public class Consent : PageModel
{
    [BindProperty]
    public string? ReturnUrl { get; set; }

    public IActionResult OnGet(string returnUrl)
    {
        ReturnUrl = returnUrl;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string grant)
    {
        User.SetClaim(Consts.ConsentNaming, grant);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, User);
        return Redirect(ReturnUrl);
    }
}
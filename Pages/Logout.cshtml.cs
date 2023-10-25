using Gflower.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gflower.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SessionHelper _sessionHelper;

        public LogoutModel(SessionHelper sessionHelper)
        {
            _sessionHelper = sessionHelper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await HttpContext.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);
            // delete session
            HttpContext.Session.Clear();
            return RedirectToPage("Home");
        }
    }
}

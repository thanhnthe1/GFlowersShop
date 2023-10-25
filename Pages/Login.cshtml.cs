using Gflower.Common;
using Gflower.Data.Repository;
using Gflower.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Gflower.Pages
{
    public class Login : PageModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly SessionHelper _sessionHelper;

        public Login(IAccountRepository repository, SessionHelper sessionHelper)
        {
            accountRepository = repository;
            _sessionHelper = sessionHelper;
        }
        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; } = false;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            //check login 
            var username = _sessionHelper.GetSessionData<string>("username");
            //check login in session
            if (username != null)
            {
                return RedirectToPage("/Home");
            }
            //check login in cookie
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Home");
            }

            var result = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            if (result.Succeeded)
            {
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    result.Principal,
                    result.Properties);

                return RedirectToPage("/Home");
            }

            return Page();
        }




        [BindProperty]
        public InputModel Input { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (LoginUser())
            {
                var returnUrl = _sessionHelper.GetSessionData<string>("ReturnUrl");
                HttpContext.Session.Clear();
                // Lưu thông tin người dùng vào session
                _sessionHelper.SaveSessionData("username", Input.Username);

                if (Input.RememberMe)
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, Input.Username)
                        };
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true, // để cookie tồn tại sau khi đóng trình duyệt
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) // thiết lập thời gian hết hạn của cookie
                    };
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                }

                if (returnUrl != null)
                {
                    return RedirectToPage(returnUrl);
                }
                else
                {
                    return RedirectToPage("Home");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Incorrect account.");
                return Page();
            }
        }



        public bool LoginUser()
        {
            var accRaw = accountRepository.Login(Input.Username, Input.Password);
            if (accRaw != null)
            {
                return true;
            }
            return false;
        }
    }
}

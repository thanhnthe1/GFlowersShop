using Gflower.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Gflower.Data.Repository;
using Gflower.Models;

namespace Gflower.Pages
{
	public class SignupModel : PageModel
	{

		private readonly IAccountRepository accountRepository;
		private readonly SessionHelper _sessionHelper;

		public SignupModel(IAccountRepository repository, SessionHelper sessionHelper)
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

			[Required]
			public string LastName { get; set; }
			[Required]
			public string FirstName { get; set; }
		}

		[BindProperty]
		public InputModel Input { get; set; }
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

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
			// check username exists
			var existingUser =  accountRepository.GetAccByUsername(Input.Username);
			if (existingUser != null)
			{
				ModelState.AddModelError(string.Empty, "Username exists.");
				return Page();
			}
			// create user new
			var newUser = new Account
			{
				Username = Input.Username,
				Password = Input.Password,
				LastName = Input.LastName,
				FirstName = Input.FirstName,
				Role = 2
			};
			// save new user database
			await accountRepository.InsertAccount(newUser);

			// save new user to session
			_sessionHelper.SaveSessionData("username", newUser.Username);

			return RedirectToPage("/Home");
		}
	}
}

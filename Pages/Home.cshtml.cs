using Gflower.Common;
using Gflower.Data.Repository;
using Gflower.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gflower.Pages
{
    public class Home : PageModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly SessionHelper _sessionHelper;
        private readonly ICartRepository cartRepository;


        public Account account { get; set; }
        public List<Cart> carts { get; set; }

        public Home(IAccountRepository repository, SessionHelper sessionHelper, ICartRepository _cartRepository)
        {
            accountRepository = repository;
            _sessionHelper = sessionHelper;
            cartRepository = _cartRepository;
        }

        public async Task OnGet()
        {
            var username = _sessionHelper.GetSessionData<string>("username");
            //check login in session
            if(username !=null)
            {
                account = GetAccountByUsername(username);
            }
            else //check login in cookie
            {
                if (User.Identity.IsAuthenticated)
                {
                    username = User.Identity.Name;
                    if (username != null)
                    {
                        account = GetAccountByUsername(username);
                        _sessionHelper.SaveSessionData("username", username);
                    }
                }
            }
            //get cart in session
            var sessionCartItems = _sessionHelper.GetSessionData<List<Cart>>("cart");

            if (sessionCartItems != null)
            {
                carts = sessionCartItems;
            }
            else
            {
                if (username != null)
                    carts = await cartRepository.GetListCart(account.AccountId);
            }

        }

        public Account GetAccountByUsername(string username)
        {
            return accountRepository.GetAccByUsername(username);
        }
    }
}

using Gflower.Common;
using Gflower.Data.Repository;
using Gflower.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gflower.Pages
{
    public class CartModel : PageModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly ICartRepository cartRepository;
        private readonly IProductRepository productRepository;
        private readonly SessionHelper _sessionHelper;

        public Account account { get; set; }

        public List<Cart> carts { get; set; }
        public List<Product> products { get; set; }

        public CartModel(IAccountRepository accRepository, SessionHelper sessionHelper,
            ICartRepository _cartRepository, IProductRepository _productRepository)
        {
            accountRepository = accRepository;
            cartRepository = _cartRepository;
            productRepository = _productRepository;
            _sessionHelper = sessionHelper;

        }
        public async Task<IActionResult> OnGetAsync()
        {
            SetLogin();

            await SetCart();
            return Page();
        }

        public void SetLogin()
        {
            var username = _sessionHelper.GetSessionData<string>("username");
            //check login in session
            if (username != null)
            {
                account = accountRepository.GetAccByUsername(username);
            }
            else //check login in cookie
            {
                if (User.Identity.IsAuthenticated)
                {
                    username = User.Identity.Name;
                    if (username != null)
                    {
                        account = accountRepository.GetAccByUsername(username);
                        _sessionHelper.SaveSessionData("username", username);
                    }
                }
            }
        }

        public async Task SetCart()
        {
            var sessionCartItems = _sessionHelper.GetSessionData<List<Cart>>("cart");

            if (sessionCartItems != null)
            {
                carts = sessionCartItems;
            }
            else
            {
                if (account != null)
                    carts = await cartRepository.GetListCart(account.AccountId);
            }
        }

        public async Task<IActionResult> OnGetPlusQuantity(int productId)
        {
            SetLogin();
            await SetCart();
            if(account != null) //da dang nhap
            {
                if (carts != null)
                {
                    var cartItem = carts.FirstOrDefault(ci => ci.ProductId == productId);
                    if(cartItem != null)
                    {
                        await cartRepository.UpdateQuantity(cartItem, true);
                    }
                    else
                    {
                        return new JsonResult(new { success = false });
                    }
                    return new JsonResult(new { success = true });

                }
                else
                {
                    return new JsonResult(new { success = false });
                }

            }
            else //chua dang nhap
            {
                if (carts != null) // neu cart co san pham
                {
                    var cartItem = carts.FirstOrDefault(ci => ci.ProductId == productId);
                    if (cartItem != null)
                    {
                        cartItem.Quantity += 1;
                        cartItem.TotalPrice += cartItem.Product.ProductPrice;
                        //set lai list cart session
                        _sessionHelper.SaveSessionData("cart", carts);

                    }
                    else
                    {
                        return new JsonResult(new { success = false });
                    }
                    return new JsonResult(new { success = true });

                } // cart chua co san pham, in ra loi
                else
                {
                    return new JsonResult(new { success = false });
                }
            }
        }

        public async Task<IActionResult> OnGetRemoveCart(int productId)
        {
            SetLogin();
            await SetCart();
            if (account != null) //da dang nhap
            {
                if (carts != null)
                {
                    var cartItem = carts.FirstOrDefault(ci => ci.ProductId == productId);
                    if (cartItem != null)
                    {
                        await cartRepository.DeleteCart(cartItem.CartId);
                    }
                    else
                    {
                        return new JsonResult(new { success = false });
                    }
                    return new JsonResult(new { success = true });

                }
                else
                {
                    return new JsonResult(new { success = false });
                }

            }
            else //chua dang nhap
            {
                if (carts != null) // neu cart co san pham
                {
                    var cartItem = carts.FirstOrDefault(ci => ci.ProductId == productId);
                    if (cartItem != null)
                    {
                        carts.Remove(cartItem);
                        //set lai list cart session
                        _sessionHelper.SaveSessionData("cart", carts);

                    }
                    else
                    {
                        return new JsonResult(new { success = false });
                    }
                    return new JsonResult(new { success = true });

                } // cart chua co san pham, in ra loi
                else
                {
                    return new JsonResult(new { success = false });
                }
            }
        }

        public async Task<IActionResult> OnGetMinusQuantity(int productId)
        {
            SetLogin();
            await SetCart();
            if (account != null) //da dang nhap
            {
                if (carts != null)
                {
                    var cartItem = carts.FirstOrDefault(ci => ci.ProductId == productId);
                    if (cartItem != null)
                    {
                        await cartRepository.UpdateQuantity(cartItem, false);
                    }
                    else
                    {
                        return new JsonResult(new { success = false });
                    }
                    return new JsonResult(new { success = true });

                }
                else
                {
                    return new JsonResult(new { success = false });
                }

            }
            else //chua dang nhap
            {
                if (carts != null) // neu cart co san pham
                {
                    var cartItem = carts.FirstOrDefault(ci => ci.ProductId == productId);
                    if (cartItem != null)
                    {
                        cartItem.Quantity -= 1;
                        cartItem.TotalPrice -= cartItem.Product.ProductPrice;
                        //set lai list cart session
                        _sessionHelper.SaveSessionData("cart", carts);

                    }
                    else
                    {
                        return new JsonResult(new { success = false });
                    }
                    return new JsonResult(new { success = true });

                } // cart chua co san pham, in ra loi
                else
                {
                    return new JsonResult(new { success = false });
                }
            }
        }

        public async Task<IActionResult> OnGetCheckout()
        {
            SetLogin();
            await SetCart();
            
            return new JsonResult(new { carts });
              
        }
    }
}

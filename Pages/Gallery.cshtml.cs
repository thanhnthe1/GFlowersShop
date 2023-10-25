using Gflower.Common;
using Gflower.Data.Repository;
using Gflower.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Gflower.Pages
{
    public class GalleryModel : PageModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly ICartRepository cartRepository;
        private readonly IProductRepository productRepository;
        private readonly SessionHelper _sessionHelper;

        public Account account { get; set; }

        public List<Cart> carts { get; set; }
        public List<Product> products { get; set; }

        public GalleryModel(IAccountRepository accRepository, SessionHelper sessionHelper,
            ICartRepository _cartRepository, IProductRepository _productRepository)
        {
            accountRepository = accRepository;
            cartRepository = _cartRepository;
            productRepository = _productRepository;
            _sessionHelper = sessionHelper;
        }
        public async Task<IActionResult> OnGetAsync()
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

            //get cart in session
            var sessionCartItems = _sessionHelper.GetSessionData < List<Cart>> ("cart");

            if (sessionCartItems != null)
            {
                carts = sessionCartItems;
            }
            else
            {
                if(username!=null)
                    carts = await cartRepository.GetListCart(account.AccountId);
            }
            // set list product
            products = await productRepository.GetProducts();
            return Page();
        }

        public async Task<IActionResult> OnGetAddCart(int productId)
        {
            bool productExist = true; //neu product co trong cart thi la true, ko thi la false
            var username = _sessionHelper.GetSessionData<string>("username");
            var product = await productRepository.GetProduct(productId);
            // neu da login thi add cart vao db
            if (product != null) {
                decimal totalPrice = product.ProductPrice - (product.ProductPrice * ((decimal)product.Discount / 100));
                product.ProductPrice= totalPrice;
                Cart newcart = new Cart
                {
                    ProductId = productId,
                    Quantity = 1,
                    TotalPrice = totalPrice
                };
                if (username != null)
                {
                    var account = accountRepository.GetAccByUsername(username);
                    newcart.AccountId = account.AccountId;
                    productExist = await cartRepository.AddProductToCart(newcart);
                }
                else // neu chua login thi add vao session
                {
                    //lay list cart tu session
                    var sessionCartItems = _sessionHelper.GetSessionData<List<Cart>>("cart");
                    if (sessionCartItems != null)
                    {
                        newcart.Product = product;
                        productExist= AddOrUpdateProductToCart(sessionCartItems, newcart);
                    }
                    else
                    {
                        newcart.Product = product;
                        sessionCartItems = new List<Cart> { newcart };
                        productExist = false;
                    }
                    //set lai list cart session
                    _sessionHelper.SaveSessionData("cart", sessionCartItems);
                }
            }
            else
            {
                //neu product khong ton tai
                return new JsonResult(new { success = false });
            }
            return new JsonResult(new { success = true,status = productExist });
        }

        public bool AddOrUpdateProductToCart(List<Cart> cart, Cart productToAdd)
        {
            Cart existingProduct = cart.FirstOrDefault(p => p.ProductId == productToAdd.ProductId);

            if (existingProduct != null)
            {
                existingProduct.Quantity += productToAdd.Quantity;
                existingProduct.TotalPrice+= productToAdd.TotalPrice;
                return true;
            }
            else
            {
                cart.Add(productToAdd);
                return false;
            }
        }

    }
}

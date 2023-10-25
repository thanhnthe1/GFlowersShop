using Gflower.Common;
using Gflower.Data.Repository;
using Gflower.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Gflower.Pages
{
    public class ProductModel : PageModel
    {
        private readonly IAccountRepository accountRepository;
        private readonly ICartRepository cartRepository;
        private readonly IOrderRepositoy orderRepository;
        private readonly IProductRepository productRepository;
        private readonly SessionHelper _sessionHelper;
        private IHostingEnvironment _environment;

        public Account account { get; set; }
        public List<Product> products { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Please choose at least one file.")]
            [DataType(DataType.Upload)]
            [FileExtensions(Extensions = "png,jpg,jpeg")]
            [Display(Name = "Choose file(s) to upload")]
            [BindProperty]
            public IFormFile[] FileUploads { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string Name { get; set; }

            [Required]
            public decimal Price { get; set; }
            [Required]
            public string Description { get; set; }
            public int Discount { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public ProductModel(IAccountRepository accRepository, SessionHelper sessionHelper,
            ICartRepository _cartRepository, IOrderRepositoy _orderRepository, IProductRepository _productRepository
            , IHostingEnvironment environment)
        {
            accountRepository = accRepository;
            cartRepository = _cartRepository;
            _sessionHelper = sessionHelper;
            orderRepository = _orderRepository;
            productRepository = _productRepository;
            _environment = environment;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            SetLogin();
            if (account == null)
            {
                _sessionHelper.SaveSessionData("ReturnUrl", "/Product");
                return RedirectToPage("/Login");
            }
            else
            {
                if (account.Role != 1)
                {
                    return RedirectToPage("/Error403");
                }
            }
            await SetProducts();
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

        public async Task SetProducts()
        {
            if(account != null)
            {
                products = await productRepository.GetProductsAdmin();
            }
        }

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    SetLogin();
        //    if (account != null)
        //    {
        //        if (Input != null)
        //        {
        //            Product product = new Product
        //            {
        //                ProductName = Input.Name,
        //                ProductDescription = Input.Description,
        //                ProductPrice = Input.Price,
        //                Status = 1

        //            };
        //            //upload image
        //            if (Input.FileUploads != null)
        //            {
        //                foreach (var FileUpload in Input.FileUploads)
        //                {
        //                    var fileName = Guid.NewGuid().ToString() + "_" + FileUpload.FileName;
        //                    var filePath = Path.Combine(_environment.WebRootPath, "Image", "bouquet", fileName);
        //                    product.ProductImage = fileName;
        //                    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //                    {
        //                        await FileUpload.CopyToAsync(fileStream);
        //                    }
        //                }

        //            }
        //            await productRepository.AddProduct(product);
        //            TempData["SuccessMessage"] = "Add success!";
        //            return RedirectToPage("/Product");
        //        }
        //        else
        //        {
        //            return RedirectToPage("/Product");
        //        }
        //    }
        //    else
        //    {
        //        _sessionHelper.SaveSessionData("ReturnUrl", "/Product");
        //        return RedirectToPage("/Login");
        //    }

        //}
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    SetLogin();
        //    if (account != null)
        //    {
        //        try
        //        {
        //            if (Input != null)
        //            {
        //                Product product = new Product
        //                {
        //                    ProductName = Input.Name,
        //                    ProductDescription = Input.Description,
        //                    ProductPrice = Input.Price,
        //                    Status = 1
        //                };

        //                var flowerDB = new GFlowersContext();

        //                // Check if the product name already exists
        //                if (await flowerDB.Products.AnyAsync(p => p.ProductName == product.ProductName))
        //                {
        //                    TempData["ErrorMessage"] = "Add lỗi: Product name already exists.";
        //                    await SetProducts();
        //                    return Page();
        //                }

        //                // Check if the price is non-negative
        //                if (product.ProductPrice < 0)
        //                {
        //                    TempData["ErrorMessage"] = "Add lỗi: Product price must be non-negative.";
        //                    await SetProducts();
        //                    return Page();
        //                }

        //                //upload image
        //                if (Input.FileUploads != null)
        //                {
        //                    foreach (var FileUpload in Input.FileUploads)
        //                    {
        //                        var fileName = Guid.NewGuid().ToString() + "_" + FileUpload.FileName;
        //                        var filePath = Path.Combine(_environment.WebRootPath, "Image", "bouquet", fileName);
        //                        product.ProductImage = fileName;
        //                        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //                        {
        //                            await FileUpload.CopyToAsync(fileStream);
        //                        }
        //                    }
        //                }

        //                await flowerDB.Products.AddAsync(product);
        //                await flowerDB.SaveChangesAsync();

        //                TempData["SuccessMessage"] = "Add success!";
        //                return RedirectToPage("/Product");
        //            }
        //            else
        //            {
        //                return RedirectToPage("/Product");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["ErrorMessage"] = "Add lỗi: " + ex.Message;
        //            await SetProducts();
        //            return Page();
        //        }
        //    }
        //    else
        //    {
        //        _sessionHelper.SaveSessionData("ReturnUrl", "/Product");
        //        return RedirectToPage("/Login");
        //    }
        //}

        public async Task<IActionResult> OnPostAsync()
        {
            SetLogin();
            if (account != null)
            {
                try
                {
                    if (Input != null)
                    {
                        Product product = new Product
                        {
                            ProductName = Input.Name,
                            ProductDescription = Input.Description,
                            ProductPrice = Input.Price,
                            Status = 1
                        };

                        var flowerDB = new GFlowersContext();

                        // Check if the product name already exists
                        if (await flowerDB.Products.AnyAsync(p => p.ProductName == product.ProductName))
                        {
                            TempData["ErrorMessage"] = " Product name already exists.";
                            await SetProducts();
                            return Page();
                        }

                        // Check if the price is non-negative
                        if (product.ProductPrice < 0)
                        {
                            TempData["ErrorMessage"] = " Product price must be non-negative.";
                            await SetProducts();
                            return Page();
                        }

                        //upload image
                        if (Input.FileUploads != null)
                        {
                            foreach (var FileUpload in Input.FileUploads)
                            {
                                var fileName = Guid.NewGuid().ToString() + "_" + FileUpload.FileName;
                                var filePath = Path.Combine(_environment.WebRootPath, "Image", "bouquet", fileName);
                                product.ProductImage = fileName;
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await FileUpload.CopyToAsync(fileStream);
                                }
                            }
                        }

                        await flowerDB.Products.AddAsync(product);
                        await flowerDB.SaveChangesAsync();

                        TempData["SuccessMessage"] = "Add success!";
                        return RedirectToPage("/Product");
                    }
                    else
                    {
                        return RedirectToPage("/Product");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Add lỗi: " + ex.Message;
                    await SetProducts();
                    return Page();
                }
            }
            else
            {
                _sessionHelper.SaveSessionData("ReturnUrl", "/Product");
                return RedirectToPage("/Login");
            }
        }

    }
}

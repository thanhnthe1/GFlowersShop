//using Gflower.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace Gflower.Data.DataAccess
//{
//    public class ProductManagement
//    {
//        private static ProductManagement instance = null;
//        private static readonly object instanceLock = new object();
//        private ProductManagement() { }
//        public static ProductManagement Instance
//        {
//            get
//            {
//                lock (instanceLock)
//                {
//                    if (instance == null)
//                    {
//                        instance = new ProductManagement();
//                    }
//                    return instance;
//                }
//            }
//        }

//        public async Task<List<Product>> GetListProducts()
//        {
//            List<Product> products = new List<Product>();
//            try
//            {
//                var flowerDB = new GFlowersContext();
//                products = await flowerDB.Products.Where(c => c.Status == 1).ToListAsync();
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//            return products;
//        }

//        public async Task<List<Product>> GetListProductsAdmin()
//        {
//            List<Product> products = new List<Product>();
//            try
//            {
//                var flowerDB = new GFlowersContext();
//                products = await flowerDB.Products.ToListAsync();
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//            return products;
//        }

//        public async Task<Product> GetProductByID(int proId)
//        {
//            Product pro = null;
//            try
//            {
//                var flowerDB = new GFlowersContext();
//                pro = await flowerDB.Products.FirstOrDefaultAsync(p => p.ProductId == proId);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//            return pro;
//        }



//        //public async Task AddNew(Product product)
//        //{
//        //    try
//        //    {
//        //        if (product != null)
//        //        {
//        //            var flowerDB = new GFlowersContext();
//        //            await flowerDB.Products.AddAsync(product);
//        //            await flowerDB.SaveChangesAsync();
//        //        }
//        //        else
//        //        {
//        //            throw new Exception("The product is null.");
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        throw new Exception(ex.Message);
//        //    }
//        //}


//        string message = null;
//        public async Task<bool> AddNew(Product product)
//        {


//        try
//        {
//                if (product != null)
//                {
//                    var flowerDB = new GFlowersContext();

//                    // Check if the product name already exists
//                    if (await flowerDB.Products.AnyAsync(p => p.ProductName == product.ProductName))
//                    {
//                        message = "The product name already exists.";
//                        return false;

//                    }

//                    // Check if the price is non-negative
//                    if (product.ProductPrice < 0)
//                    {
//                        message = "The product price cannot be negative.";
//                        return false;
//                    }

//                    await flowerDB.Products.AddAsync(product);
//                    await flowerDB.SaveChangesAsync();
//                }
//                else
//                {
//                    throw new Exception("The product is null.");
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//        public async Task<IActionResult> OnPostAddNew(Product product)
//        {
//            if (ModelState.IsValid)
//            {
//                string message;
//                bool isProductAdded = await ProductManagement.Instance.AddNew(product, out message);

//                if (isProductAdded)
//                {
//                    // Product added successfully
//                    TempData["SuccessMessage"] = "Product added successfully.";
//                }
//                else
//                {
//                    // Product not added, show error message
//                    TempData["ErrorMessage"] = message;
//                }

//                return RedirectToPage("/Product"); // Redirect back to the Product page
//            }

//            // Model state is not valid, show the same page with validation errors
//            return Page();
//        }
//        public async Task Update(Product _product)
//        {
//            try
//            {
//                var flowerDB = new GFlowersContext();
//                var product = await flowerDB.Products.FindAsync(_product.ProductId);
//                if (product != null)
//                {
//                    product.ProductName = _product.ProductName;
//                    product.ProductDescription = _product.ProductDescription;
//                    product.ProductPrice = _product.ProductPrice;
//               //  product.Status = _product.Status;
//                    if (_product.ProductImage != null)
//                        product.ProductImage = _product.ProductImage;
//                    else
//                        flowerDB.Entry(product).Property(p => p.ProductImage).IsModified = false;
//                    await flowerDB.SaveChangesAsync();
//                }
//                else
//                {
//                    throw new Exception("The product not exists.");
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//        public async Task Delete(int productId)
//        {
//            try
//            {
//                var flowerDB = new GFlowersContext();
//                var productToDelete = await flowerDB.Products.FindAsync(productId);
//                if (productToDelete != null)
//                {
//                    flowerDB.Products.Remove(productToDelete);
//                    await flowerDB.SaveChangesAsync();
//                }
//                else
//                {
//                    throw new Exception("The product not exists.");
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//    }
//}

using Gflower.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gflower.Data.DataAccess
{
    public class ProductManagement
    {
        private static ProductManagement instance = null;
        private static readonly object instanceLock = new object();
        private ProductManagement() { }
        public static ProductManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductManagement();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Product>> GetListProducts()
        {
            List<Product> products = new List<Product>();
            try
            {
                var flowerDB = new GFlowersContext();
                products = await flowerDB.Products.Where(c => c.Status == 1).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return products;
        }

        public async Task<List<Product>> GetListProductsAdmin()
        {
            List<Product> products = new List<Product>();
            try
            {
                var flowerDB = new GFlowersContext();
                products = await flowerDB.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return products;
        }

        public async Task<Product> GetProductByID(int proId)
        {
            Product pro = null;
            try
            {
                var flowerDB = new GFlowersContext();
                pro = await flowerDB.Products.FirstOrDefaultAsync(p => p.ProductId == proId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return pro;
        }

        public async Task<bool> AddNew(Product product)
        {
          
            try
            {
                if (product != null)
                {
                    var flowerDB = new GFlowersContext();

                    // Check if the product name already exists
                    if (await flowerDB.Products.AnyAsync(p => p.ProductName == product.ProductName))
                    {
                        return false;
                    }

                    // Check if the price is non-negative
                    if (product.ProductPrice < 0)
                    {
                        throw new Exception("Add lỗi: Product name already exists.");
                    }

                    await flowerDB.Products.AddAsync(product);
                    await flowerDB.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception("The product is null.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Rest of the code remains the same

        public async Task Update(Product _product)
        {
            try
            {
                var flowerDB = new GFlowersContext();
                var product = await flowerDB.Products.FindAsync(_product.ProductId);
                if (product != null)
                {
                    product.ProductName = _product.ProductName;
                    product.ProductDescription = _product.ProductDescription;
                    product.ProductPrice = _product.ProductPrice;
                    //  product.Status = _product.Status;
                    if (_product.ProductImage != null)
                        product.ProductImage = _product.ProductImage;
                    else
                        flowerDB.Entry(product).Property(p => p.ProductImage).IsModified = false;
                    await flowerDB.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("The product not exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Delete(int productId)
        {
            try
            {
                var flowerDB = new GFlowersContext();
                var productToDelete = await flowerDB.Products.FindAsync(productId);
                if (productToDelete != null)
                {
                    flowerDB.Products.Remove(productToDelete);
                    await flowerDB.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("The product not exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}

using Gflower.Models;
using Microsoft.EntityFrameworkCore;

namespace Gflower.Data.DataAccess
{
    public class CartManagement
    {
        private static CartManagement instance = null;
        private static readonly object instanceLock = new object();
        private CartManagement() { }
        public static CartManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CartManagement();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Cart>> GetCartsByAccID(int accID)
        {
            List<Cart> carts = new List<Cart>();
            try
            {
                var flowerDB = new GFlowersContext();
                carts = await flowerDB.Carts.Include(c => c.Product).Where(c => c.AccountId == accID && c.Status == 1).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return carts;
        }

        public async Task<Cart> GetCartByProID(int proId, int accId)
        {
            Cart ca = null;
            try
            {
                var flowerDB = new GFlowersContext();
                ca = await flowerDB.Carts.FirstOrDefaultAsync(c => c.ProductId == proId && c.Status == 1 && 
                                                            c.AccountId == accId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ca;
        }

        public async Task UpdateQuantity(Cart cart, bool IsPlus)
        {
            try
            {
                var flowerDB = new GFlowersContext();
                if (IsPlus)
                {
                    var _cart = await GetCartByProID(cart.ProductId, cart.AccountId);
                    _cart.Quantity += 1;
                    _cart.TotalPrice += cart.Product.ProductPrice;
                    flowerDB.Carts.Update(_cart);
                }
                else
                {
                    var _cart = await GetCartByProID(cart.ProductId, cart.AccountId);
                    _cart.Quantity -= 1;
                    _cart.TotalPrice -= cart.Product.ProductPrice;
                    flowerDB.Carts.Update(_cart);
                }
                await flowerDB.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Delete(int cartId)
        {
            try
            {
                var flowerDB = new GFlowersContext();
                var cart = await flowerDB.Carts.FindAsync(cartId);
                if (cart != null)
                {
                    flowerDB.Carts.Remove(cart);
                    await flowerDB.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Cart not exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAllCart(List<Cart> carts)
        {
            try
            {
                var flowerDB = new GFlowersContext();
                if (carts != null)
                {
                    flowerDB.Carts.RemoveRange(carts);
                    await flowerDB.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Carts is null.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> Update(Cart cart)
        {
            bool productExist = false;
            try
            {
                var flowerDB = new GFlowersContext();
                var _cart = await GetCartByProID(cart.ProductId, cart.AccountId);
                //neu san pham co trong cart roi thi chi update + quantity va totalprice
                if (_cart != null)
                {
                    _cart.Quantity += cart.Quantity;
                    _cart.TotalPrice += cart.TotalPrice;
                    flowerDB.Carts.Update(_cart);
                    productExist = true;
                }
                else // chua thi add moi
                {
                    //lay product 
                    var product = await flowerDB.Products.FindAsync(cart.ProductId);
                    if (product != null)
                    {
                        cart.Status = 1;
                        await flowerDB.Carts.AddAsync(cart);
                    }
                    else
                    {
                        throw new Exception("Product not exists.");
                    }
                }
                await flowerDB.SaveChangesAsync();
                return productExist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using Gflower.Models;

namespace Gflower.Data.Repository
{
    public interface ICartRepository
    {
        Task<List<Cart>> GetListCart(int accID);
        Task<Cart> GetCartByProduct(int productId, int accountId);
        Task DeleteCart(int carId);
        Task<bool> AddProductToCart(Cart cart);
        Task UpdateQuantity(Cart cart, bool IsPlus);

        Task DeleteAll(List<Cart> carts);


    }
}

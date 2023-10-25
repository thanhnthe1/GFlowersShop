using Gflower.Models;

namespace Gflower.Data.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();
        Task<List<Product>> GetProductsAdmin();
        Task<Product> GetProduct(int productId);

        Task AddProduct(Product product);
        Task UpdateProduct(Product product);

        Task Delete(int productId);

    }
}

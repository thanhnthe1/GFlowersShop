using Gflower.Data.DataAccess;
using Gflower.Models;

namespace Gflower.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        public Task AddProduct(Product product) =>ProductManagement.Instance.AddNew(product);

        public Task<Product> GetProduct(int productId) =>ProductManagement.Instance.GetProductByID(productId);

        public Task<List<Product>> GetProducts() => ProductManagement.Instance.GetListProducts();

        public Task<List<Product>> GetProductsAdmin()=>ProductManagement.Instance.GetListProductsAdmin();

        public Task UpdateProduct(Product product) =>ProductManagement.Instance.Update(product);

        public Task Delete(int productId) => ProductManagement.Instance.Delete(productId);
    }
}

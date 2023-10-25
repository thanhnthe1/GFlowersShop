using Gflower.Models;

namespace Gflower.Data.Repository
{
    public interface IOrderRepositoy
    {
        Task AddOrder(Order order);
        Order GetOrderAdd();

        Task<List<Order>> GetOrders(int accId);
        Task<Order> GetOrderById(int orderId);
        Task<List<Order>> GetOrdersUsers();
        Task<List<Order>> GetOrdersUnknown();

        Task UpdateStatus(int orderId,int status);
    }
}

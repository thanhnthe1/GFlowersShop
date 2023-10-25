using Gflower.Data.DataAccess;
using Gflower.Models;

namespace Gflower.Data.Repository
{
    public class OrderRepository : IOrderRepositoy
    {
        public Task AddOrder(Order order)=>OrderManagement.Instance.AddNew(order);

        public Order GetOrderAdd() => OrderManagement.Instance.GetOrderAdd();

        public Task<Order> GetOrderById(int orderId) =>OrderManagement.Instance.GetOrderByID(orderId);

        public Task<List<Order>> GetOrders(int accId) => OrderManagement.Instance.GetOrdersByAccId(accId);

        public Task<List<Order>> GetOrdersUnknown() =>OrderManagement.Instance.GetOrdersUnknown();

        public Task<List<Order>> GetOrdersUsers() => OrderManagement.Instance.GetOrdersUsers();

        public Task UpdateStatus(int orderId, int status)=>OrderManagement.Instance.UpdateStatus(orderId, status);
    }
}

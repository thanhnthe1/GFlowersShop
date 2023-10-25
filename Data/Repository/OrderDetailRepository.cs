using Gflower.Data.DataAccess;
using Gflower.Models;

namespace Gflower.Data.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public Task AddListOrder(List<OrderDetail> orders) => OrderDetailManagement.Instance.AddListOrder(orders);
    }
}

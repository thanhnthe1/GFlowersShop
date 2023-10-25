using Gflower.Models;

namespace Gflower.Data.Repository
{
    public interface IOrderDetailRepository
    {
        Task AddListOrder(List<OrderDetail> orders);
    }
}

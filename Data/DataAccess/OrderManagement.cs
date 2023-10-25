using Gflower.Models;
using Microsoft.EntityFrameworkCore;

namespace Gflower.Data.DataAccess
{
    public class OrderManagement
    {
        private static OrderManagement instance = null;
        private static readonly object instanceLock = new object();
        private OrderManagement() { }
        public static OrderManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderManagement();
                    }
                    return instance;
                }
            }
        }

        public async Task<Order> GetOrderByID(int orderID)
        {
            var order = new Order();
            try
            {
                var flowerDB = new GFlowersContext();
                order = await flowerDB.Orders.Include( o=>o.Account).Include(o=>o.OrderDetails).ThenInclude(od=>od.Product).FirstOrDefaultAsync(o => o.OrderId == orderID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return order;
        }

        public async Task<List<Order>> GetOrdersByAccId(int accID)
        {
            List<Order> orders = new List<Order>();
            try
            {
                var flowerDB = new GFlowersContext();
                orders = await flowerDB.Orders.Where(o => o.AccountId == accID).OrderByDescending(o=>o.OrderDate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public async Task<List<Order>> GetOrdersUsers()
        {
            List<Order> orders = new List<Order>();
            try
            {
                var flowerDB = new GFlowersContext();
                orders = await flowerDB.Orders.Include(o => o.Account).Where(o => o.AccountId != null).OrderByDescending(o => o.OrderDate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public async Task<List<Order>> GetOrdersUnknown()
        {
            List<Order> orders = new List<Order>();
            try
            {
                var flowerDB = new GFlowersContext();
                orders = await flowerDB.Orders.Where(o => o.AccountId == null).OrderByDescending(o => o.OrderDate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return orders;
        }

        public Order GetOrderAdd()
        {
            try
            {
                using var flowerDB = new GFlowersContext();
                return flowerDB.Orders.OrderByDescending(o => o.OrderId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddNew(Order order)
        {
            try
            {
                if (order != null)
                {
                    var flowerDB = new GFlowersContext();
                    await flowerDB.Orders.AddAsync(order);
                    await flowerDB.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("The order is null.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateStatus(int orderId, int status)
        {
            var flowerDB = new GFlowersContext();
            try
            {
            var order = await flowerDB.Orders.FirstOrDefaultAsync(o=>o.OrderId == orderId);
            if(order != null)
            {
                order.OrderStatus = status;
                flowerDB.Orders.Update(order);
                await flowerDB.SaveChangesAsync();
            }
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

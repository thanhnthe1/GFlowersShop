using Gflower.Models;
using System.Collections.Generic;

namespace Gflower.Data.DataAccess
{
    public class OrderDetailManagement
    {
        private static OrderDetailManagement instance = null;
        private static readonly object instanceLock = new object();
        private OrderDetailManagement() { }
        public static OrderDetailManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailManagement();
                    }
                    return instance;
                }
            }
        }

        public async Task AddListOrder(List<OrderDetail>  listOrder)
        {
            try
            {
                var flowerDB = new GFlowersContext();
                await flowerDB.OrderDetails.AddRangeAsync(listOrder);
                await flowerDB.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace Gflower.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? AccountId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int OrderStatus { get; set; }
        public string? ShippingInfo { get; set; }
        public decimal? TotalPrice { get; set; }

        public virtual Account? Account { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

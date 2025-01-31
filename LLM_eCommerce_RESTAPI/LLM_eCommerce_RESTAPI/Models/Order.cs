using System;
using System.Collections.Generic;

namespace LLM_eCommerce_RESTAPI.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Payments = new HashSet<Payment>();
        }

        public int OrderId { get; set; }
        public int EfUserId { get; set; }
        public int ShippingId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }

        public virtual EfUser EfUser { get; set; } = null!;
        public virtual Shipping Shipping { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}

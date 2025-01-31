using System;
using System.Collections.Generic;

namespace LLM_eCommerce_RESTAPI.Models
{
    public partial class Shipping
    {
        public Shipping()
        {
            Orders = new HashSet<Order>();
        }

        public int ShippingId { get; set; }
        public DateTime ShippingDate { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingMethod { get; set; }
        public string? TrackingNumber { get; set; }
        public string? DeliveryStatus { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}

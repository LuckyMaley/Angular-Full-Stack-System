using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocos
{
    [Table("shippings")]
    public class Shipping
    {
        [Key, Column("shipping_id")]
        public int ShippingID { get; set; }
        
        [Column("shipping_date", TypeName ="datetime")]
        public DateTime ShipDate { get; set; }
        
        [StringLength(255), Column("shipping_address")]
        public string ShipAddress { get; set; }
        
        [StringLength(50), Column("shipping_method")]
        public string ShipMethod { get; set; }
        
        [StringLength(50), Column("tracking_number")]
        public string TrackingNumber { get; set; }
        
        [StringLength(50), Column("delivery_status")]
        public string DeliveryStatus { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}

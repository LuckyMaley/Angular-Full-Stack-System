using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocos
{
    [Table("orders")]
    public class Order
    {
        [Key, Column("order_id")]
        public int OrderID { get; set; }

        [ForeignKey("EFUser"), Required, Column("ef_user_id")]
        public int EFUserID { get; set; }
        public virtual EFUser EFUser { get; set; }

        [ForeignKey("Shipping"), Required, Column("shipping_id")]
        public int ShippingID { get; set; }
        public virtual Shipping Shipping { get; set; }

        [Column("order_date",TypeName ="datetime")]
        public DateTime OrderDate { get; set; }

        [Column("total_amount")]
        public double TotalAmount { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}

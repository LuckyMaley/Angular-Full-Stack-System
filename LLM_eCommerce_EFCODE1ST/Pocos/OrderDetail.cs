using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocos
{
    [Table("order_details")]
    public class OrderDetail
    {
        [Key, Column("order_detail_id")]
        public int OrderDetailID { get; set; }

        [ForeignKey("Order"), Required, Column("order_id")]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("Product"), Required, Column("product_id")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("unit_price")]
        public double UnitPrice { get; set; }
    }
}

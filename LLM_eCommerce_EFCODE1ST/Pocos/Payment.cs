using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocos
{
    [Table("payments")]
    public class Payment
    {
        [Key, Column("payment_id")]
        public int PaymentID { get; set; }

        [StringLength(50), Column("payment_method")]
        public string PaymentMethod { get; set; }

        [Column("payment_date",TypeName ="datetime")]
        public DateTime PaymentDate { get; set; }

        [ForeignKey("Order"), Required, Column("order_id")]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        [Column("amount")]
        public double Amount { get; set; }

        [StringLength(50), Column("status")]
        public string Status { get; set; }

    }
}

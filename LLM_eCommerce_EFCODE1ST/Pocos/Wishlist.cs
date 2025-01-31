using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocos
{
    [Table("wishlists")]
    public class Wishlist
    {
        [Key, Column("wishlist_id")]
        public int WishlistID { get; set; }

        [ForeignKey("EFUser"), Required, Column("ef_user_id")]
        public int EFUserID { get; set; }
        public virtual EFUser EFUser { get; set; }

        [ForeignKey("Product"), Required, Column("product_id")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        
        [Column("added_date", TypeName ="datetime")]
        public DateTime AddedDate { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocos
{
    [Table("products")]
    public class Product
    {
        [Key, Column("product_id")]
        public int ProductID { get; set; }

        [StringLength(100), Column("name")]
        public string Name { get; set; }

        [StringLength(100), Column("brand")]
        public string Brand { get; set; }

        [StringLength(300), Column("description")]
        public string Description { get; set; }

        [StringLength(50), Column("type")]
        public string Type { get; set; }

        [Column("price")]
        public float Price { get; set; }

        [ForeignKey("Category"), Required, Column("category_id")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }

        [Column("stock_quantity")]
        public int StockQuantity { get; set; }

        [Column("modified_date", TypeName ="datetime")]
        public DateTime ModifiedDate { get; set; }

		[Column("imageUrl")]
		public string ImageUrl { get; set; }

		public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<EFUserProduct> EFUserProducts { get; set; }
    }
}

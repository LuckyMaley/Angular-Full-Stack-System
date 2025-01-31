using System;
using System.Collections.Generic;

namespace LLM_eCommerce_RESTAPI.Models
{
    public partial class Product
    {
        public Product()
        {
            EfUserProducts = new HashSet<EfUserProduct>();
            OrderDetails = new HashSet<OrderDetail>();
            Reviews = new HashSet<Review>();
            Wishlists = new HashSet<Wishlist>();
        }

        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public float Price { get; set; }
        public int CategoryId { get; set; }
        public int StockQuantity { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ImageUrl { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<EfUserProduct> EfUserProducts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}

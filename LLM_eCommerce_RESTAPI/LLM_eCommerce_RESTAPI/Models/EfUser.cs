using System;
using System.Collections.Generic;

namespace LLM_eCommerce_RESTAPI.Models
{
    public partial class EfUser
    {
        public EfUser()
        {
            EfUserProducts = new HashSet<EfUserProduct>();
            Orders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
            Wishlists = new HashSet<Wishlist>();
        }

        public int EfUserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string IdentityUsername { get; set; } = null!;
        public string Role { get; set; } = null!;

        public virtual ICollection<EfUserProduct> EfUserProducts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
    }
}

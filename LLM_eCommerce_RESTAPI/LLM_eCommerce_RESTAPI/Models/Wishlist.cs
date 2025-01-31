using System;
using System.Collections.Generic;

namespace LLM_eCommerce_RESTAPI.Models
{
    public partial class Wishlist
    {
        public int WishlistId { get; set; }
        public int EfUserId { get; set; }
        public int ProductId { get; set; }
        public DateTime AddedDate { get; set; }

        public virtual EfUser EfUser { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}

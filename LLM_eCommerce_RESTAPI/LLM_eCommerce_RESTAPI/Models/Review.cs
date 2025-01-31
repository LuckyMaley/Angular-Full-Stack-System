using System;
using System.Collections.Generic;

namespace LLM_eCommerce_RESTAPI.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public int EfUserId { get; set; }
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        public virtual EfUser EfUser { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocos
{
    [Table("reviews")]
    public class Review
    {
        [Key, Column("review_id")]
        public int ReviewID { get; set; }

        [ForeignKey("Product"), Required, Column("product_id")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("EFUser"), Required, Column("ef_user_id")]
        public int EFUserID { get; set; }
        public virtual EFUser EFUser { get; set; }

        [Column("rating")]
        public int Rating { get; set; }

        [StringLength(50), Column("title")]
        public string Title { get; set; }

        [StringLength(300), Column("comment")]
        public string Comment { get; set; }

        [Column("review_date", TypeName ="datetime")]
        public DateTime ReviewDate { get; set; }
    }
}

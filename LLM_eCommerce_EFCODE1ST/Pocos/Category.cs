using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocos
{
    [Table("categories")]
    public class Category
    {
        [Key, Column("category_id")]
        public int CategoryID { get; set; }

        [StringLength(100), Column("name")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}

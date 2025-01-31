using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Pocos
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public Model1(DbConnection connection) : base(connection, true)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<EFUser>().Property(c => c.FirstName).IsRequired();
            modelBuilder.Entity<EFUser>().Property(c => c.LastName).IsRequired();
            modelBuilder.Entity<EFUser>().Property(c => c.Email).IsRequired();
            modelBuilder.Entity<EFUser>().Property(c => c.IdentityUserName).IsRequired();
            modelBuilder.Entity<EFUser>().Property(c => c.Role).IsRequired();
        }

        public virtual DbSet<EFUser> EFUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Shipping> Shippings { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }
        public virtual DbSet<EFUserProduct> EFUserProducts { get; set; }
    }
}

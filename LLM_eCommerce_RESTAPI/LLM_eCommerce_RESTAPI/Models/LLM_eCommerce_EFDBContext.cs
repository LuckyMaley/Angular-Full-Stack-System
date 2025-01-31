using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LLM_eCommerce_RESTAPI.Models
{
    public partial class LLM_eCommerce_EFDBContext : DbContext
    {
        public LLM_eCommerce_EFDBContext()
        {
        }

        public LLM_eCommerce_EFDBContext(DbContextOptions<LLM_eCommerce_EFDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<EfUser> EfUsers { get; set; } = null!;
        public virtual DbSet<EfUserProduct> EfUserProducts { get; set; } = null!;
        public virtual DbSet<MigrationHistory> MigrationHistories { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Shipping> Shippings { get; set; } = null!;
        public virtual DbSet<Wishlist> Wishlists { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<EfUser>(entity =>
            {
                entity.ToTable("ef_users");

                entity.Property(e => e.EfUserId).HasColumnName("ef_user_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("first_name");

                entity.Property(e => e.IdentityUsername)
                    .HasMaxLength(100)
                    .HasColumnName("identity_username");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("last_name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Role)
                    .HasMaxLength(100)
                    .HasColumnName("role");
            });

            modelBuilder.Entity<EfUserProduct>(entity =>
            {
                entity.ToTable("ef_user_product");

                entity.HasIndex(e => e.EfUserId, "IX_ef_user_id");

                entity.HasIndex(e => e.ProductId, "IX_product_id");

                entity.Property(e => e.EfUserProductId).HasColumnName("ef_user_product_id");

                entity.Property(e => e.AddedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("added_date");

                entity.Property(e => e.EfUserId).HasColumnName("ef_user_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.HasOne(d => d.EfUser)
                    .WithMany(p => p.EfUserProducts)
                    .HasForeignKey(d => d.EfUserId)
                    .HasConstraintName("FK_dbo.ef_user_product_dbo.ef_users_ef_user_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.EfUserProducts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_dbo.ef_user_product_dbo.products_product_id");
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey })
                    .HasName("PK_dbo.__MigrationHistory");

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.ProductVersion).HasMaxLength(32);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasIndex(e => e.EfUserId, "IX_ef_user_id");

                entity.HasIndex(e => e.ShippingId, "IX_shipping_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.EfUserId).HasColumnName("ef_user_id");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("order_date");

                entity.Property(e => e.ShippingId).HasColumnName("shipping_id");

                entity.Property(e => e.TotalAmount).HasColumnName("total_amount");

                entity.HasOne(d => d.EfUser)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EfUserId)
                    .HasConstraintName("FK_dbo.orders_dbo.ef_users_ef_user_id");

                entity.HasOne(d => d.Shipping)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShippingId)
                    .HasConstraintName("FK_dbo.orders_dbo.shippings_shipping_id");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("order_details");

                entity.HasIndex(e => e.OrderId, "IX_order_id");

                entity.HasIndex(e => e.ProductId, "IX_product_id");

                entity.Property(e => e.OrderDetailId).HasColumnName("order_detail_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_dbo.order_details_dbo.orders_order_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_dbo.order_details_dbo.products_product_id");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payments");

                entity.HasIndex(e => e.OrderId, "IX_order_id");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("payment_date");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .HasColumnName("payment_method");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_dbo.payments_dbo.orders_order_id");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.HasIndex(e => e.CategoryId, "IX_category_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Brand)
                    .HasMaxLength(100)
                    .HasColumnName("brand");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(300)
                    .HasColumnName("description");

                entity.Property(e => e.ImageUrl).HasColumnName("imageUrl");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_date");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_dbo.products_dbo.categories_category_id");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("reviews");

                entity.HasIndex(e => e.EfUserId, "IX_ef_user_id");

                entity.HasIndex(e => e.ProductId, "IX_product_id");

                entity.Property(e => e.ReviewId).HasColumnName("review_id");

                entity.Property(e => e.Comment)
                    .HasMaxLength(300)
                    .HasColumnName("comment");

                entity.Property(e => e.EfUserId).HasColumnName("ef_user_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.ReviewDate)
                    .HasColumnType("datetime")
                    .HasColumnName("review_date");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.HasOne(d => d.EfUser)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.EfUserId)
                    .HasConstraintName("FK_dbo.reviews_dbo.ef_users_ef_user_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_dbo.reviews_dbo.products_product_id");
            });

            modelBuilder.Entity<Shipping>(entity =>
            {
                entity.ToTable("shippings");

                entity.Property(e => e.ShippingId).HasColumnName("shipping_id");

                entity.Property(e => e.DeliveryStatus)
                    .HasMaxLength(50)
                    .HasColumnName("delivery_status");

                entity.Property(e => e.ShippingAddress)
                    .HasMaxLength(255)
                    .HasColumnName("shipping_address");

                entity.Property(e => e.ShippingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("shipping_date");

                entity.Property(e => e.ShippingMethod)
                    .HasMaxLength(50)
                    .HasColumnName("shipping_method");

                entity.Property(e => e.TrackingNumber)
                    .HasMaxLength(50)
                    .HasColumnName("tracking_number");
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.ToTable("wishlists");

                entity.HasIndex(e => e.EfUserId, "IX_ef_user_id");

                entity.HasIndex(e => e.ProductId, "IX_product_id");

                entity.Property(e => e.WishlistId).HasColumnName("wishlist_id");

                entity.Property(e => e.AddedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("added_date");

                entity.Property(e => e.EfUserId).HasColumnName("ef_user_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.HasOne(d => d.EfUser)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.EfUserId)
                    .HasConstraintName("FK_dbo.wishlists_dbo.ef_users_ef_user_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_dbo.wishlists_dbo.products_product_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

namespace Pocos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialDBCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.categories",
                c => new
                    {
                        category_id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.category_id);
            
            CreateTable(
                "dbo.products",
                c => new
                    {
                        product_id = c.Int(nullable: false, identity: true),
                        name = c.String(maxLength: 100),
                        brand = c.String(maxLength: 100),
                        description = c.String(maxLength: 300),
                        type = c.String(maxLength: 50),
                        price = c.Single(nullable: false),
                        category_id = c.Int(nullable: false),
                        stock_quantity = c.Int(nullable: false),
                        modified_date = c.DateTime(nullable: false),
                        imageUrl = c.String(),
                    })
                .PrimaryKey(t => t.product_id)
                .ForeignKey("dbo.categories", t => t.category_id, cascadeDelete: true)
                .Index(t => t.category_id);
            
            CreateTable(
                "dbo.ef_user_product",
                c => new
                    {
                        ef_user_product_id = c.Int(nullable: false, identity: true),
                        ef_user_id = c.Int(nullable: false),
                        product_id = c.Int(nullable: false),
                        added_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ef_user_product_id)
                .ForeignKey("dbo.ef_users", t => t.ef_user_id, cascadeDelete: true)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .Index(t => t.ef_user_id)
                .Index(t => t.product_id);
            
            CreateTable(
                "dbo.ef_users",
                c => new
                    {
                        ef_user_id = c.Int(nullable: false, identity: true),
                        first_name = c.String(nullable: false, maxLength: 50),
                        last_name = c.String(nullable: false, maxLength: 50),
                        email = c.String(nullable: false, maxLength: 100),
                        address = c.String(maxLength: 255),
                        phone_number = c.String(maxLength: 50),
                        identity_username = c.String(nullable: false, maxLength: 100),
                        role = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ef_user_id);
            
            CreateTable(
                "dbo.orders",
                c => new
                    {
                        order_id = c.Int(nullable: false, identity: true),
                        ef_user_id = c.Int(nullable: false),
                        shipping_id = c.Int(nullable: false),
                        order_date = c.DateTime(nullable: false),
                        total_amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.order_id)
                .ForeignKey("dbo.ef_users", t => t.ef_user_id, cascadeDelete: true)
                .ForeignKey("dbo.shippings", t => t.shipping_id, cascadeDelete: true)
                .Index(t => t.ef_user_id)
                .Index(t => t.shipping_id);
            
            CreateTable(
                "dbo.order_details",
                c => new
                    {
                        order_detail_id = c.Int(nullable: false, identity: true),
                        order_id = c.Int(nullable: false),
                        product_id = c.Int(nullable: false),
                        quantity = c.Int(nullable: false),
                        unit_price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.order_detail_id)
                .ForeignKey("dbo.orders", t => t.order_id, cascadeDelete: true)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .Index(t => t.order_id)
                .Index(t => t.product_id);
            
            CreateTable(
                "dbo.payments",
                c => new
                    {
                        payment_id = c.Int(nullable: false, identity: true),
                        payment_method = c.String(maxLength: 50),
                        payment_date = c.DateTime(nullable: false),
                        order_id = c.Int(nullable: false),
                        amount = c.Double(nullable: false),
                        status = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.payment_id)
                .ForeignKey("dbo.orders", t => t.order_id, cascadeDelete: true)
                .Index(t => t.order_id);
            
            CreateTable(
                "dbo.shippings",
                c => new
                    {
                        shipping_id = c.Int(nullable: false, identity: true),
                        shipping_date = c.DateTime(nullable: false),
                        shipping_address = c.String(maxLength: 255),
                        shipping_method = c.String(maxLength: 50),
                        tracking_number = c.String(maxLength: 50),
                        delivery_status = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.shipping_id);
            
            CreateTable(
                "dbo.reviews",
                c => new
                    {
                        review_id = c.Int(nullable: false, identity: true),
                        product_id = c.Int(nullable: false),
                        ef_user_id = c.Int(nullable: false),
                        rating = c.Int(nullable: false),
                        title = c.String(maxLength: 50),
                        comment = c.String(maxLength: 300),
                        review_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.review_id)
                .ForeignKey("dbo.ef_users", t => t.ef_user_id, cascadeDelete: true)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .Index(t => t.product_id)
                .Index(t => t.ef_user_id);
            
            CreateTable(
                "dbo.wishlists",
                c => new
                    {
                        wishlist_id = c.Int(nullable: false, identity: true),
                        ef_user_id = c.Int(nullable: false),
                        product_id = c.Int(nullable: false),
                        added_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.wishlist_id)
                .ForeignKey("dbo.ef_users", t => t.ef_user_id, cascadeDelete: true)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .Index(t => t.ef_user_id)
                .Index(t => t.product_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ef_user_product", "product_id", "dbo.products");
            DropForeignKey("dbo.ef_user_product", "ef_user_id", "dbo.ef_users");
            DropForeignKey("dbo.wishlists", "product_id", "dbo.products");
            DropForeignKey("dbo.wishlists", "ef_user_id", "dbo.ef_users");
            DropForeignKey("dbo.reviews", "product_id", "dbo.products");
            DropForeignKey("dbo.reviews", "ef_user_id", "dbo.ef_users");
            DropForeignKey("dbo.orders", "shipping_id", "dbo.shippings");
            DropForeignKey("dbo.payments", "order_id", "dbo.orders");
            DropForeignKey("dbo.order_details", "product_id", "dbo.products");
            DropForeignKey("dbo.order_details", "order_id", "dbo.orders");
            DropForeignKey("dbo.orders", "ef_user_id", "dbo.ef_users");
            DropForeignKey("dbo.products", "category_id", "dbo.categories");
            DropIndex("dbo.wishlists", new[] { "product_id" });
            DropIndex("dbo.wishlists", new[] { "ef_user_id" });
            DropIndex("dbo.reviews", new[] { "ef_user_id" });
            DropIndex("dbo.reviews", new[] { "product_id" });
            DropIndex("dbo.payments", new[] { "order_id" });
            DropIndex("dbo.order_details", new[] { "product_id" });
            DropIndex("dbo.order_details", new[] { "order_id" });
            DropIndex("dbo.orders", new[] { "shipping_id" });
            DropIndex("dbo.orders", new[] { "ef_user_id" });
            DropIndex("dbo.ef_user_product", new[] { "product_id" });
            DropIndex("dbo.ef_user_product", new[] { "ef_user_id" });
            DropIndex("dbo.products", new[] { "category_id" });
            DropTable("dbo.wishlists");
            DropTable("dbo.reviews");
            DropTable("dbo.shippings");
            DropTable("dbo.payments");
            DropTable("dbo.order_details");
            DropTable("dbo.orders");
            DropTable("dbo.ef_users");
            DropTable("dbo.ef_user_product");
            DropTable("dbo.products");
            DropTable("dbo.categories");
        }
    }
}

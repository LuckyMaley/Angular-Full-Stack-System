namespace Pocos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialDBCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.admins",
                c => new
                    {
                        admin_id = c.Int(nullable: false, identity: true),
                        first_name = c.String(nullable: false, maxLength: 50),
                        surname = c.String(nullable: false, maxLength: 50),
                        email = c.String(nullable: false, maxLength: 100),
                        username = c.String(nullable: false, maxLength: 100),
                        address = c.String(maxLength: 300),
                        phone_number = c.String(maxLength: 50),
                        role = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.admin_id);
            
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
                    })
                .PrimaryKey(t => t.product_id)
                .ForeignKey("dbo.categories", t => t.category_id, cascadeDelete: true)
                .Index(t => t.category_id);
            
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
                "dbo.orders",
                c => new
                    {
                        order_id = c.Int(nullable: false, identity: true),
                        customer_id = c.Int(nullable: false),
                        shipping_id = c.Int(nullable: false),
                        order_date = c.DateTime(nullable: false),
                        total_amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.order_id)
                .ForeignKey("dbo.customers", t => t.customer_id, cascadeDelete: true)
                .ForeignKey("dbo.shippings", t => t.shipping_id, cascadeDelete: true)
                .Index(t => t.customer_id)
                .Index(t => t.shipping_id);
            
            CreateTable(
                "dbo.customers",
                c => new
                    {
                        customer_id = c.Int(nullable: false, identity: true),
                        first_name = c.String(nullable: false, maxLength: 50),
                        surname = c.String(nullable: false, maxLength: 50),
                        email = c.String(nullable: false, maxLength: 100),
                        username = c.String(nullable: false, maxLength: 100),
                        address = c.String(maxLength: 100),
                        phone_number = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.customer_id);
            
            CreateTable(
                "dbo.reviews",
                c => new
                    {
                        review_id = c.Int(nullable: false, identity: true),
                        product_id = c.Int(nullable: false),
                        customer_id = c.Int(nullable: false),
                        rating = c.Int(nullable: false),
                        title = c.String(maxLength: 50),
                        comment = c.String(maxLength: 300),
                        review_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.review_id)
                .ForeignKey("dbo.customers", t => t.customer_id, cascadeDelete: true)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .Index(t => t.product_id)
                .Index(t => t.customer_id);
            
            CreateTable(
                "dbo.wishlists",
                c => new
                    {
                        wishlist_id = c.Int(nullable: false, identity: true),
                        customer_id = c.Int(nullable: false),
                        product_id = c.Int(nullable: false),
                        added_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.wishlist_id)
                .ForeignKey("dbo.customers", t => t.customer_id, cascadeDelete: true)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .Index(t => t.customer_id)
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
                "dbo.seller_product",
                c => new
                    {
                        seller_product_id = c.Int(nullable: false, identity: true),
                        seller_id = c.Int(nullable: false),
                        product_id = c.Int(nullable: false),
                        added_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.seller_product_id)
                .ForeignKey("dbo.products", t => t.product_id, cascadeDelete: true)
                .ForeignKey("dbo.seller", t => t.seller_id, cascadeDelete: true)
                .Index(t => t.seller_id)
                .Index(t => t.product_id);
            
            CreateTable(
                "dbo.seller",
                c => new
                    {
                        seller_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 50),
                        email = c.String(nullable: false, maxLength: 100),
                        username = c.String(nullable: false, maxLength: 100),
                        type_of_seller = c.String(nullable: false, maxLength: 100),
                        address = c.String(maxLength: 100),
                        phone_number = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.seller_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.seller_product", "seller_id", "dbo.seller");
            DropForeignKey("dbo.seller_product", "product_id", "dbo.products");
            DropForeignKey("dbo.order_details", "product_id", "dbo.products");
            DropForeignKey("dbo.order_details", "order_id", "dbo.orders");
            DropForeignKey("dbo.orders", "shipping_id", "dbo.shippings");
            DropForeignKey("dbo.payments", "order_id", "dbo.orders");
            DropForeignKey("dbo.orders", "customer_id", "dbo.customers");
            DropForeignKey("dbo.wishlists", "product_id", "dbo.products");
            DropForeignKey("dbo.wishlists", "customer_id", "dbo.customers");
            DropForeignKey("dbo.reviews", "product_id", "dbo.products");
            DropForeignKey("dbo.reviews", "customer_id", "dbo.customers");
            DropForeignKey("dbo.products", "category_id", "dbo.categories");
            DropIndex("dbo.seller_product", new[] { "product_id" });
            DropIndex("dbo.seller_product", new[] { "seller_id" });
            DropIndex("dbo.payments", new[] { "order_id" });
            DropIndex("dbo.wishlists", new[] { "product_id" });
            DropIndex("dbo.wishlists", new[] { "customer_id" });
            DropIndex("dbo.reviews", new[] { "customer_id" });
            DropIndex("dbo.reviews", new[] { "product_id" });
            DropIndex("dbo.orders", new[] { "shipping_id" });
            DropIndex("dbo.orders", new[] { "customer_id" });
            DropIndex("dbo.order_details", new[] { "product_id" });
            DropIndex("dbo.order_details", new[] { "order_id" });
            DropIndex("dbo.products", new[] { "category_id" });
            DropTable("dbo.seller");
            DropTable("dbo.seller_product");
            DropTable("dbo.shippings");
            DropTable("dbo.payments");
            DropTable("dbo.wishlists");
            DropTable("dbo.reviews");
            DropTable("dbo.customers");
            DropTable("dbo.orders");
            DropTable("dbo.order_details");
            DropTable("dbo.products");
            DropTable("dbo.categories");
            DropTable("dbo.admins");
        }
    }
}

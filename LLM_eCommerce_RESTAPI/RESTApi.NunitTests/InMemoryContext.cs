using FluentAssertions;
using LLM_eCommerce_RESTAPI.AuthModels;
using LLM_eCommerce_RESTAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RESTApi.NunitTests
{
    public class InMemoryContext
    {
        public static Object GeneratedDB()
        {

            var _contextOptions = new DbContextOptionsBuilder<LLM_eCommerce_EFDBContext>()
                .UseInMemoryDatabase("ControllerTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            var context = new LLM_eCommerce_EFDBContext(_contextOptions);
            
            EFDBSeedData(context);

            return context;
        }

        public static void EFDBSeedData(LLM_eCommerce_EFDBContext _eCommerceContext)
        {
            
            _eCommerceContext.Categories.Add(new Category { CategoryId = 1, Name = "Sneaker" });
            _eCommerceContext.Categories.Add(new Category { CategoryId = 2, Name = "Sandals" });
            _eCommerceContext.Categories.Add(new Category { CategoryId = 3, Name = "T-shirt" });
            _eCommerceContext.Categories.Add(new Category { CategoryId = 4, Name = "Shirt" });
            _eCommerceContext.Categories.Add(new Category { CategoryId = 5, Name = "Jacket" });
            _eCommerceContext.Categories.Add(new Category { CategoryId = 6, Name = "Hoodie" });
            _eCommerceContext.Categories.Add(new Category { CategoryId = 7, Name = "Jersey" });
            _eCommerceContext.Categories.Add(new Category { CategoryId = 8, Name = "Jeans" });
            _eCommerceContext.Categories.Add(new Category { CategoryId = 9, Name = "Shorts" });
            _eCommerceContext.Categories.Add(new Category { CategoryId = 10, Name = "Hats" });

            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 1, FirstName = "Zandile", LastName = "Zimela", Email = "Zzimela@gmail.com", IdentityUsername = "ZzimelaAdmin", Address = "18 Jack avenue, 2001", PhoneNumber = "0743244345", Role = "Administrator" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 2, FirstName = "Andile", LastName = "Zuma", Email = "Azuma22@gmail.com", IdentityUsername = "Azuma22Admin", Address = "2003 Field street, 4001", PhoneNumber = "0844544278", Role = "Administrator" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 3, FirstName = "Jack", LastName = "Harrison", Email = "Harrionj01@gmail.com", IdentityUsername = "Harrison01Admin", Address = "230 West street, 4001", PhoneNumber = "0675647385", Role = "Administrator" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 4, FirstName = "Hannah", LastName = "Sancho", Email = "Sanchoh@gmail.com", IdentityUsername = "SanchohAdmin", Address = "15 Zimbali avenue, 4501", PhoneNumber = "0673004545", Role = "Administrator" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 5, FirstName = "Zack", LastName = "Wake", Email = "Wakez007@gmail.com", IdentityUsername = "Wakez007Admin", Address = "28 Sandton avenue, 2001", PhoneNumber = "0745667386", Role = "Administrator" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 6, FirstName = "Bruno", LastName = "Sterling", Email = "Bruno22@gmail.com", IdentityUsername = "Brunos22Admin", Address = "20 King Edwards avenue, 3201", PhoneNumber = "0843778347", Role = "Administrator" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 7, FirstName = "Luis", LastName = "Diaz", Email = "Luisd7@gmail.com", IdentityUsername = "Luis7Admin", Address = "19 Jack avenue, 2001", PhoneNumber = "0742687829", Role = "Administrator" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 8, FirstName = "Cristiano", LastName = "Walker", Email = "Walkerc07@gmail.com", IdentityUsername = "Walkerc07Admin", Address = "15 Harrison avenue, 3001", PhoneNumber = "0743264748", Role = "Administrator" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 9, FirstName = "Phil", LastName = "Foden", Email = "Fodenp47@gmail.com", IdentityUsername = "Fodenp47Admin", Address = "100 Everton street, 6001", PhoneNumber = "0746277885", Role = "Administrator" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 10, FirstName = "Kylian", LastName = "Carrick", Email = "Kcarrick@gmail.com", IdentityUsername = "KcarrickAdmin", Address = "20 Jack avenue, 2001", PhoneNumber = "0675249849", Role = "Administrator" });

            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 11, FirstName = "Corliss", LastName = "Farquarson", Email = "cfarquarson0@cam.ac.uk", IdentityUsername = "cfarquarson0", Address = "25629 Fulton Pass", PhoneNumber = "0546292527", Role = "Seller" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 12, FirstName = "Helyn", LastName = "Limeburn", Email = "hlimeburn1@adobe.com", IdentityUsername = "hlimeburn1", Address = "416 3rd Lane", PhoneNumber = "0435640354", Role = "Seller" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 13, FirstName = "Micki", LastName = "Primarolo", Email = "mprimarolo2@mit.edu", IdentityUsername = "mprimarolo2", Address = "72 Novick Junction", PhoneNumber = "0636511052", Role = "Seller" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 14, FirstName = "Ricca", LastName = "Ellen", Email = "rellen3@fema.gov", IdentityUsername = "rellen3", Address = "37 Dorton Road", PhoneNumber = "0322172307", Role = "Seller" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 15, FirstName = "Lorilyn", LastName = "Gonneau", Email = "lgonneau4@hubpages.com", IdentityUsername = "lgonneau4", Address = "75 Esker Center", PhoneNumber = "0538631803", Role = "Seller" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 16, FirstName = "Hagen", LastName = "Clerc", Email = "hclerc5@psu.edu", IdentityUsername = "hclerc5", Address = "58895 Montana Lane", PhoneNumber = "0385709825", Role = "Seller" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 17, FirstName = "Drusi", LastName = "Cullinan", Email = "dcullinan6@nasa.gov", IdentityUsername = "dcullinan6", Address = "950 Sycamore Point", PhoneNumber = "0158497651", Role = "Seller" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 18, FirstName = "Cristy", LastName = "Penhalewick", Email = "cpenhalewick7@va.gov", IdentityUsername = "cpenhalewick7", Address = "96 Golf Course Terrace", PhoneNumber = "0427359373", Role = "Seller" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 19, FirstName = "Riccardo", LastName = "Pohls", Email = "rpohls8@va.gov", IdentityUsername = "rpohls8", Address = "70485 Holmberg Way", PhoneNumber = "0528137558", Role = "Seller" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 20, FirstName = "Helyn", LastName = "Jayes", Email = "mjayes9@jimdo.com", IdentityUsername = "mjayes9", Address = "6 Paget Hill", PhoneNumber = "0729662474", Role = "Seller" });

            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 21, FirstName = "Zack", LastName = "Efron", Email = "Efronz@gmail.com", IdentityUsername = "Efronz", Address = "15 Zuma avenue, 2001", PhoneNumber = "074396748", Role = "Customer" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 22, FirstName = "Jacob", LastName = "Zuma", Email = "Jzuma11@gmail.com", IdentityUsername = "Jzuma11", Address = "13 Commercial street, 4001", PhoneNumber = "0670004175", Role = "Customer" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 23, FirstName = "Nelson", LastName = "Mandela", Email = "Nelsonm01@gmail.com", IdentityUsername = "Nelsonm01", Address = "300 West street, 4001", PhoneNumber = "0675540000", Role = "Customer" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 24, FirstName = "Jadon", LastName = "Sancho", Email = "Sanchoj@gmail.com", IdentityUsername = "Sanchoj", Address = "13 Zimbali avenue, 4501", PhoneNumber = "0677008594", Role = "Customer" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 25, FirstName = "Julian", LastName = "Alvarez", Email = "Jalvarez007@gmail.com", IdentityUsername = "Jalvarez007", Address = "51 Sandton avenue, 2001", PhoneNumber = "0848767080", Role = "Customer" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 26, FirstName = "Eden", LastName = "Hazard", Email = "Hazard10@gmail.com", IdentityUsername = "Hazard10", Address = "10 King Edwards avenue, 3201", PhoneNumber = "0840004365", Role = "Customer" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 27, FirstName = "Tammy", LastName = "Abraham", Email = "Abrahamt@gmail.com", IdentityUsername = "Abrahamt", Address = "16 Allison avenue, 2001", PhoneNumber = "0742605859", Role = "Customer" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 28, FirstName = "Sanele", LastName = "Mbatha", Email = "Mbathas07@gmail.com", IdentityUsername = "Mbathas07", Address = "22 Zakhele avenue, 3001", PhoneNumber = "0744254040", Role = "Customer" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 29, FirstName = "Sinokuhle", LastName = "Gumede", Email = "Gumedes01@gmail.com", IdentityUsername = "Gumedes01", Address = "100 Marchester street, 6001", PhoneNumber = "0749457005", Role = "Customer" });
            _eCommerceContext.EfUsers.Add(new EfUser { EfUserId = 30, FirstName = "Amanda", LastName = "Nzama", Email = "Amandan01@gmail.com", IdentityUsername = "Amandan", Address = "20 Zuma avenue, 2001", PhoneNumber = "0676709940", Role = "Customer" });

            _eCommerceContext.Products.Add(new Product { ProductId = 1, Name = "Nike Air Force 1", Brand = "Nike", Description = "Nike Air Force 1 sneakers for track wear", Type = "Men", Price = 2100.99f, CategoryId = 1, StockQuantity = 100, ModifiedDate = new DateTime(2018, 11, 12, 12, 45, 00) });
            _eCommerceContext.Products.Add(new Product { ProductId = 2, Name = "Adidas Yeezy", Brand = "Adidas", Description = "Adidas Yeezy sneakers for summer wear", Type = "Women", Price = 2700.99f, CategoryId = 1, StockQuantity = 100, ModifiedDate = new DateTime(2017, 1, 22, 15, 35, 10) });
            _eCommerceContext.Products.Add(new Product { ProductId = 3, Name = "Puma T-shirt", Brand = "Puma", Description = "Puma T-shirt for the summer and track wear", Type = "Women", Price = 900.99f, CategoryId = 3, StockQuantity = 100, ModifiedDate = new DateTime(2019, 10, 12, 12, 45, 00) });
            _eCommerceContext.Products.Add(new Product { ProductId = 4, Name = "Reebok T-shirt", Brand = "Reebok", Description = "Reebok T-shirt for sportswear", Type = "Men", Price = 1000.99f, CategoryId = 3, StockQuantity = 100, ModifiedDate = new DateTime(2018, 1, 12, 12, 45, 00) });
            _eCommerceContext.Products.Add(new Product { ProductId = 5, Name = "Redbat Jean", Brand = "Redbat", Description = "Redbat Jean for casual wear", Type = "Men", Price = 799.99f, CategoryId = 8, StockQuantity = 100, ModifiedDate = new DateTime(2018, 2, 12, 21, 47, 50) });
            _eCommerceContext.Products.Add(new Product { ProductId = 6, Name = "Relay Jeans", Brand = "Relay", Description = "Relay jeans for causal  wear", Type = "Men", Price = 800.99f, CategoryId = 8, StockQuantity = 100, ModifiedDate = new DateTime(2019, 9, 13, 8, 5, 40) });
            _eCommerceContext.Products.Add(new Product { ProductId = 7, Name = "Replay Sandals", Brand = "Replay", Description = "Replay sandals for summer wear", Type = "Men", Price = 200.99f, CategoryId = 2, StockQuantity = 100, ModifiedDate = new DateTime(2020, 1, 2, 13, 35, 00) });
            _eCommerceContext.Products.Add(new Product { ProductId = 8, Name = "Raw T-shirt", Brand = "Raw", Description = "Raw T-shirt for casual wear", Type = "Men", Price = 2200.99f, CategoryId = 3, StockQuantity = 100, ModifiedDate = new DateTime(2021, 11, 12, 12, 45, 00) });
            _eCommerceContext.Products.Add(new Product { ProductId = 9, Name = "Gant shirt", Brand = "Gant", Description = "Gant shirt for formal wear", Type = "Men", Price = 1100.99f, CategoryId = 4, StockQuantity = 100, ModifiedDate = new DateTime(2022, 11, 10, 10, 40, 00) });
            _eCommerceContext.Products.Add(new Product { ProductId = 10, Name = "Converse Allstar", Brand = "Converse", Description = "Converse sneakers for casual wear", Type = "Men", Price = 2700.99f, CategoryId = 1, StockQuantity = 100, ModifiedDate = new DateTime(2019, 3, 22, 16, 5, 20) });

            _eCommerceContext.Shippings.Add(new Shipping() { ShippingId = 1, ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00), ShippingAddress = "15 Zuma avenue, 2001", ShippingMethod = "Courier", TrackingNumber = "2D222236", DeliveryStatus = "Delivered" });
            _eCommerceContext.Shippings.Add(new Shipping() { ShippingId = 2, ShippingDate = new DateTime(2017, 4, 25, 15, 45, 10), ShippingAddress = "13 Commercial street, 4001", ShippingMethod = "Courier", TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8), DeliveryStatus = "Delivered" });
            _eCommerceContext.Shippings.Add(new Shipping() { ShippingId = 3, ShippingDate = new DateTime(2019, 11, 16, 12, 55, 00), ShippingAddress = "300 West street, 4001", ShippingMethod = "Courier", TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8), DeliveryStatus = "Delivered" });
            _eCommerceContext.Shippings.Add(new Shipping() { ShippingId = 4, ShippingDate = new DateTime(2018, 5, 16, 12, 55, 00), ShippingAddress = "13 Zimbali avenue, 4501", ShippingMethod = "Courier", TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8), DeliveryStatus = "Delivered" });
            _eCommerceContext.Shippings.Add(new Shipping() { ShippingId = 5, ShippingDate = new DateTime(2018, 3, 16, 21, 57, 50), ShippingAddress = "51 Sandton avenue, 2001", ShippingMethod = "Courier", TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8), DeliveryStatus = "Delivered" });
            _eCommerceContext.Shippings.Add(new Shipping() { ShippingId = 6, ShippingDate = new DateTime(2019, 10, 17, 8, 6, 40), ShippingAddress = "10 King Edwards avenue, 3201", ShippingMethod = "Courier", TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8), DeliveryStatus = "Delivered" });
            _eCommerceContext.Shippings.Add(new Shipping() { ShippingId = 7, ShippingDate = new DateTime(2020, 2, 5, 13, 45, 00), ShippingAddress = "16 Allison avenue, 2001", ShippingMethod = "Courier", TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8), DeliveryStatus = "Delivered" });
            _eCommerceContext.Shippings.Add(new Shipping() { ShippingId = 8, ShippingDate = new DateTime(2021, 12, 16, 12, 55, 00), ShippingAddress = "22 Zakhele avenue, 3001", ShippingMethod = "Courier", TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8), DeliveryStatus = "Delivered" });
            _eCommerceContext.Shippings.Add(new Shipping() { ShippingId = 9, ShippingDate = new DateTime(2022, 12, 14, 10, 50, 00), ShippingAddress = "100 Marchester street, 6001", ShippingMethod = "Courier", TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8), DeliveryStatus = "Delivered" });
            _eCommerceContext.Shippings.Add(new Shipping() { ShippingId = 10, ShippingDate = new DateTime(2020, 3, 25, 16, 10, 20), ShippingAddress = "20 Zuma avenue, 2001", ShippingMethod = "Courier", TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8), DeliveryStatus = "Delivered" });

            _eCommerceContext.Orders.Add(new Order() { OrderId = 1, EfUserId = 21, ShippingId = 1, OrderDate = new DateTime(2023, 11, 12, 12, 45, 00), TotalAmount = 2100.99 });
            _eCommerceContext.Orders.Add(new Order() { OrderId = 2, EfUserId = 22, ShippingId = 2, OrderDate = new DateTime(2017, 4, 22, 15, 35, 10), TotalAmount = 2700.99 });
            _eCommerceContext.Orders.Add(new Order() { OrderId = 3, EfUserId = 23, ShippingId = 3, OrderDate = new DateTime(2019, 11, 12, 12, 45, 00), TotalAmount = 900.99 });
            _eCommerceContext.Orders.Add(new Order() { OrderId = 4, EfUserId = 24, ShippingId = 4, OrderDate = new DateTime(2018, 5, 12, 12, 45, 00), TotalAmount = 1000.99 });
            _eCommerceContext.Orders.Add(new Order() { OrderId = 5, EfUserId = 25, ShippingId = 5, OrderDate = new DateTime(2018, 3, 21, 21, 47, 50), TotalAmount = 799.99 });
            _eCommerceContext.Orders.Add(new Order() { OrderId = 6, EfUserId = 26, ShippingId = 6, OrderDate = new DateTime(2019, 10, 8, 8, 5, 40), TotalAmount = 800.99 });
            _eCommerceContext.Orders.Add(new Order() { OrderId = 7, EfUserId = 27, ShippingId = 7, OrderDate = new DateTime(2020, 2, 13, 13, 35, 00), TotalAmount = 200.99 });
            _eCommerceContext.Orders.Add(new Order() { OrderId = 8, EfUserId = 28, ShippingId = 8, OrderDate = new DateTime(2021, 12, 12, 12, 45, 00), TotalAmount = 2200.99 });
            _eCommerceContext.Orders.Add(new Order() { OrderId = 9, EfUserId = 29, ShippingId = 9, OrderDate = new DateTime(2022, 12, 10, 10, 40, 00), TotalAmount = 1100.99 });
            _eCommerceContext.Orders.Add(new Order() { OrderId = 10, EfUserId = 30, ShippingId = 10, OrderDate = new DateTime(2020, 3, 16, 16, 5, 20), TotalAmount = 2700.99 });

            _eCommerceContext.OrderDetails.Add(new OrderDetail { OrderDetailId = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 2100.99 });
            _eCommerceContext.OrderDetails.Add(new OrderDetail { OrderDetailId = 2, OrderId = 2, ProductId = 2, Quantity = 1, UnitPrice = 2700.99 });
            _eCommerceContext.OrderDetails.Add(new OrderDetail { OrderDetailId = 3, OrderId = 3, ProductId = 3, Quantity = 1, UnitPrice = 900.99 });
            _eCommerceContext.OrderDetails.Add(new OrderDetail { OrderDetailId = 4, OrderId = 4, ProductId = 4, Quantity = 1, UnitPrice = 1000.99 });
            _eCommerceContext.OrderDetails.Add(new OrderDetail { OrderDetailId = 5, OrderId = 5, ProductId = 5, Quantity = 1, UnitPrice = 799.99 });
            _eCommerceContext.OrderDetails.Add(new OrderDetail { OrderDetailId = 6, OrderId = 6, ProductId = 6, Quantity = 1, UnitPrice = 800.99 });
            _eCommerceContext.OrderDetails.Add(new OrderDetail { OrderDetailId = 7, OrderId = 7, ProductId = 7, Quantity = 1, UnitPrice = 200.99 });
            _eCommerceContext.OrderDetails.Add(new OrderDetail { OrderDetailId = 8, OrderId = 8, ProductId = 8, Quantity = 1, UnitPrice = 2200.99 });
            _eCommerceContext.OrderDetails.Add(new OrderDetail { OrderDetailId = 9, OrderId = 9, ProductId = 9, Quantity = 1, UnitPrice = 1100.99 });
            _eCommerceContext.OrderDetails.Add(new OrderDetail { OrderDetailId = 10, OrderId = 10, ProductId = 10, Quantity = 1, UnitPrice = 2700.99 });

            _eCommerceContext.Payments.Add(new Payment { PaymentId = 1, OrderId = 1, PaymentDate = new DateTime(2023, 11, 12, 12, 55, 00), PaymentMethod = "Card", Amount = 2100.99, Status = "Paid" });
            _eCommerceContext.Payments.Add(new Payment { PaymentId = 2, OrderId = 2, PaymentDate = new DateTime(2017, 4, 22, 15, 45, 10), PaymentMethod = "Card", Amount = 2700.99, Status = "Paid" });
            _eCommerceContext.Payments.Add(new Payment { PaymentId = 3, OrderId = 3, PaymentDate = new DateTime(2019, 11, 12, 12, 55, 00), PaymentMethod = "Card", Amount = 900.99, Status = "Paid" });
            _eCommerceContext.Payments.Add(new Payment { PaymentId = 4, OrderId = 4, PaymentDate = new DateTime(2018, 5, 12, 12, 55, 00), PaymentMethod = "Card", Amount = 1000.99, Status = "Paid" });
            _eCommerceContext.Payments.Add(new Payment { PaymentId = 5, OrderId = 5, PaymentDate = new DateTime(2018, 3, 12, 21, 57, 50), PaymentMethod = "Card", Amount = 799.99, Status = "Paid" });
            _eCommerceContext.Payments.Add(new Payment { PaymentId = 6, OrderId = 6, PaymentDate = new DateTime(2019, 10, 13, 8, 6, 40), PaymentMethod = "Card", Amount = 800.99, Status = "Paid" });
            _eCommerceContext.Payments.Add(new Payment { PaymentId = 7, OrderId = 7, PaymentDate = new DateTime(2020, 2, 2, 13, 45, 00), PaymentMethod = "Card", Amount = 200.99, Status = "Paid" });
            _eCommerceContext.Payments.Add(new Payment { PaymentId = 8, OrderId = 8, PaymentDate = new DateTime(2021, 12, 12, 12, 55, 00), PaymentMethod = "Card", Amount = 2200.99, Status = "Paid" });
            _eCommerceContext.Payments.Add(new Payment { PaymentId = 9, OrderId = 9, PaymentDate = new DateTime(2022, 12, 10, 10, 50, 00), PaymentMethod = "Card", Amount = 1100.99, Status = "Paid" });
            _eCommerceContext.Payments.Add(new Payment { PaymentId = 10, OrderId = 10, PaymentDate = new DateTime(2020, 3, 22, 16, 10, 20), PaymentMethod = "Card", Amount = 2700.99, Status = "Paid" });

            _eCommerceContext.Reviews.Add(new Review() { ReviewId = 1, ProductId = 1, EfUserId = 21, Rating = 4, Title = "Great Product", Comment = "I liked the product", ReviewDate = new DateTime(2023, 12, 12, 12, 55, 00) });
            _eCommerceContext.Reviews.Add(new Review() { ReviewId = 2, ProductId = 2, EfUserId = 22, Rating = 4, Title = "Great Product", Comment = "I liked the product", ReviewDate = new DateTime(2017, 5, 22, 15, 45, 10) });
            _eCommerceContext.Reviews.Add(new Review() { ReviewId = 3, ProductId = 3, EfUserId = 23, Rating = 4, Title = "Great Product", Comment = "I liked the product", ReviewDate = new DateTime(2019, 12, 12, 12, 55, 00) });
            _eCommerceContext.Reviews.Add(new Review() { ReviewId = 4, ProductId = 4, EfUserId = 24, Rating = 4, Title = "Great Product", Comment = "I liked the product", ReviewDate = new DateTime(2018, 6, 12, 12, 55, 00) });
            _eCommerceContext.Reviews.Add(new Review() { ReviewId = 5, ProductId = 5, EfUserId = 25, Rating = 5, Title = "Excellent Product", Comment = "Excellent quality", ReviewDate = new DateTime(2018, 5, 12, 21, 57, 50) });
            _eCommerceContext.Reviews.Add(new Review() { ReviewId = 6, ProductId = 6, EfUserId = 26, Rating = 4, Title = "Great Product", Comment = "I liked the product", ReviewDate = new DateTime(2019, 11, 13, 8, 6, 40) });
            _eCommerceContext.Reviews.Add(new Review() { ReviewId = 7, ProductId = 7, EfUserId = 27, Rating = 4, Title = "Great Product", Comment = "I liked the product", ReviewDate = new DateTime(2021, 3, 2, 13, 45, 00) });
            _eCommerceContext.Reviews.Add(new Review() { ReviewId = 8, ProductId = 8, EfUserId = 28, Rating = 4, Title = "Great Product", Comment = "I liked the product", ReviewDate = new DateTime(2022, 1, 12, 12, 55, 00) });
            _eCommerceContext.Reviews.Add(new Review() { ReviewId = 9, ProductId = 9, EfUserId = 29, Rating = 4, Title = "Great Product", Comment = "I liked the product", ReviewDate = new DateTime(2023, 12, 10, 10, 50, 00) });
            _eCommerceContext.Reviews.Add(new Review() { ReviewId = 10, ProductId = 10, EfUserId = 30, Rating = 3, Title = "Not a bad Product", Comment = "It was okay", ReviewDate = new DateTime(2021, 4, 22, 16, 10, 20) });

            _eCommerceContext.Wishlists.Add(new Wishlist() { WishlistId = 1, EfUserId = 21, ProductId = 1, AddedDate = new DateTime(2020, 12, 14, 12, 55, 00) });
            _eCommerceContext.Wishlists.Add(new Wishlist() { WishlistId = 2, EfUserId = 22, ProductId = 2, AddedDate = new DateTime(2023, 1, 14, 12, 55, 00) });
            _eCommerceContext.Wishlists.Add(new Wishlist() { WishlistId = 3, EfUserId = 23, ProductId = 3, AddedDate = new DateTime(2023, 12, 15, 12, 55, 00) });
            _eCommerceContext.Wishlists.Add(new Wishlist() { WishlistId = 4, EfUserId = 24, ProductId = 4, AddedDate = new DateTime(2023, 3, 17, 12, 55, 00) });
            _eCommerceContext.Wishlists.Add(new Wishlist() { WishlistId = 5, EfUserId = 25, ProductId = 5, AddedDate = new DateTime(2023, 8, 18, 12, 55, 00) });
            _eCommerceContext.Wishlists.Add(new Wishlist() { WishlistId = 6, EfUserId = 26, ProductId = 6, AddedDate = new DateTime(2023, 7, 12, 12, 55, 00) });
            _eCommerceContext.Wishlists.Add(new Wishlist() { WishlistId = 7, EfUserId = 27, ProductId = 7, AddedDate = new DateTime(2023, 7, 1, 12, 55, 00) });
            _eCommerceContext.Wishlists.Add(new Wishlist() { WishlistId = 8, EfUserId = 28, ProductId = 8, AddedDate = new DateTime(2023, 2, 2, 12, 55, 00) });
            _eCommerceContext.Wishlists.Add(new Wishlist() { WishlistId = 9, EfUserId = 29, ProductId = 9, AddedDate = new DateTime(2023, 6, 14, 12, 55, 00) });
            _eCommerceContext.Wishlists.Add(new Wishlist() { WishlistId = 10, EfUserId = 30, ProductId = 10, AddedDate = new DateTime(2023, 5, 13, 12, 55, 00) });

            _eCommerceContext.SaveChanges();
        }

        public static Object GeneratedAuthDB()
        {

            var _contextOptions = new DbContextOptionsBuilder<AuthenticationContext>()
                .UseInMemoryDatabase("ControllerTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var context = new AuthenticationContext(_contextOptions);

            // Seed data
            SeedData(context);

            return context;
        }

        private static void SeedData(AuthenticationContext context)
        {
            // Add users
            var roleId_1 = Guid.NewGuid().ToString();
            var userId_1 = Guid.NewGuid().ToString();

            var roleId_2 = Guid.NewGuid().ToString();
            var userId_2 = Guid.NewGuid().ToString();

            var roleId_3 = Guid.NewGuid().ToString();
            var userId_3 = Guid.NewGuid().ToString();
            context.Roles.Add(
               new IdentityRole{ Id = roleId_1, Name = "Administrator", NormalizedName = "ADMINISTRATOR" });
            context.Roles.Add(
             new IdentityRole { Id = roleId_2, Name = "Seller", NormalizedName = "SELLER" });
            context.Roles.Add(
            new IdentityRole { Id = roleId_3, Name = "Customer", NormalizedName = "CUSTOMER" }
               );




            //create Administrator user
            var AdminUser = new ApplicationUser
            {
                Id = userId_1,
                Email = "Zzimela@gmail.com",
                EmailConfirmed = true,
                Address = "18 Jack avenue, 2001",
                PhoneNumber = "0743244345",
                FirstName = "Zandile",
                LastName = "Zimela",
                UserName = "ZzimelaAdmin",
                NormalizedUserName = "ZZIMELAADMIN"
            };

            //set user password
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            AdminUser.PasswordHash = ph.HashPassword(AdminUser, "zimelaZ@123");

            //seed user
            context.ApplicationUsers.Add(AdminUser);

            //set user role to Administrator
            context.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = roleId_1,
                UserId = userId_1
            });

            //create Seller user
            var SellerUser = new ApplicationUser
            {
                Id = userId_2,
                Email = "cfarquarson0@cam.ac.uk",
                EmailConfirmed = true,
                Address = "25629 Fulton Pass",
                PhoneNumber = "0546292527",
                FirstName = "Corliss",
                LastName = "Farquarson",
                UserName = "cfarquarson0",
                NormalizedUserName = "CFARQUARSON0"
            };

            //set user password
            PasswordHasher<ApplicationUser> cstph = new PasswordHasher<ApplicationUser>();
            SellerUser.PasswordHash = cstph.HashPassword(SellerUser, "corlissF@123");

            //seed user
            context.ApplicationUsers.Add(SellerUser);

            //set user role to admin
            context.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = roleId_2,
                UserId = userId_2
            });

            //create Customer user
            var CustomerUser = new ApplicationUser
            {
                Id = userId_3,
                Email = "Efronz@gmail.com",
                EmailConfirmed = true,
                Address = "15 Zuma avenue, 2001",
                PhoneNumber = "074396748",
                FirstName = "Zack",
                LastName = "Efron",
                UserName = "Efronz",
                NormalizedUserName = "EFRONZ"
            };

            //set user password
            PasswordHasher<ApplicationUser> lgcph = new PasswordHasher<ApplicationUser>();
            CustomerUser.PasswordHash = lgcph.HashPassword(CustomerUser, "efron@123");

            //seed user
            context.ApplicationUsers.Add(CustomerUser);

            //set user role to admin
            context.UserRoles.Add(new IdentityUserRole<string>
            {
                RoleId = roleId_3,
                UserId = userId_3
            });

            context.SaveChanges();
        }
    }
}

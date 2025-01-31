using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.ViewModels;
using static NuGet.Packaging.PackagingConstants;

namespace LLM_eCommerce_RESTAPI.Repository
{
    /// <summary>
    /// A summary about CustomersOrderDetailsRepo class.
    /// </summary>
    /// <remarks>
    /// CustomersOrderDetailsRepo has the following methods:
    /// Get current logged in user's products
    /// Get orderDetails with order details id
    /// Get orderDetails with order id
    /// </remarks>
    public class CustomersOrderDetailsRepo
    {

        private readonly LLM_eCommerce_EFDBContext _context;

        public CustomersOrderDetailsRepo(LLM_eCommerce_EFDBContext context)
        {
            _context = context;
        }

        public virtual List<CustomersOrderDetailsVM> GetCustomerOrderDetails(int userId)
        {
            int paramId = userId;
            List<CustomersOrderDetailsVM> customersOrderDetails = new List<CustomersOrderDetailsVM>();

            var customersOrderDetailsQuery =
                (from efUsers in _context.EfUsers
                 join orders in _context.Orders
                 on efUsers.EfUserId equals orders.EfUserId
                 join orderDetails in _context.OrderDetails
                 on orders.OrderId equals orderDetails.OrderId
                 join products in _context.Products
                 on orderDetails.ProductId equals products.ProductId
                 join categories in _context.Categories
                 on products.CategoryId equals categories.CategoryId
                 join shippings in _context.Shippings
                 on orders.ShippingId equals shippings.ShippingId
                 where ((paramId == 0 && efUsers.EfUserId == efUsers.EfUserId) || (efUsers.EfUserId == paramId))
                 orderby orders.EfUserId, orders.OrderDate
                 select new
                 {
                     EfUserId = efUsers.EfUserId,
                     FirstName = efUsers.FirstName,
                     LastName = efUsers.LastName,
                     Email = efUsers.Email,
                     Address = efUsers.Address,
                     PhoneNumber = efUsers.PhoneNumber,
                     IdentityUsername = efUsers.IdentityUsername,
                     Role = efUsers.Role,
                     OrderId = orders.OrderId,
                     ShippingId = shippings.ShippingId,
                     ShippingDate = shippings.ShippingDate,
                     ShippingAddress = shippings.ShippingAddress,
                     ShippingMethod = shippings.ShippingMethod,
                     TrackingNumber = shippings.TrackingNumber,
                     DeliveryStatus = shippings.DeliveryStatus,
                     OrderDate = orders.OrderDate,
                     TotalAmount = orders.TotalAmount,
                     OrderDetailId = orderDetails.OrderDetailId,
                     ProductId = products.ProductId,
                     Name = products.Name,
                     Brand = products.Brand,
                     Description = products.Description,
                     Type = products.Type,
                     Price = products.Price,
                     CategoryId = categories.CategoryId,
                     CategoryName = categories.Name,
                     StockQuantity = products.StockQuantity,
                     ModifiedDate = products.ModifiedDate,
                     Quantity = orderDetails.Quantity,
                     UnitPrice = orderDetails.UnitPrice
                 }).ToList();

            foreach (var cust in customersOrderDetailsQuery)
            {
                customersOrderDetails.Add(new CustomersOrderDetailsVM()
                {
                    EfUserId = cust.EfUserId,
                    FirstName = cust.FirstName,
                    LastName = cust.LastName,
                    Email = cust.Email,
                    Address = cust.Address,
                    PhoneNumber = cust.PhoneNumber,
                    IdentityUsername = cust.IdentityUsername,
                    Role = cust.Role,
                    OrderId = cust.OrderId,
                    ShippingId = cust.ShippingId,
                    ShippingDate = cust.ShippingDate,
                    ShippingAddress = cust.ShippingAddress,
                    ShippingMethod = cust.ShippingMethod,
                    TrackingNumber = cust.TrackingNumber,
                    DeliveryStatus = cust.DeliveryStatus,
                    OrderDate = cust.OrderDate,
                    TotalAmount = cust.TotalAmount,
                    OrderDetailId = cust.OrderDetailId,
                    ProductId = cust.ProductId,
                    Name = cust.Name,
                    Brand = cust.Brand,
                    Description = cust.Description,
                    Type = cust.Type,
                    Price = cust.Price,
                    CategoryId = cust.CategoryId,
                    CategoryName = cust.CategoryName,
                    StockQuantity = cust.StockQuantity,
                    ModifiedDate = cust.ModifiedDate,
                    Quantity = cust.Quantity,
                    UnitPrice = cust.UnitPrice
                });
            }

            return customersOrderDetails;
        }



        public virtual CustomersOrderDetailsVM GetOrderDetails(int orderDetailsId)
        {
            int paramId = orderDetailsId;
            CustomersOrderDetailsVM customersOrderDetails = new CustomersOrderDetailsVM();

            var customersOrderDetailsQuery =
                (from efUsers in _context.EfUsers
                 join orders in _context.Orders
                 on efUsers.EfUserId equals orders.EfUserId
                 join orderDetails in _context.OrderDetails
                 on orders.OrderId equals orderDetails.OrderId
                 join products in _context.Products
                 on orderDetails.ProductId equals products.ProductId
                 join categories in _context.Categories
                 on products.CategoryId equals categories.CategoryId
                 join shippings in _context.Shippings
                 on orders.ShippingId equals shippings.ShippingId
                 where (orderDetails.OrderDetailId == paramId)
                 orderby orders.EfUserId, orders.OrderDate
                 select new
                 {
                     EfUserId = efUsers.EfUserId,
                     FirstName = efUsers.FirstName,
                     LastName = efUsers.LastName,
                     Email = efUsers.Email,
                     Address = efUsers.Address,
                     PhoneNumber = efUsers.PhoneNumber,
                     IdentityUsername = efUsers.IdentityUsername,
                     Role = efUsers.Role,
                     OrderId = orders.OrderId,
                     ShippingId = shippings.ShippingId,
                     ShippingDate = shippings.ShippingDate,
                     ShippingAddress = shippings.ShippingAddress,
                     ShippingMethod = shippings.ShippingMethod,
                     TrackingNumber = shippings.TrackingNumber,
                     DeliveryStatus = shippings.DeliveryStatus,
                     OrderDate = orders.OrderDate,
                     TotalAmount = orders.TotalAmount,
                     OrderDetailId = orderDetails.OrderDetailId,
                     ProductId = products.ProductId,
                     Name = products.Name,
                     Brand = products.Brand,
                     Description = products.Description,
                     Type = products.Type,
                     Price = products.Price,
                     CategoryId = categories.CategoryId,
                     CategoryName = categories.Name,
                     StockQuantity = products.StockQuantity,
                     ModifiedDate = products.ModifiedDate,
                     Quantity = orderDetails.Quantity,
                     UnitPrice = orderDetails.UnitPrice
                 }).ToList();

            foreach (var cust in customersOrderDetailsQuery)
            {
                customersOrderDetails.EfUserId = cust.EfUserId;
                customersOrderDetails.FirstName = cust.FirstName;
                customersOrderDetails.LastName = cust.LastName;
                customersOrderDetails.Email = cust.Email;
                customersOrderDetails.Address = cust.Address;
                customersOrderDetails.PhoneNumber = cust.PhoneNumber;
                customersOrderDetails.IdentityUsername = cust.IdentityUsername;
                customersOrderDetails.Role = cust.Role;
                customersOrderDetails.OrderId = cust.OrderId;
                customersOrderDetails.ShippingId = cust.ShippingId;
                customersOrderDetails.ShippingDate = cust.ShippingDate;
                customersOrderDetails.ShippingAddress = cust.ShippingAddress;
                customersOrderDetails.ShippingMethod = cust.ShippingMethod;
                customersOrderDetails.TrackingNumber = cust.TrackingNumber;
                customersOrderDetails.DeliveryStatus = cust.DeliveryStatus;
                customersOrderDetails.OrderDate = cust.OrderDate;
                customersOrderDetails.TotalAmount = cust.TotalAmount;
                customersOrderDetails.OrderDetailId = cust.OrderDetailId;
                customersOrderDetails.ProductId = cust.ProductId;
                customersOrderDetails.Name = cust.Name;
                customersOrderDetails.Brand = cust.Brand;
                customersOrderDetails.Description = cust.Description;
                customersOrderDetails.Type = cust.Type;
                customersOrderDetails.Price = cust.Price;
                customersOrderDetails.CategoryId = cust.CategoryId;
                customersOrderDetails.CategoryName = cust.CategoryName;
                customersOrderDetails.StockQuantity = cust.StockQuantity;
                customersOrderDetails.ModifiedDate = cust.ModifiedDate;
                customersOrderDetails.Quantity = cust.Quantity;
                customersOrderDetails.UnitPrice = cust.UnitPrice;
            }

            return customersOrderDetails;
        }


        public virtual List<CustomersOrderDetailsVM> GetOrderDetailsByOrderId(int orderId)
        {
            int paramId = orderId;
            List<CustomersOrderDetailsVM> customersOrderDetails = new List<CustomersOrderDetailsVM>();

            var customersOrderDetailsQuery =
                (from efUsers in _context.EfUsers
                 join orders in _context.Orders
                 on efUsers.EfUserId equals orders.EfUserId
                 join orderDetails in _context.OrderDetails
                 on orders.OrderId equals orderDetails.OrderId
                 join products in _context.Products
                 on orderDetails.ProductId equals products.ProductId
                 join categories in _context.Categories
                 on products.CategoryId equals categories.CategoryId
                 join shippings in _context.Shippings
                 on orders.ShippingId equals shippings.ShippingId
                 where (orders.OrderId == paramId)
                 orderby orders.OrderDate, orders.TotalAmount descending
                 select new
                 {
                     EfUserId = efUsers.EfUserId,
                     FirstName = efUsers.FirstName,
                     LastName = efUsers.LastName,
                     Email = efUsers.Email,
                     Address = efUsers.Address,
                     PhoneNumber = efUsers.PhoneNumber,
                     IdentityUsername = efUsers.IdentityUsername,
                     Role = efUsers.Role,
                     OrderId = orders.OrderId,
                     ShippingId = shippings.ShippingId,
                     ShippingDate = shippings.ShippingDate,
                     ShippingAddress = shippings.ShippingAddress,
                     ShippingMethod = shippings.ShippingMethod,
                     TrackingNumber = shippings.TrackingNumber,
                     DeliveryStatus = shippings.DeliveryStatus,
                     OrderDate = orders.OrderDate,
                     TotalAmount = orders.TotalAmount,
                     OrderDetailId = orderDetails.OrderDetailId,
                     ProductId = products.ProductId,
                     Name = products.Name,
                     Brand = products.Brand,
                     Description = products.Description,
                     Type = products.Type,
                     Price = products.Price,
                     CategoryId = categories.CategoryId,
                     CategoryName = categories.Name,
                     StockQuantity = products.StockQuantity,
                     ModifiedDate = products.ModifiedDate,
                     Quantity = orderDetails.Quantity,
                     UnitPrice = orderDetails.UnitPrice
                 }).ToList();

            foreach (var cust in customersOrderDetailsQuery)
            {
                customersOrderDetails.Add(new CustomersOrderDetailsVM()
                {
                    EfUserId = cust.EfUserId,
                    FirstName = cust.FirstName,
                    LastName = cust.LastName,
                    Email = cust.Email,
                    Address = cust.Address,
                    PhoneNumber = cust.PhoneNumber,
                    IdentityUsername = cust.IdentityUsername,
                    Role = cust.Role,
                    OrderId = cust.OrderId,
                    ShippingId = cust.ShippingId,
                    ShippingDate = cust.ShippingDate,
                    ShippingAddress = cust.ShippingAddress,
                    ShippingMethod = cust.ShippingMethod,
                    TrackingNumber = cust.TrackingNumber,
                    DeliveryStatus = cust.DeliveryStatus,
                    OrderDate = cust.OrderDate,
                    TotalAmount = cust.TotalAmount,
                    OrderDetailId = cust.OrderDetailId,
                    ProductId = cust.ProductId,
                    Name = cust.Name,
                    Brand = cust.Brand,
                    Description = cust.Description,
                    Type = cust.Type,
                    Price = cust.Price,
                    CategoryId = cust.CategoryId,
                    CategoryName = cust.CategoryName,
                    StockQuantity = cust.StockQuantity,
                    ModifiedDate = cust.ModifiedDate,
                    Quantity = cust.Quantity,
                    UnitPrice = cust.UnitPrice
                });
            }

            return customersOrderDetails;
        }
    }
}

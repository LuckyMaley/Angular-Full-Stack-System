using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.ViewModels;

namespace LLM_eCommerce_RESTAPI.Repository
{
    /// <summary>
    /// A summary about CustomersOrdersRepo class.
    /// </summary>
    /// <remarks>
    /// CustomersOrdersRepo has the following methods:
    /// Get current logged in user's orders
    /// Get orders with order id
    /// Get orders with shipping id
    /// </remarks>
    public class CustomersOrdersRepo
    {
        private readonly LLM_eCommerce_EFDBContext _context;

        public CustomersOrdersRepo(LLM_eCommerce_EFDBContext context)
        {
            _context = context;
        }

        public virtual List<CustomersOrdersVM> GetCustomerOrders(int userId)
        {
            int paramId = userId;
            List<CustomersOrdersVM> customersOrders = new List<CustomersOrdersVM>();

            var customersOrdersQuery =
                (from efUsers in _context.EfUsers
                 join orders in _context.Orders
                 on efUsers.EfUserId equals orders.EfUserId
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
                     TotalAmount = orders.TotalAmount
                 }).ToList();

            foreach (var cust in customersOrdersQuery)
            {
                customersOrders.Add(new CustomersOrdersVM()
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
                    TotalAmount = cust.TotalAmount
                });
            }

            return customersOrders;
        }



        public virtual CustomersOrdersVM GetOrderDetails(int orderId)
        {
            int paramId = orderId;
            CustomersOrdersVM customersOrders = new CustomersOrdersVM();

            var customersOrdersQuery =
               (from efUsers in _context.EfUsers
                join orders in _context.Orders
                on efUsers.EfUserId equals orders.EfUserId
                join shippings in _context.Shippings
                on orders.ShippingId equals shippings.ShippingId
                where (orders.OrderId == paramId)
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
                     TotalAmount = orders.TotalAmount
                 }).ToList();

            foreach (var cust in customersOrdersQuery)
            {
                customersOrders.EfUserId = cust.EfUserId;
                customersOrders.FirstName = cust.FirstName;
                customersOrders.LastName = cust.LastName;
                customersOrders.Email = cust.Email;
                customersOrders.Address = cust.Address;
                customersOrders.PhoneNumber = cust.PhoneNumber;
                customersOrders.IdentityUsername = cust.IdentityUsername;
                customersOrders.Role = cust.Role;
                customersOrders.OrderId = cust.OrderId;
                customersOrders.ShippingId = cust.ShippingId;
                customersOrders.ShippingDate = cust.ShippingDate;
                customersOrders.ShippingAddress = cust.ShippingAddress;
                customersOrders.ShippingMethod = cust.ShippingMethod;
                customersOrders.TrackingNumber = cust.TrackingNumber;
                customersOrders.DeliveryStatus = cust.DeliveryStatus;
                customersOrders.OrderDate = cust.OrderDate;
                customersOrders.TotalAmount = cust.TotalAmount;
            }

            return customersOrders;
        }


        public virtual List<CustomersOrdersVM> GetProductsByShippingId(int shippingId)
        {
            int paramId = shippingId;
            List<CustomersOrdersVM> customersOrders = new List<CustomersOrdersVM>();

            var customersOrdersQuery =
                (from efUsers in _context.EfUsers
                 join orders in _context.Orders
                 on efUsers.EfUserId equals orders.EfUserId
                 join shippings in _context.Shippings
                 on orders.ShippingId equals shippings.ShippingId
                 where (orders.ShippingId == paramId)
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
                     TotalAmount = orders.TotalAmount
                 }).ToList();

            foreach (var cust in customersOrdersQuery)
            {
                customersOrders.Add(new CustomersOrdersVM()
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
                    TotalAmount = cust.TotalAmount
                });
            }

            return customersOrders;
        }
    }
}

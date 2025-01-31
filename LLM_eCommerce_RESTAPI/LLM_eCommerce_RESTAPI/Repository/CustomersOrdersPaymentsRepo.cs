using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.ViewModels;

namespace LLM_eCommerce_RESTAPI.Repository
{
    /// <summary>
    /// A summary about OrdersPaymentsRepo class.
    /// </summary>
    /// <remarks>
    /// OrdersPaymentsRepo has the following methods:
    /// Get current logged in user's order payments
    /// Get orders with payment id
    /// Get orders with order id
    /// </remarks>
    public class CustomersOrdersPaymentsRepo
    {
        private readonly LLM_eCommerce_EFDBContext _context;

        public CustomersOrdersPaymentsRepo(LLM_eCommerce_EFDBContext context)
        {
            _context = context;
        }

        public virtual List<CustomersOrdersPaymentsVM> GetOrdersPayments(int userId)
        {
            int paramId = userId;
            List<CustomersOrdersPaymentsVM> ordersPayments = new List<CustomersOrdersPaymentsVM>();

            var ordersPaymentsQuery =
                (from efUsers in _context.EfUsers
                 join orders in _context.Orders
                 on efUsers.EfUserId equals orders.EfUserId
                 join shippings in _context.Shippings
                 on orders.ShippingId equals shippings.ShippingId
                 join payments in _context.Payments
                 on orders.OrderId equals payments.OrderId
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
                     PaymentId = payments.PaymentId,
                     PaymentMethod = payments.PaymentMethod,
                     PaymentDate = payments.PaymentDate,
                     PaymentAmount = payments.Amount,
                     PaymentStatus = payments.Status,
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

            foreach (var cust in ordersPaymentsQuery)
            {
                ordersPayments.Add(new CustomersOrdersPaymentsVM()
                {
                    EfUserId = cust.EfUserId,
                    FirstName = cust.FirstName,
                    LastName = cust.LastName,
                    Email = cust.Email,
                    Address = cust.Address,
                    PhoneNumber = cust.PhoneNumber,
                    IdentityUsername = cust.IdentityUsername,
                    Role = cust.Role,
                    PaymentId = cust.PaymentId,
                    PaymentMethod = cust.PaymentMethod,
                    PaymentDate = cust.PaymentDate,
                    PaymentAmount = cust.PaymentAmount,
                    PaymentStatus = cust.PaymentStatus,
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

            return ordersPayments;
        }



        public virtual CustomersOrdersPaymentsVM GetPaymentDetails(int paymentId)
        {
            int paramId = paymentId;
            CustomersOrdersPaymentsVM ordersPayments = new CustomersOrdersPaymentsVM();

            var ordersPaymentsQuery =
               (from efUsers in _context.EfUsers
                join orders in _context.Orders
                on efUsers.EfUserId equals orders.EfUserId
                join shippings in _context.Shippings
                on orders.ShippingId equals shippings.ShippingId
                join payments in _context.Payments
                on orders.OrderId equals payments.OrderId
                where (payments.PaymentId == paramId)
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
                    PaymentId = payments.PaymentId,
                    PaymentMethod = payments.PaymentMethod,
                    PaymentDate = payments.PaymentDate,
                    PaymentAmount = payments.Amount,
                    PaymentStatus = payments.Status,
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

            foreach (var cust in ordersPaymentsQuery)
            {
                ordersPayments.EfUserId = cust.EfUserId;
                ordersPayments.FirstName = cust.FirstName;
                ordersPayments.LastName = cust.LastName;
                ordersPayments.Email = cust.Email;
                ordersPayments.Address = cust.Address;
                ordersPayments.PhoneNumber = cust.PhoneNumber;
                ordersPayments.IdentityUsername = cust.IdentityUsername;
                ordersPayments.Role = cust.Role;
                ordersPayments.OrderId = cust.OrderId;
                ordersPayments.ShippingId = cust.ShippingId;
                ordersPayments.ShippingDate = cust.ShippingDate;
                ordersPayments.ShippingAddress = cust.ShippingAddress;
                ordersPayments.ShippingMethod = cust.ShippingMethod;
                ordersPayments.TrackingNumber = cust.TrackingNumber;
                ordersPayments.DeliveryStatus = cust.DeliveryStatus;
                ordersPayments.OrderDate = cust.OrderDate;
                ordersPayments.TotalAmount = cust.TotalAmount;
            }

            return ordersPayments;
        }


        public virtual List<CustomersOrdersPaymentsVM> GetPaymentsByOrderId(int orderId)
        {
            int paramId = orderId;
            List<CustomersOrdersPaymentsVM> ordersPayments = new List<CustomersOrdersPaymentsVM>();

            var ordersPaymentsQuery =
                (from efUsers in _context.EfUsers
                 join orders in _context.Orders
                 on efUsers.EfUserId equals orders.EfUserId
                 join shippings in _context.Shippings
                 on orders.ShippingId equals shippings.ShippingId
                 join payments in _context.Payments
                 on orders.OrderId equals payments.OrderId
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
                     PaymentId = payments.PaymentId,
                     PaymentMethod = payments.PaymentMethod,
                     PaymentDate = payments.PaymentDate,
                     PaymentAmount = payments.Amount,
                     PaymentStatus = payments.Status,
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

            foreach (var cust in ordersPaymentsQuery)
            {
                ordersPayments.Add(new CustomersOrdersPaymentsVM()
                {
                    EfUserId = cust.EfUserId,
                    FirstName = cust.FirstName,
                    LastName = cust.LastName,
                    Email = cust.Email,
                    Address = cust.Address,
                    PhoneNumber = cust.PhoneNumber,
                    IdentityUsername = cust.IdentityUsername,
                    Role = cust.Role,
                    PaymentId = cust.PaymentId,
                    PaymentMethod = cust.PaymentMethod,
                    PaymentDate = cust.PaymentDate,
                    PaymentAmount = cust.PaymentAmount,
                    PaymentStatus = cust.PaymentStatus,
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

            return ordersPayments;
        }
    }
}

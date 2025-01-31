using LLM_eCommerce_RESTAPI.AuthModels;
using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.Services;
using LLM_eCommerce_RESTAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace LLM_eCommerce_RESTAPI.Controllers
{
    /// <summary>
    /// A summary about OrdersController class.
    /// </summary>
    /// <remarks>
    /// OrdersController has the following end points:
    /// Get all Orders
    /// Get Orders with id
    /// Get Orders with date
    /// Get Orders between dates
    /// Put (update) Order with id and Order object
    /// Post (Add) Order using a Orders View Model 
    /// Delete Order with id
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly LLM_eCommerce_EFDBContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthenticationContext _authenticationContext;
        private readonly IdentityHelper _identityHelper;
        public OrdersController(LLM_eCommerce_EFDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AuthenticationContext authenticationContext)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationContext = authenticationContext;
            _identityHelper = new IdentityHelper(_userManager, _authenticationContext, _roleManager);
        }


        // GET: api/Orders        
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {

            var orderDB = await _context.Orders.ToListAsync();

            return Ok(orderDB);
        }

        // GET: api/Orders/5
        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrders(int id)
        {
            List<Order> allOrders = new List<Order>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orders = await _context.Orders.FindAsync(id);

            if (orders == null)
            {
                return NotFound(new { message = "No Order with that ID exists, please try again" });
            }
            else
            {
                orders.Payments = GetAllPaymentsByOrderId(id);
                orders.OrderDetails = GetAllOrderDetailsByOrderId(id);
            }

            return Ok(orders);
        }


        // GET: api/Orders/SpecificDate/date
        [EnableCors("AllowOrigin")]
        [HttpGet("SpecificDateASyyyy-mm-dd/{date}")]
        public async Task<ActionResult<List<Order>>> GetOrderByDate(DateTime date)
        {
            List<Order> orders = new List<Order>();
            DateTime dateOutput;
            bool valid = DateTime.TryParse(date.ToShortDateString(), CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out dateOutput);
            if (!valid)
            {
                return BadRequest("Error the format of the date is incorrect");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Order> temOrders = _context.Orders.ToList();
            var ordersQuery = temOrders.Where(x => x.OrderDate.Date == dateOutput.Date);
            if (ordersQuery.Count() == 0)
            {
                return NotFound(new { message = "No Order with that date exists, please try again" });
            }
            var item = ordersQuery;
            foreach (var orderItem in item)
            {
                int id = orderItem.OrderId;


                if (orderItem == null)
                {

                    return NotFound(new { message = "No Order with that date exists, please try again" });
                }
                else
                {
                    orderItem.Payments = GetAllPaymentsByOrderId(id);
                    orderItem.OrderDetails = GetAllOrderDetailsByOrderId(id);
                }

                orders.Add(orderItem);
            }
            return Ok(orders);
        }

        // GET: api/Orders/BetweenDates/date1/date2
        [EnableCors("AllowOrigin")]
        [HttpGet("BetweenDatesBothASyyyy-mm-dd/{{date1}}/{{date2}}")]
        public async Task<ActionResult<List<Order>>> GetOrderByBetweenDates(DateTime date1, DateTime date2)
        {
            List<Order> orders = new List<Order>();
            DateTime date1Output;
            bool valid = DateTime.TryParse(date1.ToShortDateString(), CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out date1Output);
            if (!valid)
            {
                return BadRequest("Error the format of the date is incorrect");
            }

            DateTime date2Output;
            bool validDate2 = DateTime.TryParse(date2.ToShortDateString(), CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out date2Output);
            if (!validDate2)
            {
                return BadRequest("Error the format of the date is incorrect");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<Order> temOrders = _context.Orders.ToList();
            var ordersQuery = temOrders.Where(x => x.OrderDate.Date >= date1Output.Date && x.OrderDate <= date2Output);
            if (ordersQuery.Count() == 0)
            {
                return NotFound(new { message = "No Order with that date range exists, please try again" });
            }
            var item = ordersQuery;
            foreach (var orderItem in item)
            {
                int id = orderItem.OrderId;


                if (orderItem == null)
                {

                    return NotFound(new { message = "No Order with that date exists, please try again" });
                }
                else
                {
                    orderItem.Payments = GetAllPaymentsByOrderId(id);
                    orderItem.OrderDetails = GetAllOrderDetailsByOrderId(id);
                }

                orders.Add(orderItem);
            }
            return Ok(orders);
        }

        // PUT: api/Orders/5
        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutOrders(int id, OrdersVM order)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            if (!userSuperUserAuthorised)
            {
                    return BadRequest(new { message = "Not authorised to update orders" });
            }


            int currentOrderId = 0;

            try
            {
                Order updateOrder = _context.Orders.FirstOrDefault(o => o.OrderId == id);
                int count = 0;
                if (updateOrder == null)
                {
                    return NotFound(new { message = "No Order with that ID exists, please try again" });
                }
                if (order.TotalAmount != 0)
                {
                    if (updateOrder.TotalAmount != order.TotalAmount)
                    {
                        updateOrder.TotalAmount = order.TotalAmount;
                        count++;
                    }
                }

                var updateShipping = _context.Shippings.FirstOrDefault(c => c.ShippingId == updateOrder.ShippingId);
                if (order.ShippingAddress != "" || order.ShippingAddress != null)
                {
                    if (order.ShippingAddress != updateShipping.ShippingAddress)
                    {
                        updateShipping.ShippingAddress = order.ShippingAddress;
                        count++;
                    }
                }

                if (order.ShippingMethod != "" || order.ShippingMethod != null)
                {
                    if (updateShipping.ShippingMethod != order.ShippingMethod)
                    {
                        updateShipping.ShippingMethod = order.ShippingMethod;
                        count++;
                    }
                }

                if (count > 0)
                {
                    await _context.SaveChangesAsync();
                    currentOrderId = id;
                }
                else
                {
                    return Ok(new { message= "no updates made"});
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersExists(id))
                {
                    return NotFound(new { message = "Order Id not found, no changes made, please try again" });
                }
                else
                {
                    throw;
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error, " + e.Message });
            }

            return Ok(new { message = "Order Updated - OrderId:" + currentOrderId });
        }

        // POST: api/Orders
        [EnableCors("AllowOrigin")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> PostOrders(OrdersVM order)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
            if (userSuperUserAuthorised)
            {
                
                return BadRequest(new { message = "Not authorised to add orders - Only Customers are allowed" });
            }

            if (userSellerAuthorised)
            {
                return BadRequest(new { message = "Not authorised to add orders - Only Customers are allowed" });
            }

            if (order.TotalAmount == 0)
            {
                return BadRequest(new { message = "Cannot Add an empty order, please you enter a valid order" });
            }

            int currentOrderId = 0;

            try
            {
                Shipping newShipping = new Shipping()
                {
                    ShippingAddress = order.ShippingAddress,
                    ShippingMethod = order.ShippingMethod,
                    ShippingDate = DateTime.Now,
                    DeliveryStatus = "Pending",
                    TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8)
                };
                 _context.Shippings.Add(newShipping);
                await _context.SaveChangesAsync();
                Order newOrder = new Order();
                newOrder.TotalAmount = order.TotalAmount;
                newOrder.OrderDate = DateTime.Now;
                newOrder.EfUserId = _context.EfUsers.FirstOrDefault(c => c.IdentityUsername == user.UserName).EfUserId;
                newOrder.ShippingId = _context.Shippings.Max( c => c.ShippingId);
                newOrder.EfUser = _context.EfUsers.FirstOrDefault(c => c.IdentityUsername == user.UserName);
                newOrder.Shipping = newShipping;
                

                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();
                newOrder.Payments = GetAllPaymentsByOrderId(newOrder.OrderId);
                newOrder.OrderDetails = GetAllOrderDetailsByOrderId(newOrder.OrderId);
                _context.SaveChanges();
                currentOrderId = newOrder.OrderId;
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in adding Order, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error in adding Order, " + e.Message });
            }

            return Ok(new { message = "New Order Created - OrderId:" + currentOrderId });
        }

		// POST: api/Orders/OrdersPay
		[EnableCors("AllowOrigin")]
		[Route("OrdersPay")]
        [HttpPost]
		[Authorize]
		public async Task<ActionResult<Order>> PostFullOrders(FullOrderVM order)
		{
			string userId = User.Claims.First(c => c.Type == "UserID").Value;
			var user = await _userManager.FindByIdAsync(userId);
			bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
			bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
			if (userSuperUserAuthorised)
			{

				return BadRequest(new { message = "Not authorised to add orders - Only Customers are allowed" });
			}

			if (userSellerAuthorised)
			{
				return BadRequest(new { message = "Not authorised to add orders - Only Customers are allowed" });
			}

			if (order.TotalAmount == 0)
			{
				return BadRequest(new { message = "Cannot Add an empty order, please you enter a valid order" });
			}

			int currentOrderId = 0;

			try
			{
				Shipping newShipping = new Shipping()
				{
					ShippingAddress = order.ShippingAddress,
					ShippingMethod = order.ShippingMethod,
					ShippingDate = DateTime.Now,
					DeliveryStatus = "Pending",
					TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8)
				};
				_context.Shippings.Add(newShipping);
				await _context.SaveChangesAsync();
				Order newOrder = new Order();
				newOrder.TotalAmount = order.TotalAmount;
				newOrder.OrderDate = DateTime.Now;
				newOrder.EfUserId = _context.EfUsers.FirstOrDefault(c => c.IdentityUsername == user.UserName).EfUserId;
				newOrder.ShippingId = _context.Shippings.Max(c => c.ShippingId);
				newOrder.EfUser = _context.EfUsers.FirstOrDefault(c => c.IdentityUsername == user.UserName);
				newOrder.Shipping = newShipping;
				_context.Orders.Add(newOrder);
				await _context.SaveChangesAsync();
				currentOrderId = newOrder.OrderId;
				List<OrderDetail> orderDetails = order.OrderDetails.Select(od => new OrderDetail
				{
					OrderId = currentOrderId,
					ProductId = od.ProductId,
					Quantity = od.Quantity,
					UnitPrice = od.UnitPrice
				}).ToList();

				_context.OrderDetails.AddRange(orderDetails);
				Payment newPayment = new Payment();
				newPayment.OrderId = currentOrderId;
				newPayment.PaymentMethod = order.PaymentMethod;
				newPayment.Status = order.Status;
				newPayment.Amount = order.TotalAmount;
                newPayment.PaymentDate = DateTime.Now;
                _context.Payments.Add(newPayment);
				await _context.SaveChangesAsync();
				newOrder.Payments = GetAllPaymentsByOrderId(newOrder.OrderId);
				newOrder.OrderDetails = GetAllOrderDetailsByOrderId(newOrder.OrderId);
				await _context.SaveChangesAsync();

			}
			catch (DbUpdateConcurrencyException)
			{
				return BadRequest(new { message = "Error in adding Order, please try again" });
			}
			catch (Exception e)
			{
				return BadRequest(new { message = "Error in adding Order, " + e.Message });
			}

			return Ok(currentOrderId);
		}

		// DELETE: api/Orders/5
		[EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Order>> DeleteOrders(int id)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
            if (!userSuperUserAuthorised)
            {
                    return BadRequest(new { message = "Not authorised to delete orders" });
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound(new { message = "Order ID not found, please try again" });
            }

            try
            {

                _context.Orders.Remove(orders);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in deleting Order, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error, " + e.Message });
            }
            return orders;
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }


        private List<OrderDetail> GetAllOrderDetailsByOrderId(int id)
        {
            List<OrderDetail> allOrderDetailsForOrder = new List<OrderDetail>();

            var orderDetailsQuery =
                    (from orderDetails in _context.OrderDetails
                     where (orderDetails.OrderId == id)
                     select new
                     {
                         orderDetails.OrderDetailId,
                         orderDetails.OrderId,
                         orderDetails.ProductId,
                         orderDetails.Quantity,
                         orderDetails.UnitPrice,
                         orderDetails.Order,
                         orderDetails.Product
                     }).ToList();


            foreach (var ordd in orderDetailsQuery)
            {
                allOrderDetailsForOrder.Add(new OrderDetail()
                {
                    OrderDetailId = ordd.OrderDetailId,
                    OrderId = ordd.OrderId,
                    ProductId = ordd.ProductId,
                    Quantity = ordd.Quantity,
                    UnitPrice = ordd.UnitPrice,
                    Order = ordd.Order,
                    Product = ordd.Product
                });
            }

            return allOrderDetailsForOrder;
        }

        private List<Payment> GetAllPaymentsByOrderId(int id)
        {
            List<Payment> allPaymentsForOrder = new List<Payment>();

            var paymentsQuery =
                    (from payments in _context.Payments
                     where (payments.OrderId == id)
                     select new
                     {
                         payments.PaymentId,
                         payments.OrderId,
                         payments.PaymentDate,
                         payments.Amount,
                         payments.PaymentMethod,
                         payments.Status,
                         payments.Order
                     }).ToList();


            foreach (var pay in paymentsQuery)
            {
                allPaymentsForOrder.Add(new Payment()
                {
                    PaymentId = pay.PaymentId,
                    OrderId = pay.OrderId,
                    PaymentDate = pay.PaymentDate,
                    Amount = pay.Amount,
                    PaymentMethod = pay.PaymentMethod,
                    Status = pay.Status,
                    Order = pay.Order
                });
            }

            return allPaymentsForOrder;
        }
    }
}

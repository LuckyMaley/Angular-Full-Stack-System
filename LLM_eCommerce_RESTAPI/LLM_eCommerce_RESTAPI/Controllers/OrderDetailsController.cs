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
using NuGet.Frameworks;
using System.Globalization;

namespace LLM_eCommerce_RESTAPI.Controllers
{
    /// <summary>
    /// A summary about OrderDetailsController class.
    /// </summary>
    /// <remarks>
    /// OrderDetailsController has the following end points:
    /// Get all OrderDetails
    /// Get OrderDetails with id
    /// Put (update) OrderDetail with id and OrderDetail object
    /// Post (Add) OrderDetail using a OrderDetails View Model 
    /// Delete OrderDetail with id
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly LLM_eCommerce_EFDBContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthenticationContext _authenticationContext;
        private readonly IdentityHelper _identityHelper;
        public OrderDetailsController(LLM_eCommerce_EFDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AuthenticationContext authenticationContext)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationContext = authenticationContext;
            _identityHelper = new IdentityHelper(_userManager, _authenticationContext, _roleManager);
        }


        // GET: api/OrderDetails        
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails()
        {

            var orderDetailDB = await _context.OrderDetails.ToListAsync();

            return Ok(orderDetailDB);
        }

        // GET: api/OrderDetails/5
        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetails(int id)
        {
            List<OrderDetail> allOrderDetails = new List<OrderDetail>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderDetails = await _context.OrderDetails.FindAsync(id);

            if (orderDetails == null)
            {
                return NotFound(new { message = "No OrderDetail with that ID exists, please try again" });
            }
            else
            {
                orderDetails.Order = _context.Orders.FirstOrDefault(o => o.OrderId == _context.OrderDetails.FirstOrDefault(c => c.OrderDetailId == id).OrderId);
                orderDetails.Product = _context.Products.FirstOrDefault(c => c.ProductId == _context.OrderDetails.FirstOrDefault(o => o.OrderDetailId == id).ProductId);
            }

            return Ok(orderDetails);
        }

        // PUT: api/OrderDetails/5
        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutOrderDetails(int id, OrderDetailsVM orderDetail)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            if (!userSuperUserAuthorised)
            {
                return BadRequest(new { message = "Not authorised to update orderDetails" });
            }


            int currentOrderDetailId = 0;

            try
            {
                OrderDetail updateOrderDetail = _context.OrderDetails.FirstOrDefault(o => o.OrderDetailId == id);
                int count = 0;
                if (updateOrderDetail == null)
                {
                    return NotFound(new { message = "No OrderDetail with that ID exists, please try again" });
                }

                if (orderDetail.ProductId != 0)
                {
                    if(updateOrderDetail.ProductId != orderDetail.ProductId) 
                    {
                        updateOrderDetail.ProductId = orderDetail.ProductId;
                        updateOrderDetail.Product = _context.Products.FirstOrDefault(c => c.ProductId == orderDetail.ProductId);
                        count++;
                    }                    
                }

                if(orderDetail.OrderId != 0)
                {
                    if(updateOrderDetail.OrderId != orderDetail.OrderId)
                    {
                        updateOrderDetail.OrderId = orderDetail.OrderId;
                        updateOrderDetail.Order = _context.Orders.FirstOrDefault(c => c.OrderId == orderDetail.OrderId);
                        count++;
                    }
                }

                if(orderDetail.Quantity != 0)
                {
                    if(updateOrderDetail.Quantity != orderDetail.Quantity)
                    {
                        updateOrderDetail.Quantity = orderDetail.Quantity;
                        count++;
                    }
                }
                
                if(orderDetail.UnitPrice != 0)
                {
                    if (updateOrderDetail.UnitPrice != orderDetail.UnitPrice)
                    {
                        updateOrderDetail.UnitPrice = orderDetail.UnitPrice;
                        count++;
                    }
                }

                if (count > 0)
                {
                    await _context.SaveChangesAsync();
                    currentOrderDetailId = id;
                }
                else
                {
                    return Ok(new { message = "no updates made" });
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailsExists(id))
                {
                    return NotFound(new { message = "OrderDetail Id not found, no changes made, please try again" });
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

            return Ok(new { message = "OrderDetail Updated - OrderDetailId:" + currentOrderDetailId });
        }

        // POST: api/OrderDetails
        [EnableCors("AllowOrigin")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderDetail>> PostOrderDetails(OrderDetailsVM orderDetail)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
            if (userSuperUserAuthorised)
            {

                return BadRequest(new { message = "Not authorised to add orderDetails - Only Customers are allowed" });
            }

            if (userSellerAuthorised)
            {
                return BadRequest(new { message = "Not authorised to add orderDetails - Only Customers are allowed" });
            }

            if (orderDetail.OrderId == 0 || orderDetail.UnitPrice == 0 || orderDetail.Quantity == 0 || orderDetail.ProductId == 0)
            {
                return BadRequest(new { message = "Cannot Add an empty order detail, please you enter a valid order detail" });
            }

            int currentOrderDetailId = 0;

            try
            {
                OrderDetail newOrderDetail = new OrderDetail();
                if (_context.Products.Where(c => c.ProductId == orderDetail.ProductId).Count() == 0)
                {
                    return BadRequest(new { message = "That product does not exist please choose ProductId included in the list below", _context.Products });
                }
                newOrderDetail.ProductId = orderDetail.ProductId;
                if (_context.Orders.Where(c => c.OrderId == orderDetail.OrderId).Count() == 0)
                {
                    return BadRequest(new { message = "That order does not exist please choose OrderId included in the list below", _context.Orders });
                }
                newOrderDetail.OrderId = orderDetail.OrderId;
                newOrderDetail.Quantity = orderDetail.Quantity;
                newOrderDetail.UnitPrice = orderDetail.UnitPrice;
                newOrderDetail.Order = _context.Orders.FirstOrDefault(o => o.OrderId == orderDetail.OrderId);
                newOrderDetail.Product = _context.Products.FirstOrDefault(c => c.ProductId == orderDetail.ProductId);


                _context.OrderDetails.Add(newOrderDetail);
                await _context.SaveChangesAsync();
                currentOrderDetailId = _context.OrderDetails.Max(c => c.OrderDetailId);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in adding OrderDetail, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error in adding OrderDetail, " + e.Message });
            }

            return Ok("New OrderDetail Created - OrderDetailId:" + currentOrderDetailId);
        }

        // DELETE: api/OrderDetails/5
        [EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDetail>> DeleteOrderDetails(int id)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
            if (!userSuperUserAuthorised)
            {
                return BadRequest(new { message = "Not authorised to delete orderDetails" });
            }

            var orderDetails = await _context.OrderDetails.FindAsync(id);
            if (orderDetails == null)
            {
                return NotFound(new { message = "OrderDetail ID not found, please try again" });
            }

            try
            {

                _context.OrderDetails.Remove(orderDetails);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in deleting OrderDetail, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error, " + e.Message });
            }
            return orderDetails;
        }

        private bool OrderDetailsExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderDetailId == id);
        }
    }
}

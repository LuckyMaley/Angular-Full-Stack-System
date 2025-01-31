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
using System.Globalization;
using System.Linq;
using static NuGet.Packaging.PackagingConstants;

namespace LLM_eCommerce_RESTAPI.Controllers
{
    /// <summary>
    /// A summary about ShippingsController class.
    /// </summary>
    /// <remarks>
    /// ShippingsController has the following end points:
    /// Get all Shippings
    /// Get Shippings with id
    /// Get Shippings with method
    /// Get Shippings with Address
    /// Get Shippings with Delivery Status
    /// Get Shippings with Tracking Number
    /// Get Shippings with date
    /// Get Shippings between dates
    /// Put (update) Shipping with id and Shipping object
    /// Post (Add) Shipping using a Shippings View Model 
    /// Delete Shipping with id
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingsController : ControllerBase
    {
        private readonly LLM_eCommerce_EFDBContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthenticationContext _authenticationContext;
        private readonly IdentityHelper _identityHelper;
        public ShippingsController(LLM_eCommerce_EFDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AuthenticationContext authenticationContext)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationContext = authenticationContext;
            _identityHelper = new IdentityHelper(_userManager, _authenticationContext, _roleManager);
        }


        // GET: api/Shippings        
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shipping>>> GetShippings()
        {

            var shippingDB = await _context.Shippings.ToListAsync();

            return Ok(shippingDB);
        }

        // GET: api/Shippings/5
        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Shipping>> GetShippings(int id)
        {
            List<Shipping> allShippings = new List<Shipping>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shippings = await _context.Shippings.FindAsync(id);

            if (shippings == null)
            {
                return NotFound(new { message = "No Shipping with that ID exists, please try again" });
            }
            else
            {
                shippings.Orders = GetAllOrdersByShippingId(id);
            }

            return Ok(shippings);
        }

        // GET: api/Products/specificMethod/method
        [EnableCors("AllowOrigin")]
        [HttpGet("specificMethod/{method}")]
        public async Task<ActionResult<List<Shipping>>> GetShippingByMethod(string method)
        {
            List<Shipping> shippings = new List<Shipping>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var shippingsQuery = _context.Shippings.Where(x => x.ShippingMethod == method);
            if (shippingsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Shipping with that method exists, please try again" });
            }
            var item = shippingsQuery;
            foreach (var shippingItem in item)
            {
                int id = shippingItem.ShippingId;


                if (shippingItem == null)
                {

                    return NotFound(new { message = "No Shipping with that method exists, please try again" });
                }
                else
                {
                    shippingItem.Orders = GetAllOrdersByShippingId(id);
                }

                shippings.Add(shippingItem);
            }
            return Ok(shippings);
        }

        // GET: api/Products/specificAddress/address
        [EnableCors("AllowOrigin")]
        [HttpGet("specificAddress/{address}")]
        public async Task<ActionResult<List<Shipping>>> GetShippingByAddress(string address)
        {
            List<Shipping> shippings = new List<Shipping>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var shippingsQuery = _context.Shippings.Where(x => x.ShippingAddress == address);
            if (shippingsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Shipping with that address exists, please try again" });
            }
            var item = shippingsQuery;
            foreach (var shippingItem in item)
            {
                int id = shippingItem.ShippingId;


                if (shippingItem == null)
                {

                    return NotFound(new { message = "No Shipping with that address exists, please try again" });
                }
                else
                {
                    shippingItem.Orders = GetAllOrdersByShippingId(id);
                }

                shippings.Add(shippingItem);
            }
            return Ok(shippings);
        }

        // GET: api/Products/specificDeliveryStatus/status
        [EnableCors("AllowOrigin")]
        [HttpGet("specificDeliveryStatus/{status}")]
        public async Task<ActionResult<List<Shipping>>> GetShippingByDeliveryStatus(string status)
        {
            List<Shipping> shippings = new List<Shipping>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var shippingsQuery = _context.Shippings.Where(x => x.DeliveryStatus == status);
            if (shippingsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Shipping with that delivery status exists, please try again" });
            }
            var item = shippingsQuery;
            foreach (var shippingItem in item)
            {
                int id = shippingItem.ShippingId;


                if (shippingItem == null)
                {

                    return NotFound(new { message = "No Shipping with that delivery status exists, please try again" });
                }
                else
                {
                    shippingItem.Orders = GetAllOrdersByShippingId(id);
                }

                shippings.Add(shippingItem);
            }
            return Ok(shippings);
        }

        // GET: api/Products/specificTrackingNumber/trackingNumber
        [EnableCors("AllowOrigin")]
        [HttpGet("specificTrackNumber/{trackingNumber}")]
        public async Task<ActionResult<List<Shipping>>> GetShippingByTrackingNumber(string trackingNumber)
        {
            List<Shipping> shippings = new List<Shipping>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var shippingsQuery = _context.Shippings.Where(x => x.TrackingNumber == trackingNumber);
            if (shippingsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Shipping with that tracking number exists, please try again" });
            }
            var item = shippingsQuery;
            foreach (var shippingItem in item)
            {
                int id = shippingItem.ShippingId;


                if (shippingItem == null)
                {

                    return NotFound(new { message = "No Shipping with that tracking number exists, please try again" });
                }
                else
                {
                    shippingItem.Orders = GetAllOrdersByShippingId(id);
                }

                shippings.Add(shippingItem);
            }
            return Ok(shippings);
        }

        // GET: api/Shippings/SpecificDate/date
        [EnableCors("AllowOrigin")]
        [HttpGet("SpecificDateASyyyy-mm-dd/{date}")]
        public async Task<ActionResult<List<Shipping>>> GetShippingByDate(DateTime date)
        {
            List<Shipping> shippings = new List<Shipping>();
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

            List<Shipping> temShippings = _context.Shippings.ToList();
            var shippingsQuery = temShippings.Where(x => x.ShippingDate.Date == dateOutput.Date);
            if (shippingsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Shipping with that date exists, please try again" });
            }
            var item = shippingsQuery;
            foreach (var shippingItem in item)
            {
                int id = shippingItem.ShippingId;


                if (shippingItem == null)
                {

                    return NotFound(new { message = "No Shipping with that date exists, please try again" });
                }
                else
                {
                    shippingItem.Orders = GetAllOrdersByShippingId(id);
                }

                shippings.Add(shippingItem);
            }
            return Ok(shippings);
        }

        // GET: api/Shippings/BetweenDates/date1/date2
        [EnableCors("AllowOrigin")]
        [HttpGet("BetweenDatesBothASyyyy-mm-dd/{{date1}}/{{date2}}")]
        public async Task<ActionResult<List<Shipping>>> GetShippingByBetweenDates(DateTime date1, DateTime date2)
        {
            List<Shipping> shippings = new List<Shipping>();
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

            List<Shipping> temShippings = _context.Shippings.ToList();
            var shippingsQuery = temShippings.Where(x => x.ShippingDate.Date >= date1Output.Date && x.ShippingDate <= date2Output);
            if (shippingsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Shipping with that date range exists, please try again" });
            }
            var item = shippingsQuery;
            foreach (var shippingItem in item)
            {
                int id = shippingItem.ShippingId;


                if (shippingItem == null)
                {

                    return NotFound(new { message = "No Shipping with that date exists, please try again" });
                }
                else
                {
                    shippingItem.Orders = GetAllOrdersByShippingId(id);
                }

                shippings.Add(shippingItem);
            }
            return Ok(shippings);
        }

        // PUT: api/Shippings/5
        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutShippings(int id, ShippingsVM shipping)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            if (!userSuperUserAuthorised)
            {
                return BadRequest(new { message = "Not authorised to update shippings" });
            }


            int currentShippingId = 0;

            try
            {
                Shipping updateShipping = _context.Shippings.FirstOrDefault(o => o.ShippingId == id);
                int count = 0;
                if (updateShipping == null)
                {
                    return NotFound(new { message = "No Shipping with that ID exists, please try again" });
                }
                if (shipping.ShippingMethod != "" || shipping.ShippingMethod != null)
                {
                    if (updateShipping.ShippingMethod != shipping.ShippingMethod)
                    {
                        updateShipping.ShippingMethod = shipping.ShippingMethod;
                        count++;
                    }
                }

                if (shipping.ShippingAddress != "" || shipping.ShippingAddress != null)
                {
                    if (updateShipping.ShippingAddress != shipping.ShippingAddress)
                    {
                        updateShipping.ShippingAddress = shipping.ShippingAddress;
                        count++;
                    }
                }

                if (shipping.ShippingMethod != "" || shipping.ShippingMethod != null)
                {
                    if (updateShipping.ShippingMethod != shipping.ShippingMethod)
                    {
                        updateShipping.ShippingMethod = shipping.ShippingMethod;
                        count++;
                    }
                }

                if (shipping.TrackingNumber != "" || shipping.TrackingNumber != null)
                {
                    if (updateShipping.TrackingNumber != shipping.TrackingNumber)
                    {
                        updateShipping.TrackingNumber = shipping.TrackingNumber;
                        count++;
                    }
                }

                if (shipping.DeliveryStatus != "" || shipping.DeliveryStatus != null)
                {
                    if (updateShipping.DeliveryStatus != shipping.DeliveryStatus)
                    {
                        updateShipping.DeliveryStatus = shipping.DeliveryStatus;
                        count++;
                    }
                }

				if (shipping.ShippingDate != null)
				{
					if (updateShipping.ShippingDate != shipping.ShippingDate)
					{
						updateShipping.ShippingDate = shipping.ShippingDate;
						count++;
					}
				}

				if (count > 0)
                {
                    
                    await _context.SaveChangesAsync();
                    currentShippingId = id;
                }
                else
                {
                    return Ok(new { message = "no updates made" });
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShippingsExists(id))
                {
                    return NotFound(new { message = "Shipping Id not found, no changes made, please try again" });
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

            return Ok(new { message = "Shipping Updated - ShippingId:" + currentShippingId });
        }

        // POST: api/Shippings
        [EnableCors("AllowOrigin")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Shipping>> PostShippings(ShippingsVM shipping)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
            if (userSuperUserAuthorised)
            {

                return BadRequest(new { message = "Not authorised to add shippings - Only Customers are allowed" });
            }

            if (userSellerAuthorised)
            {
                return BadRequest(new { message = "Not authorised to add shippings - Only Customers are allowed" });
            }

            if (shipping.ShippingMethod == null || shipping.ShippingMethod == "" || shipping.DeliveryStatus == null ||  shipping.DeliveryStatus == "" || shipping.TrackingNumber == null || shipping.TrackingNumber == "")
            {
                return BadRequest(new { message = "Cannot Add an empty shipping, please you enter a valid shipping" });
            }

            int currentShippingId = 0;

            try
            {
                var newShipping = new Shipping();
                newShipping.ShippingMethod = shipping.ShippingMethod;
                newShipping.ShippingMethod = shipping.ShippingMethod;
                newShipping.ShippingAddress = shipping.ShippingAddress;
                newShipping.DeliveryStatus = shipping.DeliveryStatus;
                newShipping.ShippingDate = DateTime.Now;
                await _context.SaveChangesAsync();
                newShipping.Orders = GetAllOrdersByShippingId(newShipping.ShippingId);

                _context.Shippings.Add(newShipping);
                await _context.SaveChangesAsync();
                currentShippingId = newShipping.ShippingId;
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in adding Shipping, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error in adding Shipping, " + e.Message });
            }

            return Ok(new { message = "New Shipping Created - ShippingId:" + currentShippingId });
        }

        // DELETE: api/Shippings/5
        [EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Shipping>> DeleteShippings(int id)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            if (!userSuperUserAuthorised)
            {
                return BadRequest(new { message = "Not authorised to delete shippings" });
            }

            var shippings = await _context.Shippings.FindAsync(id);
            if (shippings == null)
            {
                return NotFound(new { message = "Shipping ID not found, please try again" });
            }

            try
            {

                _context.Shippings.Remove(shippings);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in deleting Shipping, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error, " + e.Message });
            }
            return shippings;
        }

        private bool ShippingsExists(int id)
        {
            return _context.Shippings.Any(e => e.ShippingId == id);
        }

        private List<Order> GetAllOrdersByShippingId(int id)
        {
            List<Order> allOrdersForShippings = new List<Order>();

            var ordersQuery =
                    (from orders in _context.Orders
                     where (orders.ShippingId == id)
                     select new
                     {
                         orders.OrderId,
                         orders.ShippingId,
                         orders.OrderDate,
                         orders.OrderDetails,
                         orders.Payments,
                         orders.EfUserId,
                         orders.Shipping,
                         orders.TotalAmount
                     }).ToList();


            foreach (var ord in ordersQuery)
            {
                allOrdersForShippings.Add(new Order()
                {
                    OrderId = ord.OrderId,
                    ShippingId = ord.ShippingId,
                    OrderDate = ord.OrderDate,
                    OrderDetails = ord.OrderDetails,
                    Payments = ord.Payments,
                    EfUserId = ord.EfUserId,
                    Shipping = ord.Shipping,
                    TotalAmount = ord.TotalAmount
                });
            }

            return allOrdersForShippings;
        }
    }
}

using LLM_eCommerce_RESTAPI.AuthModels;
using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.Repository;
using LLM_eCommerce_RESTAPI.Services;
using LLM_eCommerce_RESTAPI.ViewModels;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LLM_eCommerce_RESTAPI.Controllers
{
    /// <summary>
    /// A summary about CustomersOrderDetailsController class.
    /// </summary>
    /// <remarks>
    /// CustomersOrderDetailsController requires a user to be logged in and have specific role to access the end points
    /// CustomersOrderDetailsController has the following end points:
    /// Get current logged in user's order details - required role is Customer
    /// Get All CustomersOrderDetails information  - required role is Administrator
    /// Get a CustomersOrderDetails with User id - Authenticated user (Administrator, Seller)
    /// Get OrderDetails with Order id - Authenticated user (Administrator, Seller, Customer)
    /// Get OrderDetails with Shipping id - Authenticated user (Administrator, Seller, Customer)
    /// Using CustomersOrderDetailsRepo
    /// </remarks>


    [Route("cusordd")]
    [ApiController]
    [Authorize]
    public class CustomersOrderDetailsController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger("CustomersOrderDetailsController");

        private readonly LLM_eCommerce_EFDBContext _context;
        private readonly IdentityHelper _identityHelper;

        private UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationContext _authContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomersOrderDetailsController(LLM_eCommerce_EFDBContext context,
            UserManager<ApplicationUser> userManager,
            AuthenticationContext authContext, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _authContext = authContext;
            _roleManager = roleManager;

            _identityHelper = new IdentityHelper(userManager, authContext, roleManager);
        }



        [Route("MyOrders")]
        // GET: /cusordd/MyOrders
        [HttpGet]
        [EnableCors("AllowOrigin")]
        // [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyOrders()
        {
            logger.Info("CustomersOrderDetailsController - GET:  cusordd/MyOrders");

            List<CustomersOrderDetailsVM> customersOrders = new List<CustomersOrderDetailsVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            bool userAuthorisation = await _identityHelper.IsUserInRole(userId, "Administrator");
            bool userAuthorisation2 = await _identityHelper.IsUserInRole(userId, "Seller");

            if (!userAuthorisation && !userAuthorisation2)
            {
                try
                {
                    var loggedInUser = _context.EfUsers.Where(x => x.IdentityUsername == UserName);

                    if (loggedInUser == null)
                    {
                        logger.Warn("CustomersOrderDetailsController - GET:  cusordd/MyOrders - Not Found / invalid user, logged in UserName: " + UserName);
                        return NotFound();
                    }
                    else
                    {
                        int id = loggedInUser.First().EfUserId;
                        CustomersOrderDetailsRepo prod = new CustomersOrderDetailsRepo(_context);
                        customersOrders = prod.GetCustomerOrderDetails(id).ToList();

                        return Ok(new { customersOrders });
                    }
                }
                catch (Exception e)
                {
                    logger.Error("CustomersOrderDetailsController - GET:  cusordd/MyOrders - Not Found / invalid user, logged in UserName: " + UserName + ".  Exception: " + e);

                    return BadRequest(new { message = "Not Found." });
                }


            }
            else
            {
                return BadRequest(new { message = "Not Authorised." });
            }
        }



        // GET: /cusordd
        [HttpGet]
        [EnableCors("AllowOrigin")]
        // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Get()
        {
            logger.Info("CustomersOrderDetailsController - GET all CustomersOrderDetails:  /cusordd");

            List<CustomersOrderDetailsVM> customersOrders = new List<CustomersOrderDetailsVM>();
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);
			bool rightRole2 = await _identityHelper.IsSellerUserRole(userId);

			if (rightRole || rightRole2)
            {
                CustomersOrderDetailsRepo prod = new CustomersOrderDetailsRepo(_context);
                customersOrders = prod.GetCustomerOrderDetails(0).ToList();

                return Ok(customersOrders);
            }
            else
            {
                var user = await _userManager.FindByIdAsync(userId);
                List<string> userRoles = new List<string>(await _userManager.GetRolesAsync(user));

                logger.Error("CustomersOrderDetailsController - GET all CustomersOrderDetails:  /cusordd  - Not Authorised - logged in User: " + user);

                return Ok(new
                {
                    message = "Not Authorised.",
                    UserName = user.UserName,
                    rightRole,
                    userRoles
                });
            }

        }



        [EnableCors("AllowOrigin")]
        // GET: /cusordd/userId/5
        [HttpGet("userId/{id}")]
        // [Authorize(Roles = "Administrator", "Seller")]
        public async Task<IActionResult> Get(int id)
        {
            logger.Info("CustomersOrderDetailsController - GET:  cusordd/userId/" + id);

            List<CustomersOrderDetailsVM> customersOrders = new List<CustomersOrderDetailsVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);
            bool rightRole2 = await _identityHelper.IsSellerUserRole(userId);

            if (!string.IsNullOrEmpty(UserName) && (rightRole || rightRole2))
            {
                CustomersOrderDetailsRepo prod = new CustomersOrderDetailsRepo(_context);
                customersOrders = prod.GetCustomerOrderDetails(id).ToList();
                if (customersOrders.Count > 0)
                {
                    return Ok(customersOrders);
                }
                else
                {
                    return NotFound(new { message = "Order Not Found." });
                }

            }
            else
            {
                logger.Warn("CustomersOrderDetailsController - GET:  cusordd/userId/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }


        [EnableCors("AllowOrigin")]
        // GET: /cusordd/OrderDetailsInfo/5
        [HttpGet("OrderDetailsInfo/{id}")]
        // [Authorize(Roles = "Administrator", "Seller", "Customer")]
        public async Task<IActionResult> GetOrderInfo(int id)
        {
            logger.Info("CustomersOrderDetailsController - GET:  cusordd/OrderDetailsInfo/" + id);

            CustomersOrderDetailsVM OrderInfo = new CustomersOrderDetailsVM();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            if (!string.IsNullOrEmpty(UserName))
            {
                CustomersOrderDetailsRepo prod = new CustomersOrderDetailsRepo(_context);
                OrderInfo = prod.GetOrderDetails(id);

                return Ok(OrderInfo);
            }
            else
            {
                logger.Warn("CustomersOrderDetailsController - GET:  cusordd/OrderDetailsInfo/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }




        [EnableCors("AllowOrigin")]
        // GET: /cusordd/customersByOrdersId/5
        [HttpGet("customersByOrdersId/{id}")]
        // [Authorize(Roles = "Administrator", "Seller", "Customer")]
        public async Task<IActionResult> OrdersDetailByOrderId(int id)
        {
            logger.Info("CustomersOrderDetailsController - GET:  cusordd/customersByOrders/" + id);

            List<CustomersOrderDetailsVM> customersOrders = new List<CustomersOrderDetailsVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);

            if (!string.IsNullOrEmpty(UserName))
            {
                CustomersOrderDetailsRepo prod = new CustomersOrderDetailsRepo(_context);
                customersOrders = prod.GetOrderDetailsByOrderId(id).ToList();
                if (customersOrders.Count > 0)
                {
                    return Ok(customersOrders);
                }
                else
                {
                    return NotFound(new { message = "Orders with user product id " + id + " Not found" });
                }

            }
            else
            {
                logger.Warn("CustomersOrderDetailsController - GET:  cusordd/customersByOrdersId/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }
    }
}

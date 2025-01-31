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
    /// A summary about CustomersOrdersController class.
    /// </summary>
    /// <remarks>
    /// CustomersOrdersController requires a user to be logged in and have specific role to access the end points
    /// CustomersOrdersController has the following end points:
    /// Get current logged in user's order details - required role is Customer
    /// Get All CustomersOrders information  - required role is Administrator
    /// Get a CustomersOrders with User id - Authenticated user (Administrator, Seller)
    /// Get OrderDetails with Order id - Authenticated user (Administrator, Seller, Customer)
    /// Get OrderDetails with Shipping id - Authenticated user (Administrator, Seller, Customer)
    /// Using CustomersOrdersRepo
    /// </remarks>


    [Route("cusord")]
    [ApiController]
    [Authorize]
    public class CustomersOrdersController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger("CustomersOrdersController");

        private readonly LLM_eCommerce_EFDBContext _context;
        private readonly IdentityHelper _identityHelper;

        private UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationContext _authContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomersOrdersController(LLM_eCommerce_EFDBContext context,
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
        // GET: /cusord/MyOrders
        [HttpGet]
        [EnableCors("AllowOrigin")]
        // [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyOrders()
        {
            logger.Info("CustomersOrdersController - GET:  cusord/MyOrders");

            List<CustomersOrdersVM> customersOrders = new List<CustomersOrdersVM>();

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
                        logger.Warn("CustomersOrdersController - GET:  cusord/MyOrders - Not Found / invalid user, logged in UserName: " + UserName);
                        return NotFound();
                    }
                    else
                    {
                        int id = loggedInUser.First().EfUserId;
                        CustomersOrdersRepo prod = new CustomersOrdersRepo(_context);
                        customersOrders = prod.GetCustomerOrders(id).ToList();

                        return Ok(new { customersOrders });
                    }
                }
                catch (Exception e)
                {
                    logger.Error("CustomersOrdersController - GET:  cusord/MyOrders - Not Found / invalid user, logged in UserName: " + UserName + ".  Exception: " + e);

                    return BadRequest(new { message = "Not Found." });
                }


            }
            else
            {
                return BadRequest(new { message = "Not Authorised." });
            }
        }



        // GET: /cusord
        [HttpGet]
        [EnableCors("AllowOrigin")]
        // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Get()
        {
            logger.Info("CustomersOrdersController - GET all CustomersOrders:  /cusord");

            List<CustomersOrdersVM> customersOrders = new List<CustomersOrdersVM>();
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);

            if (rightRole)
            {
                CustomersOrdersRepo prod = new CustomersOrdersRepo(_context);
                customersOrders = prod.GetCustomerOrders(0).ToList();

                return Ok(customersOrders);
            }
            else
            {
                var user = await _userManager.FindByIdAsync(userId);
                List<string> userRoles = new List<string>(await _userManager.GetRolesAsync(user));

                logger.Error("CustomersOrdersController - GET all CustomersOrders:  /cusord  - Not Authorised - logged in User: " + user);

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
        // GET: /cusord/userId/5
        [HttpGet("userId/{id}")]
        // [Authorize(Roles = "Administrator", "Seller", "Customer")]
        public async Task<IActionResult> Get(int id)
        {
            logger.Info("CustomersOrdersController - GET:  cusord/userId/" + id);

            List<CustomersOrdersVM> customersOrders = new List<CustomersOrdersVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);
            bool rightRole2 = await _identityHelper.IsSellerUserRole(userId);

            if (!string.IsNullOrEmpty(UserName) && (rightRole || rightRole2))
            {
                CustomersOrdersRepo prod = new CustomersOrdersRepo(_context);
                customersOrders = prod.GetCustomerOrders(id).ToList();
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
                logger.Warn("CustomersOrdersController - GET:  cusord/userId/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }


        [EnableCors("AllowOrigin")]
        // GET: /cusord/OrderInfo/5
        [HttpGet("OrderInfo/{id}")]
        // [Authorize(Roles = "Administrator", "Seller", "Customer")]
        public async Task<IActionResult> GetOrderInfo(int id)
        {
            logger.Info("CustomersOrdersController - GET:  cusord/OrderInfo/" + id);

            CustomersOrdersVM orderInfo = new CustomersOrdersVM();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            if (!string.IsNullOrEmpty(UserName))
            {
                CustomersOrdersRepo prod = new CustomersOrdersRepo(_context);
                orderInfo = prod.GetOrderDetails(id);
                if (orderInfo.EfUserId > 0 && orderInfo.OrderId > 0)
                {
                    return Ok(orderInfo);
                }
                else
                {
                    return NotFound(new { message="Order Info not found"});
                }
            }
            else
            {
                logger.Warn("CustomersOrdersController - GET:  cusord/OrderInfo/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }




        [EnableCors("AllowOrigin")]
        // GET: /cusord/customersShippingId/5
        [HttpGet("customersShippingId/{id}")]
        // [Authorize(Roles = "Administrator", "Seller", "Customer")]
        public async Task<IActionResult> OrdersByShippingrId(int id)
        {
            logger.Info("CustomersOrdersController - GET:  cusord/customersShipping/" + id);

            List<CustomersOrdersVM> customersOrders = new List<CustomersOrdersVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);

            if (!string.IsNullOrEmpty(UserName))
            {
                CustomersOrdersRepo prod = new CustomersOrdersRepo(_context);
                customersOrders = prod.GetProductsByShippingId(id).ToList();
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
                logger.Warn("CustomersOrdersController - GET:  cusord/customersShippingId/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }
    }
}

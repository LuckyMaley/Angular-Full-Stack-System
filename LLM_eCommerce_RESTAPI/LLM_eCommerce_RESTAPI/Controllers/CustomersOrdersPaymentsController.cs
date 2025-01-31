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
    /// A summary about CustomersOrdersPaymentsController class.
    /// </summary>
    /// <remarks>
    /// CustomersOrdersPaymentsController requires a user to be logged in and have specific role to access the end points
    /// CustomersOrdersPaymentsController has the following end points:
    /// Get current logged in user's order payments - required role is Customer
    /// Get All CustomersOrdersPayments information  - required role is Administrator
    /// Get a CustomersOrdersPayments with User id - Authenticated user (Administrator, Seller)
    /// Get OrdersPaymentDetails with OrdersPayment id - Authenticated user (Administrator, Seller, Customer)
    /// Get OrdersPaymentDetails with Order id - Authenticated user (Administrator, Seller, Customer)
    /// Using CustomersOrdersPaymentsRepo
    /// </remarks>


    [Route("cusordPay")]
    [ApiController]
    [Authorize]
    public class CustomersOrdersPaymentsController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger("CustomersOrdersPaymentsController");

        private readonly LLM_eCommerce_EFDBContext _context;
        private readonly IdentityHelper _identityHelper;

        private UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationContext _authContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomersOrdersPaymentsController(LLM_eCommerce_EFDBContext context,
            UserManager<ApplicationUser> userManager,
            AuthenticationContext authContext, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _authContext = authContext;
            _roleManager = roleManager;

            _identityHelper = new IdentityHelper(userManager, authContext, roleManager);
        }



        [Route("MyOrdersPayments")]
        // GET: /cusordPay/MyOrdersPayments
        [HttpGet]
        [EnableCors("AllowOrigin")]
        // [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyOrdersPayments()
        {
            logger.Info("CustomersOrdersPaymentsController - GET:  cusordPay/MyOrdersPayments");

            List<CustomersOrdersPaymentsVM> customersOrdersPayments = new List<CustomersOrdersPaymentsVM>();

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
                        logger.Warn("CustomersOrdersPaymentsController - GET:  cusordPay/MyOrdersPayments - Not Found / invalid user, logged in UserName: " + UserName);
                        return NotFound();
                    }
                    else
                    {
                        int id = loggedInUser.First().EfUserId;
                        CustomersOrdersPaymentsRepo prod = new CustomersOrdersPaymentsRepo(_context);
                        customersOrdersPayments = prod.GetOrdersPayments(id).ToList();

                        return Ok(new { customersOrdersPayments });
                    }
                }
                catch (Exception e)
                {
                    logger.Error("CustomersOrdersPaymentsController - GET:  cusordPay/MyOrdersPayments - Not Found / invalid user, logged in UserName: " + UserName + ".  Exception: " + e);

                    return BadRequest(new { message = "Not Found." });
                }


            }
            else
            {
                return BadRequest(new { message = "Not Authorised." });
            }
        }



        // GET: /cusordPay
        [HttpGet]
        [EnableCors("AllowOrigin")]
        // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Get()
        {
            logger.Info("CustomersOrdersPaymentsController - GET all CustomersOrdersPayments:  /cusordPay");

            List<CustomersOrdersPaymentsVM> customersOrdersPayments = new List<CustomersOrdersPaymentsVM>();
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);

            if (rightRole)
            {
                CustomersOrdersPaymentsRepo prod = new CustomersOrdersPaymentsRepo(_context);
                customersOrdersPayments = prod.GetOrdersPayments(0).ToList();

                return Ok(customersOrdersPayments);
            }
            else
            {
                var user = await _userManager.FindByIdAsync(userId);
                List<string> userRoles = new List<string>(await _userManager.GetRolesAsync(user));

                logger.Error("CustomersOrdersPaymentsController - GET all CustomersOrdersPayments:  /cusordPay  - Not Authorised - logged in User: " + user);

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
        // GET: /cusordPay/userId/5
        [HttpGet("userId/{id}")]
        // [Authorize(Roles = "Administrator", "Seller")]
        public async Task<IActionResult> Get(int id)
        {
            logger.Info("CustomersOrdersPaymentsController - GET:  cusordPay/userId/" + id);

            List<CustomersOrdersPaymentsVM> customersOrdersPayments = new List<CustomersOrdersPaymentsVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);
            bool rightRole2 = await _identityHelper.IsSellerUserRole(userId);

            if (!string.IsNullOrEmpty(UserName) && (rightRole || rightRole2))
            {
                CustomersOrdersPaymentsRepo prod = new CustomersOrdersPaymentsRepo(_context);
                customersOrdersPayments = prod.GetOrdersPayments(id).ToList();
                if (customersOrdersPayments.Count > 0)
                {
                    return Ok(customersOrdersPayments);
                }
                else
                {
                    return NotFound(new { message = "OrdersPayment Not Found." });
                }

            }
            else
            {
                logger.Warn("CustomersOrdersPaymentsController - GET:  cusordPay/userId/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }


        [EnableCors("AllowOrigin")]
        // GET: /cusordPay/OrdersPaymentInfo/5
        [HttpGet("OrdersPaymentInfo/{id}")]
        // [Authorize(Roles = "Administrator", "Seller", "Customer")]
        public async Task<IActionResult> GetOrdersPaymentInfo(int id)
        {
            logger.Info("CustomersOrdersPaymentsController - GET:  cusordPay/OrdersPaymentInfo/" + id);

            CustomersOrdersPaymentsVM OrdersPaymentInfo = new CustomersOrdersPaymentsVM();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            if (!string.IsNullOrEmpty(UserName))
            {
                CustomersOrdersPaymentsRepo prod = new CustomersOrdersPaymentsRepo(_context);
                OrdersPaymentInfo = prod.GetPaymentDetails(id);

                return Ok(OrdersPaymentInfo);
            }
            else
            {
                logger.Warn("CustomersOrdersPaymentsController - GET:  cusordPay/OrdersPaymentInfo/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }




        [EnableCors("AllowOrigin")]
        // GET: /cusordPay/OrderId/5
        [HttpGet("OrderId/{id}")]
        // [Authorize(Roles = "Administrator", "Seller", "Customer")]
        public async Task<IActionResult> OrdersPaymentsByOrderId(int id)
        {
            logger.Info("CustomersOrdersPaymentsController - GET:  cusordPay/OrderId/" + id);

            List<CustomersOrdersPaymentsVM> customersOrdersPayments = new List<CustomersOrdersPaymentsVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);

            if (!string.IsNullOrEmpty(UserName))
            {
                CustomersOrdersPaymentsRepo prod = new CustomersOrdersPaymentsRepo(_context);
                customersOrdersPayments = prod.GetPaymentsByOrderId(id).ToList();
                if (customersOrdersPayments.Count > 0)
                {
                    return Ok(customersOrdersPayments);
                }
                else
                {
                    return NotFound(new { message = "OrdersPayments with user product id " + id + " Not found" });
                }

            }
            else
            {
                logger.Warn("CustomersOrdersPaymentsController - GET:  cusordPay/OrderId/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }
    }
}

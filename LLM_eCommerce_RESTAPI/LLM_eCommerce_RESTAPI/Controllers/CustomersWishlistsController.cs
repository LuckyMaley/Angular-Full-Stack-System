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
    /// A summary about CustomersWishlistsController class.
    /// </summary>
    /// <remarks>
    /// CustomersWishlistsController requires a user to be logged in and have specific role to access the end points
    /// CustomersWishlistsController has the following end points:
    /// Get current logged in user's wishlists - required role is Customer
    /// Get All CustomersWishlists information  - required role is Administrator
    /// Get a CustomersWishlists with User id - Authenticated user (Administrator, Seller)
    /// Get WishlistDetails with Wishlist id - Authenticated user (Administrator, Seller, Customer)
    /// Get WishlistDetails with product id - Authenticated user (Administrator, Seller, Customer)
    /// Using CustomersWishlistsRepo
    /// </remarks>
    [Route("cuswish")]
    [ApiController]
    [Authorize]
    public class CustomersWishlistsController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger("CustomersWishlistsController");

        private readonly LLM_eCommerce_EFDBContext _context;
        private readonly IdentityHelper _identityHelper;

        private UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationContext _authContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomersWishlistsController(LLM_eCommerce_EFDBContext context,
            UserManager<ApplicationUser> userManager,
            AuthenticationContext authContext, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _authContext = authContext;
            _roleManager = roleManager;

            _identityHelper = new IdentityHelper(userManager, authContext, roleManager);
        }



        [Route("MyWishlists")]
        // GET: /cuswish/MyWishlists
        [HttpGet]
        [EnableCors("AllowOrigin")]
        // [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyWishlists()
        {
            logger.Info("CustomersWishlistsController - GET:  cuswish/MyWishlists");

            List<CustomersWishlistsVM> customersWishlists = new List<CustomersWishlistsVM>();

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
                        logger.Warn("CustomersWishlistsController - GET:  cuswish/MyWishlists - Not Found / invalid user, logged in UserName: " + UserName);
                        return NotFound();
                    }
                    else
                    {
                        int id = loggedInUser.First().EfUserId;
                        CustomersWishlistsRepo prod = new CustomersWishlistsRepo(_context);
                        customersWishlists = prod.GetCustomersWishlists(id).ToList();

                        return Ok(new { customersWishlists });
                    }
                }
                catch (Exception e)
                {
                    logger.Error("CustomersWishlistsController - GET:  cuswish/MyWishlists - Not Found / invalid user, logged in UserName: " + UserName + ".  Exception: " + e);

                    return BadRequest(new { message = "Not Found." });
                }


            }
            else
            {
                return BadRequest(new { message = "Not Authorised." });
            }
        }



        // GET: /cuswish
        [HttpGet]
        [EnableCors("AllowOrigin")]
        // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Get()
        {
            logger.Info("CustomersWishlistsController - GET all CustomersWishlists:  /cuswish");

            List<CustomersWishlistsVM> customersWishlists = new List<CustomersWishlistsVM>();
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);

            if (rightRole)
            {
                CustomersWishlistsRepo prod = new CustomersWishlistsRepo(_context);
                customersWishlists = prod.GetCustomersWishlists(0).ToList();

                return Ok(customersWishlists);
            }
            else
            {
                var user = await _userManager.FindByIdAsync(userId);
                List<string> userRoles = new List<string>(await _userManager.GetRolesAsync(user));

                logger.Error("CustomersWishlistsController - GET all CustomersWishlists:  /cuswish  - Not Authorised - logged in User: " + user);

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
        // GET: /cuswish/userId/5
        [HttpGet("userId/{id}")]
        // [Authorize(Roles = "Administrator", "Seller")]
        public async Task<IActionResult> Get(int id)
        {
            logger.Info("CustomersWishlistsController - GET:  cuswish/userId/" + id);

            List<CustomersWishlistsVM> customersWishlists = new List<CustomersWishlistsVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);
            bool rightRole2 = await _identityHelper.IsSellerUserRole(userId);

            if (!string.IsNullOrEmpty(UserName) && (rightRole || rightRole2))
            {
                CustomersWishlistsRepo prod = new CustomersWishlistsRepo(_context);
                customersWishlists = prod.GetCustomersWishlists(id).ToList();
                if (customersWishlists.Count > 0)
                {
                    return Ok(customersWishlists);
                }
                else
                {
                    return NotFound(new { message = "Wishlist Not Found." });
                }

            }
            else
            {
                logger.Warn("CustomersWishlistsController - GET:  cuswish/userId/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }


        [EnableCors("AllowOrigin")]
        // GET: /cuswish/WishlistInfo/5
        [HttpGet("WishlistInfo/{id}")]
        // [Authorize(Roles = "Administrator", "Seller", "Customer")]
        public async Task<IActionResult> GetWishlistInfo(int id)
        {
            logger.Info("CustomersWishlistsController - GET:  cuswish/WishlistInfo/" + id);

            CustomersWishlistsVM WishlistInfo = new CustomersWishlistsVM();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            if (!string.IsNullOrEmpty(UserName))
            {
                CustomersWishlistsRepo prod = new CustomersWishlistsRepo(_context);
                WishlistInfo = prod.GetWishlistDetails(id);

                return Ok(WishlistInfo);
            }
            else
            {
                logger.Warn("CustomersWishlistsController - GET:  cuswish/WishlistInfo/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }




        [EnableCors("AllowOrigin")]
        // GET: /cuswish/wishlistsByProductId/5
        [HttpGet("wishlistsByProductId/{id}")]
        // [Authorize(Roles = "Administrator", "Seller", "Customer")]
        public async Task<IActionResult> WishlistsByProductId(int id)
        {
            logger.Info("CustomersWishlistsController - GET:  cuswish/wishlistsByProductId/" + id);

            List<CustomersWishlistsVM> customersWishlists = new List<CustomersWishlistsVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);

            if (!string.IsNullOrEmpty(UserName))
            {
                CustomersWishlistsRepo prod = new CustomersWishlistsRepo(_context);
                customersWishlists = prod.GetWishlistsByProductId(id).ToList();
                if (customersWishlists.Count > 0)
                {
                    return Ok(customersWishlists);
                }
                else
                {
                    return NotFound(new { message = "Wishlists with user product id " + id + " Not found" });
                }

            }
            else
            {
                logger.Warn("CustomersWishlistsController - GET:  cuswish/wishlistsByProductId/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }
    }
}

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
    /// A summary about UsersProductsController class.
    /// </summary>
    /// <remarks>
    /// UsersProductsController requires a user to be logged in and have specific role to access the end points
    /// UsersProductsController has the following end points:
    /// Get current logged in user's products - required role is Administrator or Seller
    /// Get All UsersProducts information  - required role is Administrator
    /// Get a UsersProducts with User id - Authenticated user (Administrator)
    /// Get a ProductDetails with Product id - Authenticated user (Administrator)
    /// Get a ProductDetails with User Product id - Authenticated user (Administrator)
    /// Using UsersProductsRepo
    /// </remarks>


    [Route("usrsprds")]
    [ApiController]
    [Authorize]
    public class UsersProductsController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger("UsersProductsController");

        private readonly LLM_eCommerce_EFDBContext _context;
        private readonly IdentityHelper _identityHelper;

        private UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationContext _authContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersProductsController(LLM_eCommerce_EFDBContext context,
            UserManager<ApplicationUser> userManager,
            AuthenticationContext authContext, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _authContext = authContext;
            _roleManager = roleManager;

            _identityHelper = new IdentityHelper(userManager, authContext, roleManager);
        }



        [Route("MyProducts")]
        // GET: /usrsprds/MyProducts
        [HttpGet]
        [EnableCors("AllowOrigin")]
        // [Authorize(Roles = "Administrator,"Seller")]
        public async Task<IActionResult> GetMyProducts()
        {
            logger.Info("UsersProductsController - GET:  usrsprds/MyProducts");

            List<UsersProductsVM> usersProducts = new List<UsersProductsVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            bool userAuthorisation = await _identityHelper.IsUserInRole(userId, "Administrator");
            bool userAuthorisation2 = await _identityHelper.IsUserInRole(userId, "Seller");

            if (userAuthorisation || userAuthorisation2)
            {
                try
                {
                    var loggedInUser = _context.EfUsers.Where(x => x.IdentityUsername == UserName);

                    if (loggedInUser == null)
                    {
                        logger.Warn("UsersProductsController - GET:  usrsprds/MyProducts - Not Found / invalid user, logged in UserName: " + UserName);
                        return NotFound("usrsprds/MyProducts - Not Found / invalid user");
                    }
                    else
                    {
                        int id = loggedInUser.First().EfUserId;
                        UsersProductsRepo prod = new UsersProductsRepo(_context);
                        usersProducts = prod.GetUsersProducts(id).ToList();

                        return Ok(new { usersProducts });
                    }
                }
                catch (Exception e)
                {
                    logger.Error("UsersProductsController - GET:  usrsprds/MyProducts - Not Found / invalid user, logged in UserName: " + UserName + ".  Exception: " + e);

                    return BadRequest(new { message = "Not Found." });
                }


            }
            else
            {
                return BadRequest(new { message = "Not Authorised." });
            }
        }

        [Route("MyProductsOrders")]
        // GET: /usrsprds/MyProductsOrders
        [HttpGet]
        [EnableCors("AllowOrigin")]
        // [Authorize(Roles = "Administrator,"Seller")]
        public async Task<IActionResult> GetMyProductsOrders()
        {
            logger.Info("UsersProductsController - GET:  usrsprds/MyProducts");

            List<UsersProductsOrdersVM> usersProductsOrders = new List<UsersProductsOrdersVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            bool userAuthorisation = await _identityHelper.IsUserInRole(userId, "Administrator");
            bool userAuthorisation2 = await _identityHelper.IsUserInRole(userId, "Seller");

            if (userAuthorisation || userAuthorisation2)
            {
                try
                {
                    var loggedInUser = _context.EfUsers.Where(x => x.IdentityUsername == UserName);

                    if (loggedInUser == null)
                    {
                        logger.Warn("UsersProductsController - GET:  usrsprds/MyProductsOrders - Not Found / invalid user, logged in UserName: " + UserName);
                        return NotFound("usrsprds/MyProductsOrders - Not Found / invalid user");
                    }
                    else
                    {
                        int id = loggedInUser.First().EfUserId;
                        UsersProductsRepo prod = new UsersProductsRepo(_context);
                        usersProductsOrders = prod.GetUsersProductsOrders(id).ToList();

                        return Ok(new { usersProductsOrders });
                    }
                }
                catch (Exception e)
                {
                    logger.Error("UsersProductsController - GET:  usrsprds/MyProductsOrders - Not Found / invalid user, logged in UserName: " + UserName + ".  Exception: " + e);

                    return BadRequest(new { message = "Not Found." });
                }


            }
            else
            {
                return BadRequest(new { message = "Not Authorised." });
            }
        }

        // GET: /usrsprds
        [HttpGet]
        [EnableCors("AllowOrigin")]
        // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Get()
        {
            logger.Info("UsersProductsController - GET all UsersProducts:  /usrsprds");

            List<UsersProductsVM> usersProducts = new List<UsersProductsVM>();
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);

            if (rightRole)
            {
                UsersProductsRepo prod = new UsersProductsRepo(_context);
                usersProducts = prod.GetUsersProducts(0).ToList();

                return Ok(usersProducts);
            }
            else
            {
                var user = await _userManager.FindByIdAsync(userId);
                List<string> userRoles = new List<string>(await _userManager.GetRolesAsync(user));

                logger.Error("UsersProductsController - GET all UsersProducts:  /usrsprds  - Not Authorised - logged in User: " + user);

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
        // GET: /usrsprds/userId/5
        [HttpGet("userId/{id}")]
        // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Get(int id)
        {
            logger.Info("UsersProductsController - GET:  usrsprds/userId/" + id);

            List<UsersProductsVM> usersProducts = new List<UsersProductsVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);
            bool rightRole2 = await _identityHelper.IsSellerUserRole(userId);

            if (!string.IsNullOrEmpty(UserName) && (rightRole || rightRole2))
            {
                UsersProductsRepo prod = new UsersProductsRepo(_context);
                usersProducts = prod.GetUsersProducts(id).ToList();
                if(usersProducts.Count > 0)
                {
                    return Ok(usersProducts);
                }
                else
                {
                    return NotFound(new { message = "Product Not Found." });
                }
                
            }
            else
            {
                logger.Warn("UsersProductsController - GET:  usrsprds/userId/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }


        [EnableCors("AllowOrigin")]
        // GET: /usrsprds/ProductInfo/5
        [HttpGet("ProductInfo/{id}")]
        // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetProductInfo(int id)
        {
            logger.Info("UsersProductsController - GET:  usrsprds/ProductInfo/" + id);

            UsersProductsVM ProductInfo = new UsersProductsVM();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            bool rightRole = await _identityHelper.IsSuperUserRole(userId);
            bool rightRole2 = await _identityHelper.IsSellerUserRole(userId);

            if (!string.IsNullOrEmpty(UserName) && (rightRole || rightRole2))
            {
                UsersProductsRepo prod = new UsersProductsRepo(_context);
                ProductInfo = prod.GetProductDetails(id);

                return Ok(ProductInfo);
            }
            else
            {
                logger.Warn("UsersProductsController - GET:  usrsprds/ProductInfo/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }




        [EnableCors("AllowOrigin")]
        // GET: /usrsprds/usersProductsId/5
        [HttpGet("usersProductsId/{id}")]
        // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ProductsByUserProductId(int id)
        {
            logger.Info("UsersProductsController - GET:  usrsprds/usersProducts/" + id);

            List<UsersProductsVM> usersProducts = new List<UsersProductsVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);

            if (!string.IsNullOrEmpty(UserName) && rightRole)
            {
                UsersProductsRepo prod = new UsersProductsRepo(_context);
                usersProducts = prod.GetProductsByUserProductId(id).ToList();
                if (usersProducts.Count > 0)
                {
                    return Ok(usersProducts);
                }
                else
                {
                    return NotFound(new { message= "Products with user product id " + id + " Not found"});
                }
                
            }
            else
            {
                logger.Warn("UsersProductsController - GET:  usrsprds/usersProductsId/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }
    }
}

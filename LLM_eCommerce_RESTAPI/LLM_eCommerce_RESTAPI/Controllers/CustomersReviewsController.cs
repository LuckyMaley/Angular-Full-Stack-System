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
    /// A summary about CustomersReviewsController class.
    /// </summary>
    /// <remarks>
    /// CustomersReviewsController requires a user to be logged in and have specific role to access the end points
    /// CustomersReviewsController has the following end points:
    /// Get current logged in user's reviews - required role is Customer
    /// Get All CustomersReviews information  - required role is Administrator
    /// Get a CustomersReviews with User id - Authenticated user (Administrator, Seller)
    /// Get ReviewDetails with Review id - Authenticated user (Administrator, Seller, Customer)
    /// Get ReviewDetails with Product id - Authenticated user (Administrator, Seller, Customer)
    /// Using CustomersReviewsRepo
    /// </remarks>
    [Route("cusrev")]
    [ApiController]
    public class CustomersReviewsController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger("CustomersReviewsController");

        private readonly LLM_eCommerce_EFDBContext _context;
        private readonly IdentityHelper _identityHelper;

        private UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationContext _authContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomersReviewsController(LLM_eCommerce_EFDBContext context,
            UserManager<ApplicationUser> userManager,
            AuthenticationContext authContext, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _authContext = authContext;
            _roleManager = roleManager;

            _identityHelper = new IdentityHelper(userManager, authContext, roleManager);
        }


		[Authorize]
		[Route("MyReviews")]
        // GET: /cusrev/MyReviews
        [HttpGet]
        [EnableCors("AllowOrigin")]
        // [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyReviews()
        {
            logger.Info("CustomersReviewsController - GET:  cusrev/MyReviews");

            List<CustomersReviewsVM> customersReviews = new List<CustomersReviewsVM>();

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
                        logger.Warn("CustomersReviewsController - GET:  cusrev/MyReviews - Not Found / invalid user, logged in UserName: " + UserName);
                        return NotFound();
                    }
                    else
                    {
                        int id = loggedInUser.First().EfUserId;
                        CustomersReviewsRepo prod = new CustomersReviewsRepo(_context);
                        customersReviews = prod.GetCustomersReviews(id).ToList();

                        return Ok(new { customersReviews });
                    }
                }
                catch (Exception e)
                {
                    logger.Error("CustomersReviewsController - GET:  cusrev/MyReviews - Not Found / invalid user, logged in UserName: " + UserName + ".  Exception: " + e);

                    return BadRequest(new { message = "Not Found." });
                }


            }
            else
            {
                return BadRequest(new { message = "Not Authorised." });
            }
        }



		// GET: /cusrev
		[Authorize]
		[HttpGet]
        [EnableCors("AllowOrigin")]
        // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Get()
        {
            logger.Info("CustomersReviewsController - GET all CustomersReviews:  /cusrev");

            List<CustomersReviewsVM> customersReviews = new List<CustomersReviewsVM>();
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);

            if (rightRole)
            {
                CustomersReviewsRepo prod = new CustomersReviewsRepo(_context);
                customersReviews = prod.GetCustomersReviews(0).ToList();

                return Ok(customersReviews);
            }
            else
            {
                var user = await _userManager.FindByIdAsync(userId);
                List<string> userRoles = new List<string>(await _userManager.GetRolesAsync(user));

                logger.Error("CustomersReviewsController - GET all CustomersReviews:  /cusrev  - Not Authorised - logged in User: " + user);

                return Ok(new
                {
                    message = "Not Authorised.",
                    UserName = user.UserName,
                    rightRole,
                    userRoles
                });
            }

        }

		[HttpGet("allreviews")]
		[EnableCors("AllowOrigin")]
		public async Task<IActionResult> GetAll()
		{
			logger.Info("CustomersReviewsController - GET all CustomersReviews:  /cusrev/allreviews");

			List<CustomersReviewsVM> customersReviews = new List<CustomersReviewsVM>();
			CustomersReviewsRepo prod = new CustomersReviewsRepo(_context);
			customersReviews = prod.GetCustomersReviews(0).ToList();

			return Ok(customersReviews);

		}

		[EnableCors("AllowOrigin")]
        // GET: /cusrev/userId/5
        [HttpGet("userId/{id}")]
        // [Authorize(Roles = "Administrator", "Seller", "Customer")]
        public async Task<IActionResult> Get(int id)
        {
            logger.Info("CustomersReviewsController - GET:  cusrev/userId/" + id);

            List<CustomersReviewsVM> customersReviews = new List<CustomersReviewsVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);
            bool rightRole2 = await _identityHelper.IsSellerUserRole(userId);

            if (!string.IsNullOrEmpty(UserName) && (rightRole || rightRole2))
            {
                CustomersReviewsRepo prod = new CustomersReviewsRepo(_context);
                customersReviews = prod.GetCustomersReviews(id).ToList();
                if (customersReviews.Count > 0)
                {
                    return Ok(customersReviews);
                }
                else
                {
                    return NotFound(new { message = "Review Not Found." });
                }

            }
            else
            {
                logger.Warn("CustomersReviewsController - GET:  cusrev/userId/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }

		[Authorize]
		[EnableCors("AllowOrigin")]
        // GET: /cusrev/ReviewInfo/5
        [HttpGet("ReviewInfo/{id}")]
        // [Authorize(Roles = "Administrator", "Seller", "Customer")]
        public async Task<IActionResult> GetReviewInfo(int id)
        {
            logger.Info("CustomersReviewsController - GET:  cusrev/ReviewInfo/" + id);

            CustomersReviewsVM ReviewInfo = new CustomersReviewsVM();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;

            if (!string.IsNullOrEmpty(UserName))
            {
                CustomersReviewsRepo prod = new CustomersReviewsRepo(_context);
                ReviewInfo = prod.GetReviewDetails(id);

                return Ok(ReviewInfo);
            }
            else
            {
                logger.Warn("CustomersReviewsController - GET:  cusrev/ReviewInfo/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }



		[Authorize]
		[EnableCors("AllowOrigin")]
        // GET: /cusrev/ProductId/5
        [HttpGet("ProductId/{id}")]
        // [Authorize(Roles = "Administrator", "Seller", "Customer")]
        public async Task<IActionResult> ReviewsByProductId(int id)
        {
            logger.Info("CustomersReviewsController - GET:  cusrev/Reviews/" + id);

            List<CustomersReviewsVM> customersReviews = new List<CustomersReviewsVM>();

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            string UserName = user.UserName;
            bool rightRole = await _identityHelper.IsSuperUserRole(userId);

            if (!string.IsNullOrEmpty(UserName))
            {
                CustomersReviewsRepo prod = new CustomersReviewsRepo(_context);
                customersReviews = prod.GetReviewsByProductId(id).ToList();
                if (customersReviews.Count > 0)
                {
                    return Ok(customersReviews);
                }
                else
                {
                    return NotFound(new { message = "Reviews with user product id " + id + " Not found" });
                }

            }
            else
            {
                logger.Warn("CustomersReviewsController - GET:  cusrev/ProductId/" + id + "logged in User: " + user);

                return BadRequest(new { message = "Not Authorised." });
            }
        }
    }
}

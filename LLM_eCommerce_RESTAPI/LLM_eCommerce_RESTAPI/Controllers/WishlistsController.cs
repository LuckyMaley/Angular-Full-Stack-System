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

namespace LLM_eCommerce_RESTAPI.Controllers
{
    /// <summary>
    /// A summary about WishlistsController class.
    /// </summary>
    /// <remarks>
    /// WishlistsController has the following end points:
    /// Get all Wishlists
    /// Get Wishlists with id
    /// Get Wishlists between dates
    /// Put (update) Wishlist with id and Wishlist object
    /// Post (Add) Wishlist using a Wishlists View Model 
    /// Delete Wishlist with id
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        private readonly LLM_eCommerce_EFDBContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthenticationContext _authenticationContext;
        private readonly IdentityHelper _identityHelper;
        public WishlistsController(LLM_eCommerce_EFDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AuthenticationContext authenticationContext)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationContext = authenticationContext;
            _identityHelper = new IdentityHelper(_userManager, _authenticationContext, _roleManager);
        }


        // GET: api/Wishlists        
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wishlist>>> GetWishlists()
        {

            var wishlistDB = await _context.Wishlists.ToListAsync();

            return Ok(wishlistDB);
        }

        // GET: api/Wishlists/5
        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Wishlist>> GetWishlists(int id)
        {
            List<Wishlist> allWishlists = new List<Wishlist>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wishlists = await _context.Wishlists.FindAsync(id);

            if (wishlists == null)
            {
                return NotFound(new { message = "No Wishlist with that ID exists, please try again" });
            }
            else
            {
                wishlists.EfUser = _context.EfUsers.FirstOrDefault(c => c.EfUserId == _context.Wishlists.FirstOrDefault(d => d.WishlistId == id).EfUserId);
                wishlists.Product = _context.Products.FirstOrDefault(c => c.ProductId == _context.Wishlists.FirstOrDefault(c => c.WishlistId == id).ProductId);
            }

            return Ok(wishlists);
        }

        // GET: api/Wishlists/SpecificDate/date
        [EnableCors("AllowOrigin")]
        [HttpGet("SpecificDateASyyyy-mm-dd/{date}")]
        public async Task<ActionResult<List<Wishlist>>> GetWishlistByDate(DateTime date)
        {
            List<Wishlist> wishlists = new List<Wishlist>();
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

            List<Wishlist> temWishlists = _context.Wishlists.ToList();
            var wishlistsQuery = temWishlists.Where(x => x.AddedDate.Date == dateOutput.Date);
            if (wishlistsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Wishlist with that date exists, please try again" });
            }
            var item = wishlistsQuery;
            foreach (var wishlistItem in item)
            {
                int id = wishlistItem.WishlistId;


                if (wishlistItem == null)
                {

                    return NotFound(new { message = "No Wishlist with that date exists, please try again" });
                }
                else
                {
                    wishlistItem.EfUser = _context.EfUsers.FirstOrDefault(c => c.EfUserId == _context.Wishlists.FirstOrDefault(d => d.WishlistId == id).EfUserId);
                    wishlistItem.Product = _context.Products.FirstOrDefault(c => c.ProductId == _context.Wishlists.FirstOrDefault(c => c.WishlistId == id).ProductId);
                }

                wishlists.Add(wishlistItem);
            }
            return Ok(wishlists);
        }

        // GET: api/Wishlists/BetweenDates/date1/date2
        [EnableCors("AllowOrigin")]
        [HttpGet("BetweenDatesBothASyyyy-mm-dd/{{date1}}/{{date2}}")]
        public async Task<ActionResult<List<Wishlist>>> GetWishlistByBetweenDates(DateTime date1, DateTime date2)
        {
            List<Wishlist> wishlists = new List<Wishlist>();
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

            List<Wishlist> temWishlists = _context.Wishlists.ToList();
            var wishlistsQuery = temWishlists.Where(x => x.AddedDate.Date >= date1Output.Date && x.AddedDate <= date2Output);
            if (wishlistsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Wishlist with that date range exists, please try again" });
            }
            var item = wishlistsQuery;
            foreach (var wishlistItem in item)
            {
                int id = wishlistItem.WishlistId;


                if (wishlistItem == null)
                {

                    return NotFound(new { message = "No Wishlist with that date exists, please try again" });
                }
                else
                {
                    wishlistItem.EfUser = _context.EfUsers.FirstOrDefault(c => c.EfUserId == _context.Wishlists.FirstOrDefault(d => d.WishlistId == id).EfUserId);
                    wishlistItem.Product = _context.Products.FirstOrDefault(c => c.ProductId == _context.Wishlists.FirstOrDefault(c => c.WishlistId == id).ProductId);
                }

                wishlists.Add(wishlistItem);
            }
            return Ok(wishlists);
        }

        // PUT: api/Wishlists/5
        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutWishlists(int id, WishlistsVM wishlist)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            if (!userSuperUserAuthorised)
            {
                return BadRequest(new { message = "Not authorised to update wishlists" });
            }


            int currentWishlistId = 0;

            try
            {
                Wishlist updateWishlist = _context.Wishlists.FirstOrDefault(o => o.WishlistId == id);
                int count = 0;
                if (updateWishlist == null)
                {
                    return NotFound(new { message = "No Wishlist with that ID exists, please try again" });
                }

                if (wishlist.ProductId != 0)
                {
                    if (updateWishlist.ProductId != wishlist.ProductId)
                    {
                        updateWishlist.ProductId = wishlist.ProductId;
                        count++;
                    }
                }

               

                if (count > 0)
                {
                    updateWishlist.AddedDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                    currentWishlistId = id;
                }
                else
                {
                    return Ok(new { message = "no updates made" });
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WishlistsExists(id))
                {
                    return NotFound(new { message = "Wishlist Id not found, no changes made, please try again" });
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

            return Ok(new { message = "Wishlist Updated - WishlistId:" + currentWishlistId });
        }

        // POST: api/Wishlists
        [EnableCors("AllowOrigin")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Wishlist>> PostWishlists(WishlistsVM wishlist)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
            if (userSuperUserAuthorised)
            {

                return BadRequest(new { message = "Not authorised to add wishlists - Only Customers are allowed" });
            }

            if (userSellerAuthorised)
            {
                return BadRequest(new { message = "Not authorised to add wishlists - Only Customers are allowed" });
            }

            if (wishlist.ProductId == 0 )
            {
                return BadRequest(new { message = "Cannot Add an empty wishlist, please you enter a valid wishlist" });
            }

            int currentWishlistId = 0;

            try
            {
                var newWishlist = new Wishlist();
                if (_context.Products.Where(c => c.ProductId == wishlist.ProductId).Count() == 0)
                {
                    return BadRequest(new { message = "That product does not exist please choose ProductId included in the list below", _context.Products });
                }
                newWishlist.ProductId = wishlist.ProductId;
                newWishlist.EfUserId = _context.EfUsers.FirstOrDefault(c => c.IdentityUsername == user.UserName).EfUserId;
                newWishlist.AddedDate = DateTime.Now;
                newWishlist.EfUser = _context.EfUsers.FirstOrDefault(c => c.IdentityUsername == user.UserName);
                newWishlist.Product = _context.Products.FirstOrDefault(c => c.ProductId == wishlist.ProductId);

                _context.Wishlists.Add(newWishlist);
                await _context.SaveChangesAsync();
                currentWishlistId = newWishlist.WishlistId;
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in adding Wishlist, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error in adding Wishlist, " + e.Message });
            }

            return Ok(new { message = "New Wishlist Created - WishlistId:" + currentWishlistId});
        }

        // DELETE: api/Wishlists/5
        [EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Wishlist>> DeleteWishlists(int id)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
			bool userSuperUserAuthorised2 = await _identityHelper.IsSellerUserRole(userId);
            if (userSuperUserAuthorised2)
            {
                return BadRequest(new { message = "Not authorised to delete wishlists" });
            }

            var wishlists = await _context.Wishlists.FindAsync(id);
            if (wishlists == null)
            {
                return NotFound(new { message = "Wishlist ID not found, please try again" });
            }

            try
            {

                _context.Wishlists.Remove(wishlists);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in deleting Wishlist, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error, " + e.Message });
            }
            return wishlists;
        }

        private bool WishlistsExists(int id)
        {
            return _context.Wishlists.Any(e => e.WishlistId == id);
        }
    }
}

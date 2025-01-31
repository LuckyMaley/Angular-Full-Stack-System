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
    /// A summary about ReviewsController class.
    /// </summary>
    /// <remarks>
    /// ReviewsController has the following end points:
    /// Get all Reviews
    /// Get Reviews with id
    /// Get Reviews with rating
    /// Get Reviews with title
    /// Get Reviews with date
    /// Get Reviews between dates
    /// Put (update) Review with id and Review object
    /// Post (Add) Review using a Reviews View Model 
    /// Delete Review with id
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly LLM_eCommerce_EFDBContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthenticationContext _authenticationContext;
        private readonly IdentityHelper _identityHelper;
        public ReviewsController(LLM_eCommerce_EFDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AuthenticationContext authenticationContext)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationContext = authenticationContext;
            _identityHelper = new IdentityHelper(_userManager, _authenticationContext, _roleManager);
        }


        // GET: api/Reviews        
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {

            var reviewDB = await _context.Reviews.ToListAsync();

            return Ok(reviewDB);
        }

        // GET: api/Reviews/5
        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReviews(int id)
        {
            List<Review> allReviews = new List<Review>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviews = await _context.Reviews.FindAsync(id);

            if (reviews == null)
            {
                return NotFound(new { message = "No Review with that ID exists, please try again" });
            }
            else
            {
                reviews.EfUser = _context.EfUsers.FirstOrDefault(c => c.EfUserId == _context.Reviews.FirstOrDefault(d => d.ReviewId == id).EfUserId);
                reviews.Product = _context.Products.FirstOrDefault(c => c.ProductId == _context.Reviews.FirstOrDefault(c => c.ReviewId == id).ProductId);
            }

            return Ok(reviews);
        }

        // GET: api/Products/specificRating/rating
        [EnableCors("AllowOrigin")]
        [HttpGet("specificRating/{rating}")]
        public async Task<ActionResult<List<Review>>> GetReviewByRating(int rating)
        {
            List<Review> reviews = new List<Review>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var reviewsQuery = _context.Reviews.Where(x => x.Rating == rating);
            if (reviewsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Review with that rating exists, please try again" });
            }
            var item = reviewsQuery;
            foreach (var reviewItem in item)
            {
                int id = reviewItem.ReviewId;


                if (reviewItem == null)
                {

                    return NotFound(new { message = "No Review with that rating exists, please try again" });
                }
                else
                {
                    reviewItem.EfUser = _context.EfUsers.FirstOrDefault(c => c.EfUserId == _context.Reviews.FirstOrDefault(d => d.ReviewId == id).EfUserId);
                    reviewItem.Product = _context.Products.FirstOrDefault(c => c.ProductId == _context.Reviews.FirstOrDefault(c => c.ReviewId == id).ProductId);
                }

                reviews.Add(reviewItem);
            }
            return Ok(reviews);
        }

        // GET: api/Products/specificTitle/title
        [EnableCors("AllowOrigin")]
        [HttpGet("specificTitle/{title}")]
        public async Task<ActionResult<List<Review>>> GetReviewByTitle(string title)
        {
            List<Review> reviews = new List<Review>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var reviewsQuery = _context.Reviews.Where(x => x.Title == title);
            if (reviewsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Review with that title exists, please try again" });
            }
            var item = reviewsQuery;
            foreach (var reviewItem in item)
            {
                int id = reviewItem.ReviewId;


                if (reviewItem == null)
                {

                    return NotFound(new { message = "No Review with that title exists, please try again" });
                }
                else
                {
                    reviewItem.EfUser = _context.EfUsers.FirstOrDefault(c => c.EfUserId == _context.Reviews.FirstOrDefault(d => d.ReviewId == id).EfUserId);
                    reviewItem.Product = _context.Products.FirstOrDefault(c => c.ProductId == _context.Reviews.FirstOrDefault(c => c.ReviewId == id).ProductId);
                }

                reviews.Add(reviewItem);
            }
            return Ok(reviews);
        }
      
        // GET: api/Reviews/SpecificDate/date
        [EnableCors("AllowOrigin")]
        [HttpGet("SpecificDateASyyyy-mm-dd/{date}")]
        public async Task<ActionResult<List<Review>>> GetReviewByDate(DateTime date)
        {
            List<Review> reviews = new List<Review>();
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

            List<Review> temReviews = _context.Reviews.ToList();
            var reviewsQuery = temReviews.Where(x => x.ReviewDate.Date == dateOutput.Date);
            if (reviewsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Review with that date exists, please try again" });
            }
            var item = reviewsQuery;
            foreach (var reviewItem in item)
            {
                int id = reviewItem.ReviewId;


                if (reviewItem == null)
                {

                    return NotFound(new { message = "No Review with that date exists, please try again" });
                }
                else
                {
                    reviewItem.EfUser = _context.EfUsers.FirstOrDefault(c => c.EfUserId == _context.Reviews.FirstOrDefault(d => d.ReviewId == id).EfUserId);
                    reviewItem.Product = _context.Products.FirstOrDefault(c => c.ProductId == _context.Reviews.FirstOrDefault(c => c.ReviewId == id).ProductId);
                }

                reviews.Add(reviewItem);
            }
            return Ok(reviews);
        }

        // GET: api/Reviews/BetweenDates/date1/date2
        [EnableCors("AllowOrigin")]
        [HttpGet("BetweenDatesBothASyyyy-mm-dd/{{date1}}/{{date2}}")]
        public async Task<ActionResult<List<Review>>> GetReviewByBetweenDates(DateTime date1, DateTime date2)
        {
            List<Review> reviews = new List<Review>();
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

            List<Review> temReviews = _context.Reviews.ToList();
            var reviewsQuery = temReviews.Where(x => x.ReviewDate.Date >= date1Output.Date && x.ReviewDate <= date2Output);
            if (reviewsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Review with that date range exists, please try again" });
            }
            var item = reviewsQuery;
            foreach (var reviewItem in item)
            {
                int id = reviewItem.ReviewId;


                if (reviewItem == null)
                {

                    return NotFound(new { message = "No Review with that date exists, please try again" });
                }
                else
                {
                    reviewItem.EfUser = _context.EfUsers.FirstOrDefault(c => c.EfUserId == _context.Reviews.FirstOrDefault(d => d.ReviewId == id).EfUserId);
                    reviewItem.Product = _context.Products.FirstOrDefault(c => c.ProductId == _context.Reviews.FirstOrDefault(c => c.ReviewId == id).ProductId);
                }

                reviews.Add(reviewItem);
            }
            return Ok(reviews);
        }

        // PUT: api/Reviews/5
        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutReviews(int id, ReviewsVM review)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
			bool userSuperUserAuthorised2 = await _identityHelper.IsSellerUserRole(userId);
            if (userSuperUserAuthorised2)
            {
                return BadRequest(new { message = "Not authorised to update reviews" });
            }


            int currentReviewId = 0;

            try
            {
                Review updateReview = _context.Reviews.FirstOrDefault(o => o.ReviewId == id);
                int count = 0;
                if (updateReview == null)
                {
                    return NotFound(new { message = "No Review with that ID exists, please try again" });
                }

                if (review.ProductId != 0)
                {
                    if (updateReview.ProductId != review.ProductId)
                    {
                        updateReview.ProductId = review.ProductId;
                        count++;
                    }
                }

                if (review.Rating != 0)
                {
                    if (updateReview.Rating != review.Rating)
                    {
                        updateReview.Rating = review.Rating;
                        count++;
                    }
                }

                if (review.Title != "" || review.Title != null)
                {
                    if (updateReview.Title != review.Title)
                    {
                        updateReview.Title = review.Title;
                        count++;
                    }
                }

                if (review.Comment != "" || review.Comment != null)
                {
                    if (updateReview.Comment != review.Comment)
                    {
                        updateReview.Comment = review.Comment;
                        count++;
                    }
                }

                if (count > 0)
                {
                    updateReview.ReviewDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                    currentReviewId = id;
                }
                else
                {
                    return Ok(new { message = "no updates made" });
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewsExists(id))
                {
                    return NotFound(new { message = "Review Id not found, no changes made, please try again" });
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

            return Ok(new { message = "Review Updated - ReviewId:" + currentReviewId });
        }

        // POST: api/Reviews
        [EnableCors("AllowOrigin")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Review>> PostReviews(ReviewsVM review)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
            if (userSuperUserAuthorised)
            {

                return BadRequest(new { message = "Not authorised to add reviews - Only Customers are allowed" });
            }

            if (userSellerAuthorised)
            {
                return BadRequest(new { message = "Not authorised to add reviews - Only Customers are allowed" });
            }

            if (review.ProductId == 0 || review.Rating == 0)
            {
                return BadRequest(new { message = "Cannot Add an empty review, please you enter a valid review" });
            }

            int currentReviewId = 0;

            try
            {
                var newReview = new Review();
                if (_context.Products.Where(c => c.ProductId == review.ProductId).Count() == 0)
                {
                    return BadRequest(new { message = "That product does not exist please choose ProductId included in the list below", _context.Products });
                }
                newReview.ProductId = review.ProductId;
                newReview.EfUserId = _context.EfUsers.FirstOrDefault(c => c.IdentityUsername == user.UserName).EfUserId;
                newReview.Rating = review.Rating;
                newReview.Title = review.Title;
                newReview.Comment = review.Comment;
                newReview.ReviewDate = DateTime.Now;
                newReview.EfUser = _context.EfUsers.FirstOrDefault(c => c.IdentityUsername == user.UserName);
                newReview.Product = _context.Products.FirstOrDefault(c => c.ProductId == review.ProductId);

                _context.Reviews.Add(newReview);
                await _context.SaveChangesAsync();
                currentReviewId = newReview.ReviewId;
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in adding Review, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error in adding Review, " + e.Message });
            }

            return Ok(new { message = "New Review Created - ReviewId:" + currentReviewId });
        }

        // DELETE: api/Reviews/5
        [EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Review>> DeleteReviews(int id)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
			bool userSuperUserAuthorised2 = await _identityHelper.IsSellerUserRole(userId);
            if (userSuperUserAuthorised2)
            {
                return BadRequest(new { message = "Not authorised to delete reviews" });
            }

            var reviews = await _context.Reviews.FindAsync(id);
            if (reviews == null)
            {
                return NotFound(new { message = "Review ID not found, please try again" });
            }

            try
            {

                _context.Reviews.Remove(reviews);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in deleting Review, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error, " + e.Message });
            }
            return reviews;
        }

        private bool ReviewsExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
        }
    }
}

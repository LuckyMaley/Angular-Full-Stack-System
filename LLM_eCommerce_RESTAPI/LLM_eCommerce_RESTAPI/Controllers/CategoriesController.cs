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

namespace LLM_eCommerce_RESTAPI.Controllers
{
    /// <summary>
    /// A summary about CategoriesController class.
    /// </summary>
    /// <remarks>
    /// CategoriesController has the following end points:
    /// Get all Categories
    /// Get Categories with id
    /// Get Categories with Name
    /// Put (update) Category with id and Category object
    /// Post (Add) Category using a Categories View Model 
    /// Delete Category with id
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly LLM_eCommerce_EFDBContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthenticationContext _authenticationContext;
        private readonly IdentityHelper _identityHelper;
        public CategoriesController(LLM_eCommerce_EFDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AuthenticationContext authenticationContext)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationContext = authenticationContext;
            _identityHelper = new IdentityHelper(_userManager, _authenticationContext, _roleManager);
        }


        // GET: api/Categories        
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {

            var categoryDB = await _context.Categories.ToListAsync();

            return Ok(categoryDB);
        }

        // GET: api/Categories/5
        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategories(int id)
        {
            List<Category> allCategories = new List<Category>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categories = await _context.Categories.FindAsync(id);

            if (categories == null || categories.CategoryId == 0)
            {
                return NotFound(new { message = "No Category with that ID exists, please try again" });
            }
            else
            {
                categories.Products = GetAllProductsByCategoryId(id);
            }

            return Ok(categories);
        }


        // GET: api/Categories/specificCategory/name
        [EnableCors("AllowOrigin")]
        [HttpGet("specificCategory/{name}")]
        public async Task<ActionResult<Category>> GetCategoryByName(string name)
        {
            List<Category> categories = new List<Category>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var categoriesQuery = _context.Categories.Where(x => x.Name == name);
            if(categoriesQuery.Count() == 0)
            {
                return NotFound(new { message = "No Category with that Name exists, please try again" });
            }
            var Item = categoriesQuery;
            foreach (var categoryItem in Item)
            {
                int id = categoryItem.CategoryId;


                if (categoryItem == null)
                {

                    return NotFound(new { message = "No Category with that Name exists, please try again" });
                }
                else
                {
                    categoryItem.Products = GetAllProductsByCategoryId(id);
                }

                categories.Add(categoryItem);
            }
            return Ok(categories);
        }

        // PUT: api/Categories/5
        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCategories(int id, CategoriesVM category)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            if (!userSuperUserAuthorised)
            {
                    return BadRequest(new { message = "Not authorised to update categories" });
            }

            if (!CategoriesExists(id))
            {
                return NotFound(new { message = "Category Id not found, no changes made, please try again" });
            }

            int currentCategoryId = 0;

            try
            {
                var cat = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
                cat.Name = category.Name;
                await _context.SaveChangesAsync();
                currentCategoryId = cat.CategoryId;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriesExists(id))
                {
                    return NotFound(new { message = "Category Id not found, no changes made, please try again" });
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

            return Ok(new { message = "Category Updated - CategoryId:" + currentCategoryId });
        }

        // POST: api/Categories
        [EnableCors("AllowOrigin")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Category>> PostCategories(CategoriesVM category)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            if (!userSuperUserAuthorised)
            {
                    return BadRequest(new { message = "Not authorised to add categories" });
            }

            if (category.Name == "" || category.Name == null)
            {
                return BadRequest(new { message = "Cannot Add an empty category" });
            }

            Category newCategory = new Category();
            newCategory.Name = category.Name;
            int currentCategoryId = 0;

            try
            {
                _context.Categories.Add(newCategory);
                
                await _context.SaveChangesAsync();
                newCategory.Products = GetAllProductsByCategoryId(newCategory.CategoryId);
                await _context.SaveChangesAsync();
                currentCategoryId = newCategory.CategoryId;
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in adding Category, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error in adding Category, " + e.Message });
            }

            return Ok(new { meesage = "New Category Created - CategoryId:" + currentCategoryId });
        }

        // DELETE: api/Categories/5
        [EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Category>> DeleteCategories(int id)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
            if (!userSuperUserAuthorised)
            {
                    return BadRequest(new { message = "Not authorised to delete categories" });
            }

            var categories = await _context.Categories.FindAsync(id);
            if (categories == null)
            {
                return NotFound(new { message = "Category ID not found, please try again" });
            }

            if (_context.Products.Where(c => c.CategoryId == id).Count() > 0)
            {
                return BadRequest(new { message = "Error, Cannot delete Categories that have been assigned to a product" });
            }

            try
            {

                _context.Categories.Remove(categories);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in deleting Category, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error, " + e.Message });
            }
            return categories;
        }

        private bool CategoriesExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }


        private List<Product> GetAllProductsByCategoryId(int id)
        {
            List<Product> allProductsForCategory = new List<Product>();

            var productsQuery =
                    (from products in _context.Products
                     where (products.CategoryId == id)
                     select new
                     {
                         products.ProductId,
                         products.CategoryId,
                         products.Category,
                         products.Brand,
                         products.Name,
                         products.Description,
                         products.Price,
                         products.Reviews,
                         products.OrderDetails,
                         products.StockQuantity,
                         products.Wishlists,
                         products.ModifiedDate
                     }).ToList();


            foreach (var prod in productsQuery)
            {
                allProductsForCategory.Add(new Product()
                {
                    ProductId = prod.ProductId,
                    CategoryId = prod.CategoryId,
                    Category = prod.Category,
                    Brand = prod.Brand,
                    Name = prod.Name,
                    Description = prod.Description,
                    Price = prod.Price,
                    Reviews = prod.Reviews,
                    OrderDetails = prod.OrderDetails,
                    StockQuantity = prod.StockQuantity,
                    Wishlists = prod.Wishlists,
                    ModifiedDate = prod.ModifiedDate
                });
            }

            return allProductsForCategory;
        }

       
    }
}

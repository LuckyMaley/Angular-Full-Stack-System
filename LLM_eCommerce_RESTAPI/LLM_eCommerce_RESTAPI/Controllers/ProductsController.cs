using LLM_eCommerce_RESTAPI.AuthModels;
using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.Services;
using LLM_eCommerce_RESTAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace LLM_eCommerce_RESTAPI.Controllers
{
    /// <summary>
    /// A summary about ProductsController class.
    /// </summary>
    /// <remarks>
    /// ProductsController has the following end points:
    /// Get all Products
    /// Get Products with id
    /// Get Products with Name
    /// Put (update) Product with id and Product object
    /// Post (Add) Product using a Products View Model 
    /// Delete Product with id
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly LLM_eCommerce_EFDBContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AuthenticationContext _authenticationContext;
        private readonly IdentityHelper _identityHelper;
        public ProductsController(LLM_eCommerce_EFDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AuthenticationContext authenticationContext)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationContext = authenticationContext;
            _identityHelper = new IdentityHelper(_userManager, _authenticationContext, _roleManager);
        }


        // GET: api/Products        
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            
            var productDB = await _context.Products.ToListAsync();
            
            return Ok(productDB);
        }

        // GET: api/Products/5
        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProducts(int id)
        {
            List<Product> allProducts = new List<Product>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = await _context.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound(new { message = "No Product with that ID exists, please try again" });
            }
            else
            {
                products.Reviews = GetAllReviewsByProductId(id);
                products.Wishlists = GetAllWishlistsByProductId(id);
                products.OrderDetails = GetAllOrderDetailsByProductId(id);
                products.EfUserProducts = GetAllEFUserProductsByProductId(id);
                products.Category = _context.Categories.FirstOrDefault(c => c.CategoryId == products.CategoryId);
            }

            return Ok(products);
        }


        // GET: api/Products/specificProduct/name
        [EnableCors("AllowOrigin")]
        [HttpGet("specificProduct/{name}")]
        public async Task<ActionResult<List<Product>>> GetProductByName(string name)
        {
            List<Product> products = new List<Product>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var productsQuery = _context.Products.Where(x => x.Name == name);
            if(productsQuery.Count() == 0)
            {
                return NotFound(new { message = "No Product with that Name exists, please try again" });
            }
            var item = productsQuery;
            foreach (var productItem in item)
            {
                int id = productItem.ProductId;


                if (productItem == null)
                {

                    return NotFound(new { message = "No Product with that Name exists, please try again" });
                }
                else
                {
                    productItem.Reviews = GetAllReviewsByProductId(id);
                    productItem.Wishlists = GetAllWishlistsByProductId(id);
                    productItem.OrderDetails = GetAllOrderDetailsByProductId(id);
                    productItem.EfUserProducts = GetAllEFUserProductsByProductId(id);
                    productItem.Category = _context.Categories.FirstOrDefault(c => c.CategoryId == productItem.CategoryId);
                }

                products.Add(productItem);
            }
            return Ok(products);
        }

        // PUT: api/Products/5
        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProducts(int id, ProductsVM product)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
            if (!userSuperUserAuthorised && !userSellerAuthorised)
            {
                return BadRequest(new { message = "Not authorised to update products as a customer" });
            }

            

            var efUser = _context.EfUsers.FirstOrDefault(c => c.IdentityUsername == user.UserName);
            if (efUser != null && userSellerAuthorised)
            {
                if (_context.EfUserProducts.Where(c => c.EfUserId == efUser.EfUserId && c.ProductId == id).Count() == 0)
                {
                    return BadRequest(new { message = "Error, Seller can only update products they've added" });
                }
            }


            int currentProductId = 0;

            try
            {
                var updateProduct = _context.Products.FirstOrDefault(c => c.ProductId == id);
                int count = 0;
                if (updateProduct == null)
                {
                    return NotFound(new { message = "No Product with that ID exists, please try again" });
                }
                if (product.Name != "" || product.Name != null)
                {
                    if (updateProduct.Name != product.Name)
                    {
                        updateProduct.Name = product.Name;
                        count++;
                    }
                }
                if (product.Brand != "" || product.Brand != null)
                {
                    if (updateProduct.Brand != product.Brand)
                    {
                        updateProduct.Brand = product.Brand;
                        count++;
                    }
                }
                if (updateProduct.Description != "" || product.Description != null)
                {
                    if (updateProduct.Description != product.Description)
                    {
                        updateProduct.Description = product.Description;
                        count++;
                    }
                }
                if (product.CategoryId != 0)
                {
                    if (updateProduct.CategoryId != product.CategoryId)
                    {
                        if (_context.Categories.Where(c => c.CategoryId == product.CategoryId).Count() == 0)
                        {
                            return BadRequest(new { message = "That category does not exist please choose CategoryId included in the list below", _context.Categories });
                        }
                        updateProduct.CategoryId = product.CategoryId;
                        updateProduct.Category = _context.Categories.FirstOrDefault(c => c.CategoryId == product.CategoryId);
                        count++;
                    }
                }
                if (product.Type != "" || product.Type != null)
                {
                    if (updateProduct.Type != product.Type)
                    {
                        updateProduct.Type = product.Type;
                        count++;
                    }
                }
                if (product.Price != 0f)
                {
                    if (updateProduct.Price != product.Price)
                    {
                        updateProduct.Price = product.Price;
                        count++;
                    }
                }
                if (product.StockQuantity != 0)
                {
                    if (updateProduct.StockQuantity != product.StockQuantity)
                    {
                        updateProduct.StockQuantity = product.StockQuantity;
                        count++;
                    }
                }
				if (product.ImageUrl != "" || product.ImageUrl != null)
				{
					if (updateProduct.ImageUrl != product.ImageUrl)
					{
						updateProduct.ImageUrl = product.ImageUrl;
						count++;
					}
				}

				if (count > 0)
                {
                    updateProduct.ModifiedDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                    currentProductId = updateProduct.ProductId;
                }
                else
                {
                    return Ok(new { message = "no updates made" });
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return NotFound(new { message = "Product Id not found, no changes made, please try again" });
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

            return Ok(new { message = "Product Updated - ProductId:" + currentProductId });
        }

        // POST: api/Products
        [EnableCors("AllowOrigin")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Product>> PostProducts(ProductsVM product)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
            if (!userSuperUserAuthorised && !userSellerAuthorised)
            {
                return BadRequest(new { message = "Not authorised to add products as a customer" });
            }

            if (_context.Categories.Where(c => c.CategoryId == product.CategoryId).Count() == 0)
            {
                return BadRequest(new { message = "That category does not exist please choose CategoryId included in the list below", _context.Categories });
            }
            Product newProduct = new Product();
            newProduct.Name = product.Name;
            newProduct.Brand = product.Brand;
            newProduct.Description = product.Description;
            newProduct.CategoryId = product.CategoryId;
            newProduct.Type = product.Type;
            newProduct.Price = product.Price;
            newProduct.StockQuantity = product.StockQuantity;
            newProduct.ModifiedDate = DateTime.Now;
            newProduct.ImageUrl = product.ImageUrl;
            newProduct.Category = _context.Categories.FirstOrDefault(c => c.CategoryId == product.CategoryId);
            int currentProductId = 0;

            try
            {
                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
                newProduct.Reviews = GetAllReviewsByProductId(newProduct.ProductId);
                newProduct.Wishlists = GetAllWishlistsByProductId(newProduct.ProductId);
                newProduct.OrderDetails = GetAllOrderDetailsByProductId(newProduct.ProductId);
                newProduct.EfUserProducts = GetAllEFUserProductsByProductId(newProduct.ProductId);
                var efUserId = _context.EfUsers.FirstOrDefault(c => c.IdentityUsername == user.UserName).EfUserId;
                _context.EfUserProducts.Add(new EfUserProduct
                {
                    ProductId = _context.Products.Max(c => c.ProductId),
                    EfUserId = efUserId,
                    AddedDate = DateTime.Now
                });
                await _context.SaveChangesAsync();
                currentProductId = _context.Products.Max(c => c.ProductId);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in adding Product, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error in adding Product, " + e.Message });
            }

            return Ok(new { message = "New Product Created - ProductId:" + currentProductId });
        }

        // DELETE: api/Products/5
        [EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Product>> DeleteProducts(int id)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
            bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
            if (!userSuperUserAuthorised && !userSellerAuthorised)
            {
                return BadRequest(new { message = "Not authorised to delete products as a customer" });
            }

            var efUser = _context.EfUsers.FirstOrDefault(c => c.IdentityUsername == user.UserName);
            if (efUser != null && userSellerAuthorised)
            {
                if (_context.EfUserProducts.Where(c => c.EfUserId == efUser.EfUserId && c.ProductId == id).Count() == 0)
                {
                    
                    return BadRequest(new { message = "Error, Seller can only delete products they've added" });
                }
                
            }
            if (_context.OrderDetails.Where(c => c.ProductId == id).Count() > 0)
            {
                return BadRequest(new { message = "Error, Cannot delete Products that have been ordered" });
            }
            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound(new { message = "Product ID not found, please try again" });
            }

            try
            {

                _context.Products.Remove(products);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Error in deleting Product, please try again" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Error, " + e.Message });
            }
            return products;
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }


        private List<Review> GetAllReviewsByProductId(int id)
        {
            List<Review> allReviewsForProduct = new List<Review>();

            var reviewsQuery =
                    (from reviews in _context.Reviews
                     where (reviews.ProductId == id)
                     select new
                     {
                         reviews.ReviewId,
                         reviews.ProductId,
                         reviews.Rating,
                         reviews.Title,
                         reviews.Comment,
                         reviews.ReviewDate,
                         reviews.EfUser,
                         reviews.EfUserId
                     }).ToList();


            foreach (var rev in reviewsQuery)
            {
                allReviewsForProduct.Add(new Review()
                {
                    ReviewId = rev.ReviewId,
                    ProductId = rev.ProductId,
                    Rating = rev.Rating,
                    Title = rev.Title,
                    Comment = rev.Comment,
                    ReviewDate = rev.ReviewDate,
                    EfUser = rev.EfUser,
                    EfUserId = rev.EfUserId
                });
            }

            return allReviewsForProduct;
        }

        private List<OrderDetail> GetAllOrderDetailsByProductId(int id)
        {
            List<OrderDetail> allOrderDetailsForProduct = new List<OrderDetail>();

            var orderDetailsQuery =
                    (from orderDetails in _context.OrderDetails
                     where (orderDetails.ProductId == id)
                     select new
                     {
                         orderDetails.OrderDetailId,
                         orderDetails.OrderId,
                         orderDetails.ProductId,
                         orderDetails.Quantity,
                         orderDetails.UnitPrice,
                         orderDetails.Product,
                         orderDetails.Order
                     }).ToList();


            foreach (var ordd in orderDetailsQuery)
            {
                allOrderDetailsForProduct.Add(new OrderDetail()
                {
                    OrderDetailId = ordd.OrderDetailId,
                    OrderId = ordd.OrderId,
                    ProductId = ordd.ProductId,
                    Quantity = ordd.Quantity,
                    UnitPrice = ordd.UnitPrice,
                    Product = ordd.Product,
                    Order = ordd.Order
                });
            }

            return allOrderDetailsForProduct;
        }

        private List<Wishlist> GetAllWishlistsByProductId(int id)
        {
            List<Wishlist> allWishlistsForProduct = new List<Wishlist>();

            var wishlistsQuery =
                    (from wishlists in _context.Wishlists
                     where (wishlists.ProductId == id)
                     select new
                     {
                         wishlists.WishlistId,
                         wishlists.ProductId,
                         wishlists.EfUserId,
                         wishlists.AddedDate,
                         wishlists.Product,
                         wishlists.EfUser
                     }).ToList();


            foreach (var wish in wishlistsQuery)
            {
                allWishlistsForProduct.Add(new Wishlist()
                {
                    WishlistId = wish.WishlistId,
                    ProductId = wish.ProductId,
                    EfUserId = wish.EfUserId,
                    AddedDate = wish.AddedDate,
                    Product = wish.Product,
                    EfUser = wish.EfUser
                });
            }

            return allWishlistsForProduct;
        }

        private List<EfUserProduct> GetAllEFUserProductsByProductId(int id)
        {
            List<EfUserProduct> allEFUserProductsForProduct = new List<EfUserProduct>();

            var efUserProductsQuery =
                    (from efUserProducts in _context.EfUserProducts
                     where (efUserProducts.ProductId == id)
                     select new
                     {
                         efUserProducts.EfUserProductId,
                         efUserProducts.ProductId,
                         efUserProducts.EfUserId,
                         efUserProducts.AddedDate,
                         efUserProducts.Product,
                         efUserProducts.EfUser
                     }).ToList();


            foreach (var ef in efUserProductsQuery)
            {
                allEFUserProductsForProduct.Add(new EfUserProduct()
                {
                    EfUserProductId = ef.EfUserProductId,
                    ProductId = ef.ProductId,
                    EfUserId = ef.EfUserId,
                    AddedDate = ef.AddedDate,
                    Product = ef.Product,
                    EfUser = ef.EfUser
                });
            }

            return allEFUserProductsForProduct;
        }
    }
}

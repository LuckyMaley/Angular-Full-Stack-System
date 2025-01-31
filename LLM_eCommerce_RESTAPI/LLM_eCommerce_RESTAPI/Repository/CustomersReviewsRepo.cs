using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.ViewModels;
using Microsoft.CodeAnalysis;
using System.Xml.Linq;

namespace LLM_eCommerce_RESTAPI.Repository
{
    /// <summary>
    /// A summary about CustomersReviewsRepo class.
    /// </summary>
    /// <remarks>
    /// CustomersReviewsRepo has the following methods:
    /// Get current logged in user's reviews
    /// Get reviews with review id
    /// Get reviews with product id
    /// </remarks>
    public class CustomersReviewsRepo
    {
        private readonly LLM_eCommerce_EFDBContext _context;

        public CustomersReviewsRepo(LLM_eCommerce_EFDBContext context)
        {
            _context = context;
        }

        public virtual List<CustomersReviewsVM> GetCustomersReviews(int userId)
        {
            int paramId = userId;
            List<CustomersReviewsVM> customersReviews = new List<CustomersReviewsVM>();

            var customersReviewsQuery =
                (from efUsers in _context.EfUsers
                 join reviews in _context.Reviews
                 on efUsers.EfUserId equals reviews.EfUserId
                 join products in _context.Products
                 on reviews.ProductId equals products.ProductId
                 join categories in _context.Categories
                 on products.CategoryId equals categories.CategoryId
                 where ((paramId == 0 && efUsers.EfUserId == efUsers.EfUserId) || (efUsers.EfUserId == paramId))
                 orderby reviews.EfUserId, reviews.ReviewDate
                 select new
                 {
                     EfUserId = efUsers.EfUserId,
                     FirstName = efUsers.FirstName,
                     LastName = efUsers.LastName,
                     Email = efUsers.Email,
                     Address = efUsers.Address,
                     PhoneNumber = efUsers.PhoneNumber,
                     IdentityUsername = efUsers.IdentityUsername,
                     Role = efUsers.Role,
                     ReviewId = reviews.ReviewId,
                     ProductId = products.ProductId,
                     Name = products.Name,
                     Brand = products.Brand,
                     Description = products.Description,
                     Type = products.Type,
                     Price = products.Price,
                     CategoryId = categories.CategoryId,
                     CategoryName = categories.Name,
                     StockQuantity = products.StockQuantity,
                     ModifiedDate = products.ModifiedDate,
                     Rating = reviews.Rating,
                     Title = reviews.Title,
                     Comment = reviews.Comment,
                     ReviewDate = reviews.ReviewDate
                 }).ToList();

            foreach (var cust in customersReviewsQuery)
            {
                customersReviews.Add(new CustomersReviewsVM()
                {
                    EfUserId = cust.EfUserId,
                    FirstName = cust.FirstName,
                    LastName = cust.LastName,
                    Email = cust.Email,
                    Address = cust.Address,
                    PhoneNumber = cust.PhoneNumber,
                    IdentityUsername = cust.IdentityUsername,
                    Role = cust.Role,
                    ReviewId = cust.ReviewId,
                    ProductId = cust.ProductId,
                    Name = cust.Name,
                    Brand = cust.Brand,
                    Description = cust.Description,
                    Type = cust.Type,
                    Price = cust.Price,
                    CategoryId = cust.CategoryId,
                    CategoryName = cust.CategoryName,
                    StockQuantity = cust.StockQuantity,
                    ModifiedDate = cust.ModifiedDate,
                    Rating = cust.Rating,
                    Title = cust.Title,
                    Comment = cust.Comment,
                    ReviewDate = cust.ReviewDate
                });
            }

            return customersReviews;
        }



        public virtual CustomersReviewsVM GetReviewDetails(int reviewId)
        {
            int paramId = reviewId;
            CustomersReviewsVM customersReviews = new CustomersReviewsVM();

            var customersReviewsQuery =
               (from efUsers in _context.EfUsers
                join reviews in _context.Reviews
                on efUsers.EfUserId equals reviews.EfUserId
                join products in _context.Products
                on reviews.ProductId equals products.ProductId
                join categories in _context.Categories
                on products.CategoryId equals categories.CategoryId
                where (reviews.ReviewId == paramId)
                orderby reviews.EfUserId, reviews.ReviewDate
                select new
                {
                    EfUserId = efUsers.EfUserId,
                    FirstName = efUsers.FirstName,
                    LastName = efUsers.LastName,
                    Email = efUsers.Email,
                    Address = efUsers.Address,
                    PhoneNumber = efUsers.PhoneNumber,
                    IdentityUsername = efUsers.IdentityUsername,
                    Role = efUsers.Role,
                    ReviewId = reviews.ReviewId,
                    ProductId = products.ProductId,
                    Name = products.Name,
                    Brand = products.Brand,
                    Description = products.Description,
                    Type = products.Type,
                    Price = products.Price,
                    CategoryId = categories.CategoryId,
                    CategoryName = categories.Name,
                    StockQuantity = products.StockQuantity,
                    ModifiedDate = products.ModifiedDate,
                    Rating = reviews.Rating,
                    Title = reviews.Title,
                    Comment = reviews.Comment,
                    ReviewDate = reviews.ReviewDate
                }).ToList();

            foreach (var cust in customersReviewsQuery)
            {
                customersReviews.EfUserId = cust.EfUserId;
                customersReviews.FirstName = cust.FirstName;
                customersReviews.LastName = cust.LastName;
                customersReviews.Email = cust.Email;
                customersReviews.Address = cust.Address;
                customersReviews.PhoneNumber = cust.PhoneNumber;
                customersReviews.IdentityUsername = cust.IdentityUsername;
                customersReviews.Role = cust.Role;
                customersReviews.ReviewId = cust.ReviewId;
                customersReviews.ProductId = cust.ProductId;
                customersReviews.Name = cust.Name;
                customersReviews.Brand = cust.Brand;
                customersReviews.Description = cust.Description;
                customersReviews.Type = cust.Type;
                customersReviews.Price = cust.Price;
                customersReviews.CategoryId = cust.CategoryId;
                customersReviews.CategoryName = cust.Name;
                customersReviews.StockQuantity = cust.StockQuantity;
                customersReviews.ModifiedDate = cust.ModifiedDate;
                customersReviews.Rating = cust.Rating;
                customersReviews.Title = cust.Title;
                customersReviews.Comment = cust.Comment;
                customersReviews.ReviewDate = cust.ReviewDate;
            }

            return customersReviews;
        }


        public virtual List<CustomersReviewsVM> GetReviewsByProductId(int productId)
        {
            int paramId = productId;
            List<CustomersReviewsVM> customersReviews = new List<CustomersReviewsVM>();

            var customersReviewsQuery =
                (from efUsers in _context.EfUsers
                 join reviews in _context.Reviews
                 on efUsers.EfUserId equals reviews.EfUserId
                 join products in _context.Products
                 on reviews.ProductId equals products.ProductId
                 join categories in _context.Categories
                 on products.CategoryId equals categories.CategoryId
                 where (products.ProductId == paramId)
                 orderby reviews.ReviewDate, reviews.Rating descending
                 select new
                 {
                     EfUserId = efUsers.EfUserId,
                     FirstName = efUsers.FirstName,
                     LastName = efUsers.LastName,
                     Email = efUsers.Email,
                     Address = efUsers.Address,
                     PhoneNumber = efUsers.PhoneNumber,
                     IdentityUsername = efUsers.IdentityUsername,
                     Role = efUsers.Role,
                     ReviewId = reviews.ReviewId,
                     ProductId = products.ProductId,
                     Name = products.Name,
                     Brand = products.Brand,
                     Description = products.Description,
                     Type = products.Type,
                     Price = products.Price,
                     CategoryId = categories.CategoryId,
                     CategoryName = categories.Name,
                     StockQuantity = products.StockQuantity,
                     ModifiedDate = products.ModifiedDate,
                     Rating = reviews.Rating,
                     Title = reviews.Title,
                     Comment = reviews.Comment,
                     ReviewDate = reviews.ReviewDate
                 }).ToList();

            foreach (var cust in customersReviewsQuery)
            {
                customersReviews.Add(new CustomersReviewsVM()
                {
                    EfUserId = cust.EfUserId,
                    FirstName = cust.FirstName,
                    LastName = cust.LastName,
                    Email = cust.Email,
                    Address = cust.Address,
                    PhoneNumber = cust.PhoneNumber,
                    IdentityUsername = cust.IdentityUsername,
                    Role = cust.Role,
                    ReviewId = cust.ReviewId,
                    ProductId = cust.ProductId,
                    Name = cust.Name,
                    Brand = cust.Brand,
                    Description = cust.Description,
                    Type = cust.Type,
                    Price = cust.Price,
                    CategoryId = cust.CategoryId,
                    CategoryName = cust.CategoryName,
                    StockQuantity = cust.StockQuantity,
                    ModifiedDate = cust.ModifiedDate,
                    Rating = cust.Rating,
                    Title = cust.Title,
                    Comment = cust.Comment,
                    ReviewDate = cust.ReviewDate
                });
            }

            return customersReviews;
        }
    }
}

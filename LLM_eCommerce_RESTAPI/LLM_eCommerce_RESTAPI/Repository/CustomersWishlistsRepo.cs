using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.ViewModels;

namespace LLM_eCommerce_RESTAPI.Repository
{
    /// <summary>
    /// A summary about CustomersWishlistssRepo class.
    /// </summary>
    /// <remarks>
    /// CustomersWishlistsRepo has the following methods:
    /// Get current logged in user's wishlists
    /// Get wishlists with wishlist id
    /// Get wishlists with product id
    /// </remarks>
    public class CustomersWishlistsRepo
    {
        private readonly LLM_eCommerce_EFDBContext _context;

        public CustomersWishlistsRepo(LLM_eCommerce_EFDBContext context)
        {
            _context = context;
        }

        public virtual List<CustomersWishlistsVM> GetCustomersWishlists(int userId)
        {
            int paramId = userId;
            List<CustomersWishlistsVM> customersWishlists = new List<CustomersWishlistsVM>();

            var customersWishlistsQuery =
                (from efUsers in _context.EfUsers
                 join wishlists in _context.Wishlists
                 on efUsers.EfUserId equals wishlists.EfUserId
                 join products in _context.Products
                 on wishlists.ProductId equals products.ProductId
                 join categories in _context.Categories
                 on products.CategoryId equals categories.CategoryId
                 where ((paramId == 0 && efUsers.EfUserId == efUsers.EfUserId) || (efUsers.EfUserId == paramId))
                 orderby wishlists.EfUserId, wishlists.AddedDate
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
                     WishlistId = wishlists.WishlistId,
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
                     AddedDate = wishlists.AddedDate
    }).ToList();

            foreach (var cust in customersWishlistsQuery)
            {
                customersWishlists.Add(new CustomersWishlistsVM()
                {
                    EfUserId = cust.EfUserId,
                    FirstName = cust.FirstName,
                    LastName = cust.LastName,
                    Email = cust.Email,
                    Address = cust.Address,
                    PhoneNumber = cust.PhoneNumber,
                    IdentityUsername = cust.IdentityUsername,
                    Role = cust.Role,
                    WishlistId = cust.WishlistId,
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
                    AddedDate = cust.AddedDate
                });
            }

            return customersWishlists;
        }



        public virtual CustomersWishlistsVM GetWishlistDetails(int wishlistId)
        {
            int paramId = wishlistId;
            CustomersWishlistsVM customersWishlists = new CustomersWishlistsVM();

            var customersWishlistsQuery =
               (from efUsers in _context.EfUsers
                join wishlists in _context.Wishlists
                on efUsers.EfUserId equals wishlists.EfUserId
                join products in _context.Products
                on wishlists.ProductId equals products.ProductId
                join categories in _context.Categories
                on products.CategoryId equals categories.CategoryId
                where (wishlists.WishlistId == paramId)
                orderby wishlists.EfUserId, wishlists.AddedDate
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
                    WishlistId = wishlists.WishlistId,
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
                    AddedDate = wishlists.AddedDate
                }).ToList();

            foreach (var cust in customersWishlistsQuery)
            {
                customersWishlists.EfUserId = cust.EfUserId;
                customersWishlists.FirstName = cust.FirstName;
                customersWishlists.LastName = cust.LastName;
                customersWishlists.Email = cust.Email;
                customersWishlists.Address = cust.Address;
                customersWishlists.PhoneNumber = cust.PhoneNumber;
                customersWishlists.IdentityUsername = cust.IdentityUsername;
                customersWishlists.Role = cust.Role;
                customersWishlists.WishlistId = cust.WishlistId;
                customersWishlists.ProductId = cust.ProductId;
                customersWishlists.Name = cust.Name;
                customersWishlists.Brand = cust.Brand;
                customersWishlists.Description = cust.Description;
                customersWishlists.Type = cust.Type;
                customersWishlists.Price = cust.Price;
                customersWishlists.CategoryId = cust.CategoryId;
                customersWishlists.CategoryName = cust.Name;
                customersWishlists.StockQuantity = cust.StockQuantity;
                customersWishlists.ModifiedDate = cust.ModifiedDate;
                customersWishlists.AddedDate = cust.AddedDate;
            }

            return customersWishlists;
        }


        public virtual List<CustomersWishlistsVM> GetWishlistsByProductId(int productId)
        {
            int paramId = productId;
            List<CustomersWishlistsVM> customersWishlists = new List<CustomersWishlistsVM>();

            var customersWishlistsQuery =
                (from efUsers in _context.EfUsers
                 join wishlists in _context.Wishlists
                 on efUsers.EfUserId equals wishlists.EfUserId
                 join products in _context.Products
                 on wishlists.ProductId equals products.ProductId
                 join categories in _context.Categories
                 on products.CategoryId equals categories.CategoryId
                 where (products.ProductId == paramId)
                 orderby wishlists.AddedDate, products.Price descending
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
                     WishlistId = wishlists.WishlistId,
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
                     AddedDate = wishlists.AddedDate
                 }).ToList();

            foreach (var cust in customersWishlistsQuery)
            {
                customersWishlists.Add(new CustomersWishlistsVM()
                {
                    EfUserId = cust.EfUserId,
                    FirstName = cust.FirstName,
                    LastName = cust.LastName,
                    Email = cust.Email,
                    Address = cust.Address,
                    PhoneNumber = cust.PhoneNumber,
                    IdentityUsername = cust.IdentityUsername,
                    Role = cust.Role,
                    WishlistId = cust.WishlistId,
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
                    AddedDate = cust.AddedDate
                });
            }

            return customersWishlists;
        }
    }
}

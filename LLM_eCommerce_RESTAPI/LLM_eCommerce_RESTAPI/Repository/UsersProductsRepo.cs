using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.ViewModels;

namespace LLM_eCommerce_RESTAPI.Repository
{
    /// <summary>
    /// A summary about UsersProductsRepo class.
    /// </summary>
    /// <remarks>
    /// UsersProductsRepo has the following methods:
    /// Get current logged in user's products
    /// Get a ProductDetails with Product id
    /// Get a ProductDetails with User Product id
    /// </remarks>
    public class UsersProductsRepo
    {
        private readonly LLM_eCommerce_EFDBContext _context;

        public UsersProductsRepo(LLM_eCommerce_EFDBContext context)
        {
            _context = context;
        }

        public virtual List<UsersProductsVM> GetUsersProducts(int userId)
        {
            int paramId = userId;
            List<UsersProductsVM> usersProducts = new List<UsersProductsVM>();

            var usersProductsquery =
                (from products in _context.Products
                 join userProduct in _context.EfUserProducts
                 on products.ProductId equals userProduct.ProductId
                 join efUsers in _context.EfUsers
                 on userProduct.EfUserId equals efUsers.EfUserId
                 where ((paramId == 0 && efUsers.EfUserId == userProduct.EfUserId) || (efUsers.EfUserId == paramId))
                 orderby userProduct.EfUserId, userProduct.ProductId
                 select new
                 {
                     EfUserId = userProduct.EfUserId,
                     FirstName = efUsers.FirstName,
                     LastName = efUsers.LastName,
                     Email = efUsers.Email,
                     Address = efUsers.Address,
                     PhoneNumber = efUsers.PhoneNumber,
                     IdentityUsername = efUsers.IdentityUsername,
                     Role = efUsers.Role,
                     ProductId = userProduct.ProductId,
                     Name = products.Name,
                     Brand = products.Brand,
                     Description = products.Description,
                     Type = products.Type,
                     Price = products.Price,
                     CategoryId = products.CategoryId,
                     StockQuantity = products.StockQuantity,
                     ModifiedDate = products.ModifiedDate,
                     ImageUrl = products.ImageUrl
                 }).ToList();

            foreach (var usps in usersProductsquery)
            {
                usersProducts.Add(new UsersProductsVM()
                {
                    EfUserId = usps.EfUserId,
                    FirstName = usps.FirstName,
                    LastName = usps.LastName,
                    Email = usps.Email,
                    Address = usps.Address,
                    PhoneNumber = usps.PhoneNumber,
                    IdentityUsername = usps.IdentityUsername,
                    Role = usps.Role,
                    ProductId = usps.ProductId,
                    Name = usps.Name,
                    Brand = usps.Brand,
                    Description = usps.Description,
                    Type = usps.Type,
                    Price = usps.Price,
                    CategoryId = usps.CategoryId,
                    StockQuantity = usps.StockQuantity,
                    ModifiedDate = usps.ModifiedDate,
                    ImageUrl = usps.ImageUrl
                });
            }

            return usersProducts;
        }

        public virtual List<UsersProductsOrdersVM> GetUsersProductsOrders(int userId)
        {
            List<UsersProductsOrdersVM> usersProductsOrders = new List<UsersProductsOrdersVM>();
            List<int> prods = new List<int>();
            foreach (var Id in _context.EfUserProducts.Where(c => c.EfUserId == userId))
            {
                prods.Add(Id.ProductId);
            }
            HashSet<int> orderItems = new HashSet<int>();
            foreach (var ids in prods)
            {
                foreach(var item in _context.OrderDetails.Where(c => c.ProductId == ids))
                {
                    orderItems.Add(item.OrderId);
                }
            }

            foreach (var Id in orderItems)
            {
                int paramId = _context.Orders.FirstOrDefault(c => c.OrderId == Id).EfUserId;
                var usersProductsOrdersquery =
                    (from efUsers in _context.EfUsers
                     join orders in _context.Orders
                     on efUsers.EfUserId equals orders.EfUserId
                     join shippings in _context.Shippings
                     on orders.ShippingId equals shippings.ShippingId
                     where ((paramId == 0 && efUsers.EfUserId == efUsers.EfUserId) || (efUsers.EfUserId == paramId))
                     orderby orders.EfUserId, orders.OrderDate
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
                         OrderId = orders.OrderId,
                         ShippingId = shippings.ShippingId,
                         ShippingDate = shippings.ShippingDate,
                         ShippingAddress = shippings.ShippingAddress,
                         ShippingMethod = shippings.ShippingMethod,
                         TrackingNumber = shippings.TrackingNumber,
                         DeliveryStatus = shippings.DeliveryStatus,
                         OrderDate = orders.OrderDate,
                         TotalAmount = orders.TotalAmount
                     }).ToList();

                foreach (var usps in usersProductsOrdersquery)
                {
                    usersProductsOrders.Add(new UsersProductsOrdersVM()
                    {
                        EfUserId = usps.EfUserId,
                        FirstName = usps.FirstName,
                        LastName = usps.LastName,
                        Email = usps.Email,
                        Address = usps.Address,
                        PhoneNumber = usps.PhoneNumber,
                        IdentityUsername = usps.IdentityUsername,
                        Role = usps.Role,
                        OrderId = usps.OrderId,
                        ShippingId = usps.ShippingId,
                        ShippingDate = usps.ShippingDate,
                        ShippingAddress = usps.ShippingAddress,
                        ShippingMethod = usps.ShippingMethod,
                        TrackingNumber = usps.TrackingNumber,
                        DeliveryStatus = usps.DeliveryStatus,
                        OrderDate = usps.OrderDate,
                        TotalAmount = usps.TotalAmount
                    });
                }
            }
            return usersProductsOrders;
        }

        public virtual UsersProductsVM GetProductDetails(int productId)
        {
            int paramId = productId;
            UsersProductsVM usersProducts = new UsersProductsVM();

            var usersProductsquery =
                (from products in _context.Products
                 join userProduct in _context.EfUserProducts
                 on products.ProductId equals userProduct.ProductId
                 join efUsers in _context.EfUsers
                 on userProduct.EfUserId equals efUsers.EfUserId
                 where (products.ProductId == paramId)
                 orderby userProduct.EfUserId, userProduct.ProductId
                 select new
                 {
                     EfUserId = userProduct.EfUserId,
                     FirstName = efUsers.FirstName,
                     LastName = efUsers.LastName,
                     Email = efUsers.Email,
                     Address = efUsers.Address,
                     PhoneNumber = efUsers.PhoneNumber,
                     IdentityUsername = efUsers.IdentityUsername,
                     Role = efUsers.Role,
                     ProductId = userProduct.ProductId,
                     Name = products.Name,
                     Brand = products.Brand,
                     Description = products.Description,
                     Type = products.Type,
                     Price = products.Price,
                     CategoryId = products.CategoryId,
                     StockQuantity = products.StockQuantity,
                     ModifiedDate = products.ModifiedDate,
                     ImageUrl = products.ImageUrl
                 }).ToList();

            foreach (var usps in usersProductsquery)
            {
                usersProducts.EfUserId = usps.EfUserId;
                usersProducts.FirstName = usps.FirstName;
                usersProducts.LastName = usps.LastName;
                usersProducts.Email = usps.Email;
                usersProducts.Address = usps.Address;
                usersProducts.PhoneNumber = usps.PhoneNumber;
                usersProducts.IdentityUsername = usps.IdentityUsername;
                usersProducts.Role = usps.Role;
                usersProducts.ProductId = usps.ProductId;
                usersProducts.Name = usps.Name;
                usersProducts.Brand = usps.Brand;
                usersProducts.Description = usps.Description;
                usersProducts.Type = usps.Type;
                usersProducts.Price = usps.Price;
                usersProducts.CategoryId = usps.CategoryId;
                usersProducts.StockQuantity = usps.StockQuantity;
                usersProducts.ModifiedDate = usps.ModifiedDate;
                usersProducts.ImageUrl = usps.ImageUrl;
            }
            
            return usersProducts;
        }


        public virtual List<UsersProductsVM> GetProductsByUserProductId(int userProductId)
        {
            int paramId = userProductId;
            List<UsersProductsVM> usersProducts = new List<UsersProductsVM>();

            var usersProductsquery =
                (from products in _context.Products
                 join userProduct in _context.EfUserProducts
                 on products.ProductId equals userProduct.ProductId
                 join efUsers in _context.EfUsers
                 on userProduct.EfUserId equals efUsers.EfUserId
                 where (userProduct.EfUserProductId == paramId)
                 orderby products.Price, userProduct.EfUserId descending
                 select new
                 {
                     EfUserId = userProduct.EfUserId,
                     FirstName = efUsers.FirstName,
                     LastName = efUsers.LastName,
                     Email = efUsers.Email,
                     Address = efUsers.Address,
                     PhoneNumber = efUsers.PhoneNumber,
                     IdentityUsername = efUsers.IdentityUsername,
                     Role = efUsers.Role,
                     ProductId = userProduct.ProductId,
                     Name = products.Name,
                     Brand = products.Brand,
                     Description = products.Description,
                     Type = products.Type,
                     Price = products.Price,
                     CategoryId = products.CategoryId,
                     StockQuantity = products.StockQuantity,
                     ModifiedDate = products.ModifiedDate,
                     ImageUrl = products.ImageUrl
                 }).ToList();

            foreach (var usps in usersProductsquery)
            {
                usersProducts.Add(new UsersProductsVM()
                {
                    EfUserId = usps.EfUserId,
                    FirstName = usps.FirstName,
                    LastName = usps.LastName,
                    Email = usps.Email,
                    Address = usps.Address,
                    PhoneNumber = usps.PhoneNumber,
                    IdentityUsername = usps.IdentityUsername,
                    Role = usps.Role,
                    ProductId = usps.ProductId,
                    Name = usps.Name,
                    Brand = usps.Brand,
                    Description = usps.Description,
                    Type = usps.Type,
                    Price = usps.Price,
                    CategoryId = usps.CategoryId,
                    StockQuantity = usps.StockQuantity,
                    ModifiedDate = usps.ModifiedDate,
                    ImageUrl = usps.ImageUrl
                });
            }

            return usersProducts;
        }
    }
}

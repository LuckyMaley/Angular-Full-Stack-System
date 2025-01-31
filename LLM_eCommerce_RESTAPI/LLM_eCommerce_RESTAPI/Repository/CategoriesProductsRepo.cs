using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.ViewModels;

namespace LLM_eCommerce_RESTAPI.Repository
{
    /// <summary>
    /// A summary about CategoriesProductsRepo class.
    /// </summary>
    /// <remarks>
    /// CategoriesProductsRepo has the following methods to get information from DB using EF:
    /// Get all CategoriesProducts
    /// Get CategoriesProducts with id
    /// Get All Categories with product information
    /// </remarks>
    public class CategoriesProductsRepo
    {
        private readonly LLM_eCommerce_EFDBContext _context;

        public CategoriesProductsRepo(LLM_eCommerce_EFDBContext context)
        {
            _context = context;
        }

        public virtual List<Category> GetAllCategories()
        {
            List<Category> allCategories = _context.Categories.ToList();

            return allCategories;
        }

        public virtual Category GetCategoriesWithId(int id)
        {
            var categories = _context.Categories.Find(id);

            return categories;
        }

        public virtual List<CategoriesProductsVM> GetAllCategoriesProducts()
        {
            List<CategoriesProductsVM> allCategoriesProducts = new List<CategoriesProductsVM>();

            var categoriesProductsQuery =
                (from categories in _context.Categories
                 join products in _context.Products
                 on categories.CategoryId equals products.CategoryId
                 select new
                 {
                     CategoryId = categories.CategoryId,
                     CategoryName = categories.Name,
                     ProductId = products.ProductId,
                     Name = products.Name,
                     Brand = products.Brand,
                     Description = products.Description,
                     Type = products.Type,
                     Price = products.Price,
                     StockQuantity = products.StockQuantity,
                     ModifiedDate = products.ModifiedDate,
                     ImageUrl = products.ImageUrl,
                 }).ToList();

            foreach (var catProd in categoriesProductsQuery)
            {
                allCategoriesProducts.Add(new CategoriesProductsVM()
                {
                    CategoryId = catProd.CategoryId,
                    CategoryName = catProd.Name,
                    ProductId = catProd.ProductId,
                    Name = catProd.Name,
                    Brand = catProd.Brand,
                    Description = catProd.Description,
                    Type = catProd.Type,
                    Price = catProd.Price,
                    StockQuantity = catProd.StockQuantity,
                    ModifiedDate = catProd.ModifiedDate,
                    ImageUrl = catProd.ImageUrl
                });

            }

            return allCategoriesProducts;
        }
    }
}

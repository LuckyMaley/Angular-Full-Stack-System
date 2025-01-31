using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.Repository;
using LLM_eCommerce_RESTAPI.ViewModels;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLM_eCommerce_RESTAPI.Controllers
{
    /// <summary>
    /// A summary about CategoriesProductsController class.
    /// </summary>
    /// <remarks>
    /// CategoriesController has the following end points:
    /// Get all Categories
    /// Get a Categorie with id
    /// Get All Categories with Product information
    /// Given an Id will get a single Category with Product information
    /// Using CategoriesProductsRepo
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesProductsController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger("CategoriesProductsController");

        private readonly LLM_eCommerce_EFDBContext _context;
        CategoriesProductsRepo _repo;

        public CategoriesProductsController(LLM_eCommerce_EFDBContext context)
        {
            _context = context;
            _repo = new CategoriesProductsRepo(_context);
        }

        //GET:  api/Categorys
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            logger.Info("CategoriesController - GET:  api/Categories");

            List<Category> allCat = new List<Category>();
            allCat = _repo.GetAllCategories();
            return Ok(allCat);
        }

        //GET:  api/Categories/2
        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            logger.Info("CategoriesController - GET:  api/Categories/" + id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var theCat = _repo.GetCategoriesWithId(id);
            return Ok(theCat);
        }


        [Route("All")]
        //GET:  api/CategoriesProducts/All
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriesProductsVM>>> GetAllCategorieWithProducts()
        {
            logger.Info("CategoriesProductsController - GET: api/CategoriesProducts/All");

            List<CategoriesProductsVM> allCatProd = new List<CategoriesProductsVM>();
            allCatProd = _repo.GetAllCategoriesProducts();
            return allCatProd;
        }

        //GET:  api/CategoriesProducts/Prod/2
        [EnableCors("AllowOrigin")]
        [HttpGet("Prod/{id}")]
        public async Task<ActionResult<CategoriesProductsVM>> GetOneCategoryWithProduct(int id)
        {
            logger.Info("CategoriesProductController - GET:  api/CategoriesProducts/Prod/" + id);

            CategoriesProductsVM theCategoryProduct = new CategoriesProductsVM();

            List<CategoriesProductsVM> allCatProd = new List<CategoriesProductsVM>();
            allCatProd = _repo.GetAllCategoriesProducts();

            var catProdQuery = allCatProd.Where(x => x.ProductId == id);
            theCategoryProduct = catProdQuery.First();

            return theCategoryProduct;
        }
    }
}

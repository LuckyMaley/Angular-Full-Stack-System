using LLM_eCommerce_RESTAPI.AuthModels;
using LLM_eCommerce_RESTAPI.Controllers;
using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using NUnit.Framework;
using System.IO.Pipelines;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RESTApi.NunitTests
{
    [TestFixture]
    public class ProductsControllerTests
    {
        private LLM_eCommerce_EFDBContext _eCommerceContext;
        private ProductsController _controllerUnderTest;
        private List<Product> _productList;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AuthenticationContext _authenticationContext;
        ProductsVM _productVM;
        Product _product;
        IdentityUser identityUser;
        ClaimsPrincipal principal;

        [SetUp]
        public void Initialiser()
        {
            _eCommerceContext = (LLM_eCommerce_EFDBContext)InMemoryContext.GeneratedDB();
            var prod = _eCommerceContext.Products.Count();
            _authenticationContext = (AuthenticationContext)InMemoryContext.GeneratedAuthDB();
            _userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(_authenticationContext), null, null, null, null, null, null, null, null); 

            _roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(_authenticationContext), null, null, null, null);
            var authdb = _authenticationContext;
            identityUser = authdb.ApplicationUsers.First();
            var user = new ApplicationUser { Id = identityUser.Id };
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("UserID", user.Id),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthentication");
            principal = new ClaimsPrincipal(identity);
            _controllerUnderTest = new ProductsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            _productList = new List<Product>();
            _product = new Product()
            {
                ProductId = 243,
                Name = "Nike Air Force 2",
                Brand = "Nike",
                Description = "Nike Air Force 2 sneakers for track wear",
                Type = "Men",
                Price = 2100.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2018, 11, 12, 12, 45, 00)
            };
            _productVM = new ProductsVM()
            {
                Name = _product.Name,
                Brand = _product.Brand,
                Description = _product.Description,
                Type = _product.Type,
                Price = _product.Price,
                CategoryId = _product.CategoryId,
                StockQuantity = _product.StockQuantity
            };

        }



        [TearDown]
        public void CleanUpObject()
        {
            _eCommerceContext.Database.EnsureDeleted();
            _controllerUnderTest = null;
            _productList = null;
            _product = null;
            _userManager = null;

            _roleManager = null;
            _authenticationContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task _01Test_GetAllProduct_ReturnsListWithValidCount0()
        {
            // Arrange
           

            // Act
            var result = await _controllerUnderTest.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var productList = okResult.Value as List<Product>;
            Assert.NotNull(productList);
            Assert.AreEqual(10, productList.Count);
        }

        [Test]
        public async Task _02Test_GetAllProduct_ReturnsListWithValidCountEqualTo11()
        {
            // Arrange
            _eCommerceContext.Products.Add(_product);
             await _eCommerceContext.SaveChangesAsync();


            _controllerUnderTest = new ProductsController(_eCommerceContext, _userManager, _roleManager, (AuthenticationContext)_authenticationContext);


            // Act
            var result = await _controllerUnderTest.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var productList = okResult.Value as List<Product>;
            Assert.NotNull(productList);
            Assert.AreEqual(11, productList.Count);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        public async Task _03Test_GetProductByID_ReturnsaListwithCount1_WhenCorrectProductIDEntered(int idPassedIn)
        {
            //Arrange
            int id = idPassedIn;

            //Act            
            var result = await _controllerUnderTest.GetProducts(id);
            var okResult = (OkObjectResult)result.Result;
            var actual = (Product)okResult.Value;
            var expected = _eCommerceContext.Products.FirstOrDefault(c => c.ProductId == id);
            
            //Assert
            Assert.IsInstanceOf<ActionResult<Product>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.AreEqual(actual.ProductId, expected.ProductId);

        }


        [TestCase("Nike Air Force 1")]
        [TestCase("Adidas Yeezy")]
        [TestCase("Puma T-shirt")]
        [TestCase("Reebok T-shirt")]
        [TestCase("Converse Allstar")]
        public async Task _04Test_GetProductByName_ReturnsaListwithCount1_WhenCorrectProductIDEntered(string namePassedIn)
        {
            //Arrange
            string name = namePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetProductByName(name);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Product>)okResult.Value;
            var expected = _eCommerceContext.Products.FirstOrDefault(c => c.Name == name);

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Product>>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.AreEqual(actual.FirstOrDefault(c => c.Name == name).ProductId, expected.ProductId);

        }

        [TestCase("Nike Air Loss 1")]
        [TestCase("Adidas Drizzy")]
        [TestCase("Puma See See")]
        [TestCase("Reebok Lost")]
        [TestCase("Converse Nostar")]
        public async Task _05Test_GetProductByName_ReturnsNotFound_WhenIncorrectProductNameEntered(string namePassedIn)
        {
            //Arrange
            string name = namePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetProductByName(name);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = notFoundResult.Value.ToString();
            var expected = "No Product with that Name exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }

        [Test]
        public async Task _06Test_GetProductByID_ReturnsaBackActionResult_WhenProductIDEnteredDoesNotExist()
        {
            //Arrange
            int id = 20;

            //Act            
            var result = await _controllerUnderTest.GetProducts(id);
            var badResult = (NotFoundObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "No Product with that ID exists, please try again";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 404);

        }

        [Test]
        public async Task _07Test_PostAproduct_ReturnsBadRequest_WhenEmptyProductAdded()
        {
            //Arrange 

            //Act            
            var result = await _controllerUnderTest.PostProducts(new ProductsVM());
            var badResult = (BadRequestObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "That category does not exist";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.AreEqual(10, _eCommerceContext.Products.Count());

        }

        [Test]
        public async Task _08Test_PutProducts_ReturnsOkObjectResult()
        {
            //Arrange 
            var product1 = new Product();
            product1.Name = "Nike Air 4";
            ProductsVM productsVM1 = new ProductsVM()
            {
                Name = product1.Name,
                Brand = product1.Brand,
                Description = product1.Description,
                Price = product1.Price,
                CategoryId = product1.CategoryId,
                StockQuantity = product1.StockQuantity,
                Type = product1.Type
            };

            var product2 = new Product();
            product2.Name = "Adidas Yeezy New Style 4";
            ProductsVM productsVM2 = new ProductsVM()
            {
                Name = product2.Name,
                Brand = product2.Brand,
                Description = product2.Description,
                Price = product2.Price,
                CategoryId = product2.CategoryId,
                StockQuantity = product2.StockQuantity,
                Type = product2.Type
            };

            var product3 = new Product();
            product3.Name = "Adidas Yeezy Black Slides 4";
            ProductsVM productsVM3 = new ProductsVM()
            {
                Name = product3.Name,
                Brand = product3.Brand,
                Description = product3.Description,
                Price = product3.Price,
                CategoryId = product3.CategoryId,
                StockQuantity = product3.StockQuantity,
                Type = product3.Type
            };


            //Act            
            var result1 = await _controllerUnderTest.PutProducts(1, productsVM1);
            var result2 = await _controllerUnderTest.PutProducts(2, productsVM2);
            var result3 = await _controllerUnderTest.PutProducts(3, productsVM3);

            var okResult1 = (OkObjectResult)result1;
            var actual1 = okResult1.Value.ToString();
            var expected1 = "Product Updated";

            var okResult2 = (OkObjectResult)result2;
            var actual2 = okResult2.Value.ToString();
            var expected2 = "Product Updated";

            var okResult3 = (OkObjectResult)result3;
            var actual3 = okResult3.Value.ToString();
            var expected3 = "Product Updated";

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result1);
            Assert.AreEqual(okResult1.StatusCode, 200);
            Assert.IsTrue(actual1.Contains(expected1));

            Assert.IsInstanceOf<OkObjectResult>(result2);
            Assert.AreEqual(okResult2.StatusCode, 200);
            Assert.IsTrue(actual2.Contains(expected2));

            Assert.IsInstanceOf<OkObjectResult>(result3);
            Assert.AreEqual(okResult3.StatusCode, 200);
            Assert.IsTrue(actual3.Contains(expected3));
            Assert.AreEqual(product1.Name, _eCommerceContext.Products.FirstOrDefault(c => c.ProductId == 1).Name);
            Assert.AreEqual(product2.Name, _eCommerceContext.Products.FirstOrDefault(c => c.ProductId == 2).Name);
            Assert.AreEqual(product3.Name, _eCommerceContext.Products.FirstOrDefault(c => c.ProductId == 3).Name);

        }

        [Test]
        public async Task _09Test_PutProducts_ReturnsNotUpdated_WhenSellerDidNotAddThem()
        {
            //Arrange 
            var authdb = _authenticationContext;
            var sellerRole = authdb.Roles.FirstOrDefault(c => c.Name == "Seller").Id;
            var userRoles = authdb.UserRoles.FirstOrDefault(c => c.RoleId == sellerRole).UserId;
            identityUser = authdb.ApplicationUsers.FirstOrDefault(c => c.Id == userRoles);
            var user = new ApplicationUser { Id = identityUser.Id };
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("UserID", user.Id),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthentication");
            principal = new ClaimsPrincipal(identity);
            _controllerUnderTest = new ProductsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var product1 = new Product();
            product1.Name = "Nike Air 2";
            ProductsVM productsVM1 = new ProductsVM()
            {
                Name = product1.Name,
                Brand = product1.Brand,
                Description = product1.Description,
                Price = product1.Price,
                CategoryId = product1.CategoryId,
                StockQuantity = product1.StockQuantity,
                Type = product1.Type
            };

            var product2 = new Product();
            product2.Name = "Adidas Yeezy New Style";
            ProductsVM productsVM2 = new ProductsVM()
            {
                Name = product2.Name,
                Brand = product2.Brand,
                Description = product2.Description,
                Price = product2.Price,
                CategoryId = product2.CategoryId,
                StockQuantity = product2.StockQuantity,
                Type = product2.Type
            };

            var product3 = new Product();
            product3.Name = "Adidas Yeezy Black Slides";
            ProductsVM productsVM3 = new ProductsVM()
            {
                Name = product3.Name,
                Brand = product3.Brand,
                Description = product3.Description,
                Price = product3.Price,
                CategoryId = product3.CategoryId,
                StockQuantity = product3.StockQuantity,
                Type = product3.Type
            };




            //Act            
            var result1 = await _controllerUnderTest.PutProducts(1, productsVM1);
            var result2 = await _controllerUnderTest.PutProducts(2, productsVM2);
            var result3 = await _controllerUnderTest.PutProducts(3, productsVM3);

            var badResult1 = (BadRequestObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "Error, Seller can only update products they've added";

            var badResult2 = (BadRequestObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "Error, Seller can only update products they've added";

            var badResult3 = (BadRequestObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "Error, Seller can only update products they've added";

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result1);
            Assert.AreEqual(badResult1.StatusCode, 400);
            Assert.IsTrue(actual1.Contains(expected1));

            Assert.IsInstanceOf<BadRequestObjectResult>(result2);
            Assert.AreEqual(badResult2.StatusCode, 400);
            Assert.IsTrue(actual2.Contains(expected2));

            Assert.IsInstanceOf<BadRequestObjectResult>(result3);
            Assert.AreEqual(badResult3.StatusCode, 400);
            Assert.IsTrue(actual3.Contains(expected3));

        }

        [Test]
        public async Task _10Test_PutProducts_ReturnsNotFoundResult()
        {
            //Arrange 
            var product1 = new Product();
            product1.ProductId = 11;
            product1.Name = "Nike Air 2";
            ProductsVM productsVM1 = new ProductsVM()
            {
                Name = product1.Name,
            };

            var product2 = new Product();
            product2.ProductId = 12;
            product2.Name = "Adidas Yeezy New Style";
            ProductsVM productsVM2 = new ProductsVM()
            {
                Name = product2.Name,
            };

            var product3 = new Product();
            product3.ProductId = 13;
            product3.Name = "Adidas Yeezy Black Slides";
            ProductsVM productsVM3 = new ProductsVM()
            {
                Name = product3.Name,
            };


            //Act            
            var result1 = await _controllerUnderTest.PutProducts(11, productsVM1);
            var result2 = await _controllerUnderTest.PutProducts(12, productsVM2);
            var result3 = await _controllerUnderTest.PutProducts(13, productsVM3);

            var badResult1 = (NotFoundObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "No Product with that ID exists, please try again";

            var badResult2 = (NotFoundObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "No Product with that ID exists, please try again";

            var badResult3 = (NotFoundObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "No Product with that ID exists, please try again";

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result1);
            Assert.AreEqual(badResult1.StatusCode, 404);
            Assert.IsTrue(actual1.Contains(expected1));

            Assert.IsInstanceOf<NotFoundObjectResult>(result2);
            Assert.AreEqual(badResult2.StatusCode, 404);
            Assert.IsTrue(actual2.Contains(expected2));

            Assert.IsInstanceOf<NotFoundObjectResult>(result3);
            Assert.AreEqual(badResult3.StatusCode, 404);
            Assert.IsTrue(actual3.Contains(expected3));

        }

        [Test]
        public async Task _11Test_PostProduct_ReturnsActionResultObjectWith13Products_WhenAdded3Products()
        {
            //Arrange
            Product _product2 = new Product()
            {
                ProductId = 2,
                Name = "Adidas Yeezy",
                Brand = "Adidas",
                Description = "Adidas Yeezy sneakers for summer wear",
                Type = "Women",
                Price = 2700.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2017, 1, 22, 15, 35, 10)
            };
            ProductsVM _productVM2 = new ProductsVM()
            {
                Name = _product2.Name,
                Brand = _product2.Brand,
                Description = _product2.Description,
                Type = _product2.Type,
                Price = _product2.Price,
                CategoryId = _product2.CategoryId,
                StockQuantity = _product2.StockQuantity,
            };
            Product _product3 = new Product()
            {
                ProductId = 3,
                Name = "Puma T-shirt",
                Brand = "Puma",
                Description = "Puma T-shirt for the summer and track wear",
                Type = "Women",
                Price = 900.99f,
                CategoryId = 3,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2019, 10, 12, 12, 45, 00)
            };
            ProductsVM _productVM3 = new ProductsVM()
            {
                Name = _product3.Name,
                Brand = _product3.Brand,
                Description = _product3.Description,
                Type = _product3.Type,
                Price = _product3.Price,
                CategoryId = _product3.CategoryId,
                StockQuantity = _product3.StockQuantity
            };

            //Act            
            var result1 = await _controllerUnderTest.PostProducts(_productVM);
            var result2 = await _controllerUnderTest.PostProducts(_productVM2);
            var result3 = await _controllerUnderTest.PostProducts(_productVM3);

            //Assert
            Assert.NotNull(_eCommerceContext.Products);
            Assert.AreEqual(13, _eCommerceContext.Products.Count());

        }


        [TestCase(11)]
        [TestCase(12)]
        [TestCase(10)]
        [TestCase(9)]
        [TestCase(8)]
        [TestCase(7)]
        public async Task _12Test_GetProductByID_ReturnsWithCorrectType_WhenPassedInID(int id)
        {
            //Arrange 
            Product _product2 = new Product()
            {
                ProductId = 2,
                Name = "Adidas Yeezy",
                Brand = "Adidas",
                Description = "Adidas Yeezy sneakers for summer wear",
                Type = "Women",
                Price = 2700.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2017, 1, 22, 15, 35, 10)
            };
            ProductsVM _productVM2 = new ProductsVM()
            {
                Name = _product2.Name,
                Brand = _product2.Brand,
                Description = _product2.Description,
                Type = _product2.Type,
                Price = _product2.Price,
                CategoryId = _product2.CategoryId,
                StockQuantity = _product2.StockQuantity
            };

            await _controllerUnderTest.PostProducts(_productVM);
            await _controllerUnderTest.PostProducts(_productVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetProducts(id);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Product>>(actionResult);
        }



        [Test]
        public async Task _12Test_GetAllproduct_ReturnsWithCorrectTypeAndCount()
        {
            //Arrange 
            Product _product2 = new Product()
            {
                ProductId = 2,
                Name = "Adidas Yeezy",
                Brand = "Adidas",
                Description = "Adidas Yeezy sneakers for summer wear",
                Type = "Women",
                Price = 2700.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2017, 1, 22, 15, 35, 10)
            };
            ProductsVM _productVM2 = new ProductsVM()
            {
                Name = _product2.Name,
                Brand = _product2.Brand,
                Description = _product2.Description,
                Type = _product2.Type,
                Price = _product2.Price,
                CategoryId = _product2.CategoryId,
                StockQuantity = _product2.StockQuantity
            };
            Product _product3 = new Product()
            {
                ProductId = 3,
                Name = "Puma T-shirt",
                Brand = "Puma",
                Description = "Puma T-shirt for the summer and track wear",
                Type = "Women",
                Price = 900.99f,
                CategoryId = 3,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2019, 10, 12, 12, 45, 00)
            };
            ProductsVM _productVM3 = new ProductsVM()
            {
                Name = _product3.Name,
                Brand = _product3.Brand,
                Description = _product3.Description,
                Type = _product3.Type,
                Price = _product3.Price,
                CategoryId = _product3.CategoryId,
                StockQuantity = _product3.StockQuantity
            };

            await _controllerUnderTest.PostProducts(_productVM);
            await _controllerUnderTest.PostProducts(_productVM2);
            await _controllerUnderTest.PostProducts(_productVM3);

            //Act            
            var actionResult = await _controllerUnderTest.GetProducts();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<IEnumerable<Product>>>(actionResult);
            var result = (OkObjectResult)actionResult.Result;
            var value = (List<Product>)result.Value;
            Assert.AreEqual(_eCommerceContext.Products.Count(), value.Count);
        }

        [Test]
        public async Task _13Test_GetproductById_ReturnsWithCorrectType()
        {
            //Arrange 
            Product _product2 = new Product()
            {
                ProductId = 2,
                Name = "Adidas Yeezy",
                Brand = "Adidas",
                Description = "Adidas Yeezy sneakers for summer wear",
                Type = "Women",
                Price = 2700.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2017, 1, 22, 15, 35, 10)
            };
            ProductsVM _productVM2 = new ProductsVM()
            {
                Name = _product2.Name,
                Brand = _product2.Brand,
                Description = _product2.Description,
                Type = _product2.Type,
                Price = _product2.Price,
                CategoryId = _product2.CategoryId,
                StockQuantity = _product2.StockQuantity
            };


            await _controllerUnderTest.PostProducts(_productVM);
            await _controllerUnderTest.PostProducts(_productVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetProducts(11);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Product>>(actionResult);
        }


        [Test]
        public async Task _14Test_PostProducts_AddedSuccessfullyAndShowsInContextCount_WhenUserIsASeller()
        {
            //Arrange
            var authdb = _authenticationContext;
            var sellerRole = authdb.Roles.FirstOrDefault(c => c.Name == "Seller").Id;
            var userRoles = authdb.UserRoles.FirstOrDefault(c => c.RoleId == sellerRole).UserId;
            identityUser = authdb.ApplicationUsers.FirstOrDefault(c=> c.Id == userRoles);
            var user = new ApplicationUser { Id = identityUser.Id };
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("UserID", user.Id),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthentication");
            principal = new ClaimsPrincipal(identity);
            _controllerUnderTest = new ProductsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            //Act            
            var actionResult = await _controllerUnderTest.PostProducts(_productVM);
            var userId = _eCommerceContext.EfUsers.FirstOrDefault(c => c.IdentityUsername == identityUser.UserName).EfUserId;
            var prodId = _eCommerceContext.Products.FirstOrDefault(c => c.ProductId == _eCommerceContext.Products.Max(c => c.ProductId)).ProductId;
            //Assert
            Assert.NotNull(actionResult);
            Assert.IsTrue(_eCommerceContext.EfUserProducts.Where(c => c.EfUserId == userId && c.ProductId == prodId).Count() > 0);

        }


        [Test]
        public async Task _15Test_PostProducts_ReturnsBadObjectResult_WhenUserIsACustomer()
        {
            //Arrange
            var authdb = _authenticationContext;
            var customerRole = authdb.Roles.FirstOrDefault(c => c.Name == "Customer").Id;
            var userRoles = authdb.UserRoles.FirstOrDefault(c => c.RoleId == customerRole).UserId;
            identityUser = authdb.ApplicationUsers.FirstOrDefault(c => c.Id == userRoles);
            var user = new ApplicationUser { Id = identityUser.Id };
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("UserID", user.Id),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthentication");
            principal = new ClaimsPrincipal(identity);
            _controllerUnderTest = new ProductsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            //Act            
            var actionResult = await _controllerUnderTest.PostProducts(_productVM);

            //Assert
            var result = (ActionResult<Product>)actionResult.Result;
            var badResult = (BadRequestObjectResult)actionResult.Result;
            var expected = "Not authorised to add products as a customer";
            var actual = badResult.Value.ToString();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Product>>(actionResult);
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(10, _eCommerceContext.Products.Count());
        }


        [Test]
        public async Task _16Test_DeleteProducts_ReturnsMessageThatSellerCannotDeleteProduct_WhenTheyDidNotAddIt()
        {
            //Arrange 
            var authdb = _authenticationContext;
            var sellerRole = authdb.Roles.FirstOrDefault(c => c.Name == "Seller").Id;
            var userRoles = authdb.UserRoles.FirstOrDefault(c => c.RoleId == sellerRole).UserId;
            identityUser = authdb.ApplicationUsers.FirstOrDefault(c => c.Id == userRoles);
            var user = new ApplicationUser { Id = identityUser.Id };
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("UserID", user.Id),
            };
            var identity = new ClaimsIdentity(claims, "TestAuthentication");
            principal = new ClaimsPrincipal(identity);
            _controllerUnderTest = new ProductsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            Product _product2 = new Product()
            {
                ProductId = 2,
                Name = "Adidas Yeezy",
                Brand = "Adidas",
                Description = "Adidas Yeezy sneakers for summer wear",
                Type = "Women",
                Price = 2700.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2017, 1, 22, 15, 35, 10)
            };
            ProductsVM _productVM2 = new ProductsVM()
            {
                Name = _product2.Name,
                Brand = _product2.Brand,
                Description = _product2.Description,
                Type = _product2.Type,
                Price = _product2.Price,
                CategoryId = _product2.CategoryId,
                StockQuantity = _product2.StockQuantity
            };

            //Act
            var actionResult = await _controllerUnderTest.PostProducts(_productVM2);

            var actionResultDeleted = await _controllerUnderTest.DeleteProducts(8);
            var result = (ActionResult<Product>)actionResultDeleted.Result;
            var badResult = (BadRequestObjectResult)actionResultDeleted.Result;
            var expected = "Error, Seller can only delete products they've added";
            var actual = badResult.Value.ToString();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Product>>(actionResultDeleted);
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(11, _eCommerceContext.Products.Count());
        }



        [Test]
        public async Task _17Test_DeleteProducts_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount()
        {
            //Arrange
            Product _product2 = new Product()
            {
                ProductId = 11,
                Name = "Adidas Yeezy",
                Brand = "Adidas",
                Description = "Adidas Yeezy sneakers for summer wear",
                Type = "Women",
                Price = 2700.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2017, 1, 22, 15, 35, 10)
            };
            ProductsVM _productVM2 = new ProductsVM()
            {
                Name = _product2.Name,
                Brand = _product2.Brand,
                Description = _product2.Description,
                Type = _product2.Type,
                Price = _product2.Price,
                CategoryId = _product2.CategoryId,
                StockQuantity = _product2.StockQuantity
            };
            //Act
            var actionResult = await _controllerUnderTest.PostProducts(_productVM2);
            var actionResultDeleted = await _controllerUnderTest.DeleteProducts(_product2.ProductId);
            
            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Product>>(actionResultDeleted);
            Assert.AreEqual(10, _eCommerceContext.Products.Count());
        }

        [Test]
        public async Task _18Test_DeleteProducts_ReturnsBadObjectResult_WhenTryToDeleteAProductThatHasBeenOrdered()
        {
            //Arrange
            Product _product2 = new Product()
            {
                ProductId = 2,
                Name = "Adidas Yeezy",
                Brand = "Adidas",
                Description = "Adidas Yeezy sneakers for summer wear",
                Type = "Women",
                Price = 2700.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2017, 1, 22, 15, 35, 10)
            };
            ProductsVM _productVM2 = new ProductsVM()
            {
                Name = _product2.Name,
                Brand = _product2.Brand,
                Description = _product2.Description,
                Type = _product2.Type,
                Price = _product2.Price,
                CategoryId = _product2.CategoryId,
                StockQuantity = _product2.StockQuantity
            };
            //Act
            var actionResult = await _controllerUnderTest.PostProducts(_productVM2);
            var actionResultDeleted = await _controllerUnderTest.DeleteProducts(_product2.ProductId);
            var badResult = (BadRequestObjectResult)actionResultDeleted.Result;
            var expected = "Error, Cannot delete Products that have been ordered";
            var actual = badResult.Value.ToString();

            //Assert
            Assert.NotNull(actionResult);
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(11, _eCommerceContext.Products.Count());
        }


        [Test]
        public async Task _19Test_DeleteProducts_AddMultipleDeleteOne_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount()
        {
            //Arrange 
            Product _product2 = new Product()
            {
                ProductId = 11,
                Name = "Adidas Yeezy",
                Brand = "Adidas",
                Description = "Adidas Yeezy sneakers for summer wear",
                Type = "Women",
                Price = 2700.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2017, 1, 22, 15, 35, 10)
            };
            ProductsVM _productVM2 = new ProductsVM()
            {
                Name = _product2.Name,
                Brand = _product2.Brand,
                Description = _product2.Description,
                Type = _product2.Type,
                Price = _product2.Price,
                CategoryId = _product2.CategoryId,
                StockQuantity = _product2.StockQuantity
            };
            Product _product3 = new Product()
            {
                ProductId = 12,
                Name = "Puma T-shirt",
                Brand = "Puma",
                Description = "Puma T-shirt for the summer and track wear",
                Type = "Women",
                Price = 900.99f,
                CategoryId = 3,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2019, 10, 12, 12, 45, 00)
            };
            ProductsVM _productVM3 = new ProductsVM()
            {
                Name = _product3.Name,
                Brand = _product3.Brand,
                Description = _product3.Description,
                Type = _product3.Type,
                Price = _product3.Price,
                CategoryId = _product3.CategoryId,
                StockQuantity = _product3.StockQuantity
            };

            //Act
            await _controllerUnderTest.PostProducts(_productVM2);
            await _controllerUnderTest.PostProducts(_productVM3);

            var actionResultDeleted = await _controllerUnderTest.DeleteProducts(_product2.ProductId);
            var actionResultDeleted2 = await _controllerUnderTest.DeleteProducts(_product3.ProductId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Product>>(actionResultDeleted);
            Assert.NotNull(actionResultDeleted2);
            Assert.IsInstanceOf<ActionResult<Product>>(actionResultDeleted2);
            Assert.AreEqual(10, _eCommerceContext.Products.Count());
        }
    }
}
using LLM_eCommerce_RESTAPI.AuthModels;
using LLM_eCommerce_RESTAPI.Controllers;
using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RESTApi.NunitTests
{
    [TestFixture]
    public class CategoriesControllerTests
    {
        private LLM_eCommerce_EFDBContext _eCommerceContext;
        private CategoriesController _controllerUnderTest;
        private List<Category> _categoriesList;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AuthenticationContext _authenticationContext;
        CategoriesVM _categoriesVM;
        Category _categories;
        IdentityUser identityUser;
        ClaimsPrincipal principal;

        [SetUp]
        public void Initialiser()
        {
            _eCommerceContext = (LLM_eCommerce_EFDBContext)InMemoryContext.GeneratedDB();
            var prod = _eCommerceContext.Categories.Count();
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
            _controllerUnderTest = new CategoriesController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            _categoriesList = new List<Category>();
            _categories = new Category()
            {
                CategoryId = 243,
                Name = "Shacket"
            };
            _categoriesVM = new CategoriesVM()
            {
                Name = _categories.Name
            };

        }



        [TearDown]
        public void CleanUpObject()
        {
            _eCommerceContext.Database.EnsureDeleted();
            _controllerUnderTest = null;
            _categoriesList = null;
            _categories = null;
            _userManager = null;

            _roleManager = null;
            _authenticationContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task _01Test_GetAllCategorie_ReturnsListWithValidCount0()
        {
            // Arrange


            // Act
            var result = await _controllerUnderTest.GetCategories();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var categoriesList = okResult.Value as List<Category>;
            Assert.NotNull(categoriesList);
            Assert.AreEqual(10, categoriesList.Count);
        }

        [Test]
        public async Task _02Test_GetAllCategorie_ReturnsListWithValidCountEqualTo11()
        {
            // Arrange
            _eCommerceContext.Categories.Add(_categories);
            await _eCommerceContext.SaveChangesAsync();


            _controllerUnderTest = new CategoriesController(_eCommerceContext, _userManager, _roleManager, (AuthenticationContext)_authenticationContext);


            // Act
            var result = await _controllerUnderTest.GetCategories();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var categoriesList = okResult.Value as List<Category>;
            Assert.NotNull(categoriesList);
            Assert.AreEqual(11, categoriesList.Count);
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
        public async Task _03Test_GetCategorieByID_ReturnsaListwithCount1_WhenCorrectCategoryIDEntered(int idPassedIn)
        {
            //Arrange
            int id = idPassedIn;

            //Act            
            var result = await _controllerUnderTest.GetCategories(id);
            var okResult = (OkObjectResult)result.Result;
            var actual = (Category)okResult.Value;
            var expected = _eCommerceContext.Categories.FirstOrDefault(c => c.CategoryId == id);

            //Assert
            Assert.IsInstanceOf<ActionResult<Category>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.AreEqual(actual.CategoryId, expected.CategoryId);

        }


        [TestCase("Sneaker")]
        [TestCase("Hats")]
        [TestCase("T-shirt")]
        [TestCase("Hoodie")]
        [TestCase("Shorts")]
        public async Task _04Test_GetCategoryByName_ReturnsaListwithCount1_WhenCorrectCategoryIDEntered(string namePassedIn)
        {
            //Arrange
            string name = namePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetCategoryByName(name);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Category>)okResult.Value;
            var expected = _eCommerceContext.Categories.FirstOrDefault(c => c.Name == name);

            //Assert
            Assert.IsInstanceOf<ActionResult<Category>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.AreEqual(actual.FirstOrDefault(c => c.Name == name).CategoryId, expected.CategoryId);

        }

        [TestCase("Nike Air Loss 1")]
        [TestCase("Adidas Drizzy")]
        [TestCase("Puma See See")]
        [TestCase("Reebok Lost")]
        [TestCase("Converse Nostar")]
        public async Task _05Test_GetCategoryByName_ReturnsNotFound_WhenIncorrectCategorieNameEntered(string namePassedIn)
        {
            //Arrange
            string name = namePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetCategoryByName(name);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = notFoundResult.Value.ToString();
            var expected = "No Category with that Name exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }

        [Test]
        public async Task _06Test_GetCategorieByID_ReturnsaBackActionResult_WhenCategoryIDEnteredDoesNotExist()
        {
            //Arrange
            int id = 20;

            //Act            
            var result = await _controllerUnderTest.GetCategories(id);
            var badResult = (NotFoundObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "No Category with that ID exists, please try again";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 404);

        }

        [Test]
        public async Task _07Test_PostAcategories_ReturnsBadRequest_WhenEmptyCategorieAdded()
        {
            //Arrange 

            //Act            
            var result = await _controllerUnderTest.PostCategories(new CategoriesVM());
            var badResult = (BadRequestObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "Cannot Add an empty category";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.AreEqual(10, _eCommerceContext.Categories.Count());

        }

        [Test]
        public async Task _08Test_PutCategories_ReturnsOkObjectResult()
        {
            //Arrange 
            var categories1 = new CategoriesVM();
            categories1.Name = "Vest";

            var categories2 = new CategoriesVM();
            categories2.Name = "Heels";

            var categories3 = new CategoriesVM();
            categories3.Name = "Sweater";


            //Act            
            var result1 = await _controllerUnderTest.PutCategories(1, categories1);
            var result2 = await _controllerUnderTest.PutCategories(2, categories2);
            var result3 = await _controllerUnderTest.PutCategories(3, categories3);

            var okResult1 = (OkObjectResult)result1;
            var actual1 = okResult1.Value.ToString();
            var expected1 = "Category Updated";

            var okResult2 = (OkObjectResult)result2;
            var actual2 = okResult2.Value.ToString();
            var expected2 = "Category Updated";

            var okResult3 = (OkObjectResult)result3;
            var actual3 = okResult3.Value.ToString();
            var expected3 = "Category Updated";

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
            Assert.AreEqual(categories1.Name, _eCommerceContext.Categories.FirstOrDefault(c => c.CategoryId == 1).Name);
            Assert.AreEqual(categories2.Name, _eCommerceContext.Categories.FirstOrDefault(c => c.CategoryId == 2).Name);
            Assert.AreEqual(categories3.Name, _eCommerceContext.Categories.FirstOrDefault(c => c.CategoryId == 3).Name);

        }

        [Test]
        public async Task _09Test_PutCategories_ReturnsNotUpdated_WhenSellerDidNotAddThem()
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
            _controllerUnderTest = new CategoriesController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var categories1 = new CategoriesVM();
            categories1.Name = "Vest";

            var categories2 = new CategoriesVM();
            categories2.Name = "Heels";

            var categories3 = new CategoriesVM();
            categories3.Name = "Sweater";


            //Act            
            var result1 = await _controllerUnderTest.PutCategories(1, categories1);
            var result2 = await _controllerUnderTest.PutCategories(2, categories2);
            var result3 = await _controllerUnderTest.PutCategories(3, categories3);

            var badResult1 = (BadRequestObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "Not authorised to update categories";

            var badResult2 = (BadRequestObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "Not authorised to update categories";

            var badResult3 = (BadRequestObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "Not authorised to update categories";

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
        public async Task _10Test_PutCategories_ReturnsNotFoundResult()
        {
            //Arrange 
            var categories1 = new CategoriesVM();
            categories1.Name = "Lip Gloss";
            CategoriesVM categoriesVM1 = new CategoriesVM()
            {
                Name = categories1.Name,
            };

            var categories2 = new CategoriesVM();
            categories2.Name = "Torn Jeans";
            var categories3 = new CategoriesVM();
            categories3.Name = "Torn Sweaters";


            //Act            
            var result1 = await _controllerUnderTest.PutCategories(11, categories1);
            var result2 = await _controllerUnderTest.PutCategories(12, categories2);
            var result3 = await _controllerUnderTest.PutCategories(13, categories3);

            var badResult1 = (NotFoundObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "Category Id not found, no changes made, please try again";

            var badResult2 = (NotFoundObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "Category Id not found, no changes made, please try again";

            var badResult3 = (NotFoundObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "Category Id not found, no changes made, please try again";

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
        public async Task _11Test_PostCategorie_ReturnsActionResultObjectWith13Categories_WhenAdded3Categories()
        {
            //Arrange
            Category _categories2 = new Category()
            {
                CategoryId = 12,
                Name = "Bag"
            };
            CategoriesVM _categoriesVM2 = new CategoriesVM()
            {
                Name = _categories2.Name
            };
            Category _categories3 = new Category()
            {
                CategoryId = 13,
                Name = "Sweater"
            };
            CategoriesVM _categoriesVM3 = new CategoriesVM()
            {
                Name = _categories3.Name
            };

            //Act            
            var result1 = await _controllerUnderTest.PostCategories(_categoriesVM);
            var result2 = await _controllerUnderTest.PostCategories(_categoriesVM2);
            var result3 = await _controllerUnderTest.PostCategories(_categoriesVM3);

            //Assert
            Assert.NotNull(_eCommerceContext.Categories);
            Assert.AreEqual(13, _eCommerceContext.Categories.Count());

        }


        [TestCase(11)]
        [TestCase(12)]
        [TestCase(10)]
        [TestCase(9)]
        [TestCase(8)]
        [TestCase(7)]
        public async Task _12Test_GetCategorieByID_ReturnsWithCorrectType_WhenPassedInID(int id)
        {
            //Arrange 
            Category _categories2 = new Category()
            {
                CategoryId = 12,
                Name = "Sweater"
            };
            CategoriesVM _categoriesVM2 = new CategoriesVM()
            {
                Name = _categories2.Name
            };

            await _controllerUnderTest.PostCategories(_categoriesVM);
            await _controllerUnderTest.PostCategories(_categoriesVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetCategories(id);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Category>>(actionResult);
        }



        [Test]
        public async Task _12Test_GetAllcategories_ReturnsWithCorrectTypeAndCount()
        {
            //Arrange 
            Category _categories2 = new Category()
            {
                CategoryId = 2,
                Name = "Sweater",
            };
            CategoriesVM _categoriesVM2 = new CategoriesVM()
            {
                Name = _categories2.Name
            };
            Category _categories3 = new Category()
            {
                CategoryId = 3,
                Name = "Bag"
            };
            CategoriesVM _categoriesVM3 = new CategoriesVM()
            {
                Name = _categories3.Name
            };

            await _controllerUnderTest.PostCategories(_categoriesVM);
            await _controllerUnderTest.PostCategories(_categoriesVM2);
            await _controllerUnderTest.PostCategories(_categoriesVM3);

            //Act            
            var actionResult = await _controllerUnderTest.GetCategories();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<IEnumerable<Category>>>(actionResult);
            var result = (OkObjectResult)actionResult.Result;
            var value = (List<Category>)result.Value;
            Assert.AreEqual(_eCommerceContext.Categories.Count(), value.Count);
        }

        [Test]
        public async Task _13Test_GetcategoriesById_ReturnsWithCorrectType()
        {
            //Arrange 
            Category _categories2 = new Category()
            {
                CategoryId = 12,
                Name = "Bag"
            };
            CategoriesVM _categoriesVM2 = new CategoriesVM()
            {
                Name = _categories2.Name,
            };


            await _controllerUnderTest.PostCategories(_categoriesVM);
            await _controllerUnderTest.PostCategories(_categoriesVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetCategories(11);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Category>>(actionResult);
        }


        [Test]
        public async Task _14Test_PostCategories_AddedSuccessfullyAndShowsInContextCount_WhenUserIsASeller()
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
            _controllerUnderTest = new CategoriesController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            //Act            
            var actionResult = await _controllerUnderTest.PostCategories(_categoriesVM);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsTrue(_eCommerceContext.Categories.Count() == 10);

        }


        [Test]
        public async Task _15Test_PostCategories_ReturnsBadObjectResult_WhenUserIsACustomer()
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
            _controllerUnderTest = new CategoriesController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            //Act            
            var actionResult = await _controllerUnderTest.PostCategories(_categoriesVM);

            //Assert
            var result = (ActionResult<Category>)actionResult.Result;
            var badResult = (BadRequestObjectResult)actionResult.Result;
            var expected = "Not authorised to add categories";
            var actual = badResult.Value.ToString();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Category>>(actionResult);
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(10, _eCommerceContext.Categories.Count());
        }


        [Test]
        public async Task _16Test_DeleteCategories_ReturnsMessageThatSellerCannotDeleteCategorie_WhenTheyDidNotAddIt()
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
            _controllerUnderTest = new CategoriesController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            //Act

            var actionResultDeleted = await _controllerUnderTest.DeleteCategories(8);
            var result = (ActionResult<Category>)actionResultDeleted.Result;
            var badResult = (BadRequestObjectResult)actionResultDeleted.Result;
            var expected = "Not authorised to delete categories";
            var actual = badResult.Value.ToString();

            //Assert
            Assert.IsInstanceOf<ActionResult<Category>>(actionResultDeleted);
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(10, _eCommerceContext.Categories.Count());
        }



        [Test]
        public async Task _17Test_DeleteCategories_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount()
        {
            //Arrange
            Category _categories2 = new Category()
            {
                CategoryId = 11,
                Name = "Sweater"
            };
            CategoriesVM _categoriesVM2 = new CategoriesVM()
            {
                Name = _categories2.Name
            };
            //Act
            var actionResult = await _controllerUnderTest.PostCategories(_categoriesVM2);
            var actionResultDeleted = await _controllerUnderTest.DeleteCategories(_categories2.CategoryId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Category>>(actionResultDeleted);
            Assert.AreEqual(10, _eCommerceContext.Categories.Count());
        }

        [Test]
        public async Task _18Test_DeleteCategories_ReturnsBadObjectResult_WhenTryToDeleteACategorieThatHasBeenAssignedToAProduct()
        {
            //Arrange
            int id = 2;

            //Act
            var actionResultDeleted = await _controllerUnderTest.DeleteCategories(id);
            var badResult = (BadRequestObjectResult)actionResultDeleted.Result;
            var expected = "Error, Cannot delete Categories that have been assigned to a product";
            var actual = badResult.Value.ToString();

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(10, _eCommerceContext.Categories.Count());
        }


        [Test]
        public async Task _19Test_DeleteCategories_AddMultipleDeleteOne_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount()
        {
            //Arrange 
            Category _categories2 = new Category()
            {
                CategoryId = 11,
                Name = "Sweater"
            };
            CategoriesVM _categoriesVM2 = new CategoriesVM()
            {
                Name = _categories2.Name
            };
            Category _categories3 = new Category()
            {
                CategoryId = 12,
                Name = "Bag"
            };
            CategoriesVM _categoriesVM3 = new CategoriesVM()
            {
                Name = _categories3.Name
            };

            //Act
            await _controllerUnderTest.PostCategories(_categoriesVM2);
            await _controllerUnderTest.PostCategories(_categoriesVM3);

            var actionResultDeleted = await _controllerUnderTest.DeleteCategories(_categories2.CategoryId);
            var actionResultDeleted2 = await _controllerUnderTest.DeleteCategories(_categories3.CategoryId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Category>>(actionResultDeleted);
            Assert.NotNull(actionResultDeleted2);
            Assert.IsInstanceOf<ActionResult<Category>>(actionResultDeleted2);
            Assert.AreEqual(10, _eCommerceContext.Categories.Count());
        }
    }
}

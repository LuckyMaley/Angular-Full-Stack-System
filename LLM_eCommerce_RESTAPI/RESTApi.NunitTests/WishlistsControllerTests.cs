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
    public class WishlistsControllerTests
    {
        private LLM_eCommerce_EFDBContext _eCommerceContext;
        private WishlistsController _controllerUnderTest;
        private List<Wishlist> _wishlistList;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AuthenticationContext _authenticationContext;
        WishlistsVM _wishlistVM;
        Wishlist _wishlist;
        IdentityUser identityUser;
        ClaimsPrincipal principal;

        [SetUp]
        public void Initialiser()
        {
            _eCommerceContext = (LLM_eCommerce_EFDBContext)InMemoryContext.GeneratedDB();
            var prod = _eCommerceContext.Wishlists.Count();
            _authenticationContext = (AuthenticationContext)InMemoryContext.GeneratedAuthDB();
            _userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(_authenticationContext), null, null, null, null, null, null, null, null);

            _roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(_authenticationContext), null, null, null, null);
            var authdb = _authenticationContext;
            var sellerRole = authdb.Roles.FirstOrDefault(c => c.Name == "Customer").Id;
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
            _controllerUnderTest = new WishlistsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            _wishlistList = new List<Wishlist>();
            _wishlist = new Wishlist()
            {
                WishlistId = 11,
                EfUserId = 21,
                ProductId = 1,
                AddedDate = new DateTime(2020, 12, 14, 12, 55, 00)
            };
            _wishlistVM = new WishlistsVM()
            {
                ProductId = 1
            };

        }



        [TearDown]
        public void CleanUpObject()
        {
            _eCommerceContext.Database.EnsureDeleted();
            _controllerUnderTest = null;
            _wishlistList = null;
            _wishlist = null;
            _userManager = null;

            _roleManager = null;
            _authenticationContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task _01Test_GetAllWishlist_ReturnsListWithValidCount0()
        {
            // Arrange


            // Act
            var result = await _controllerUnderTest.GetWishlists();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var wishlistList = okResult.Value as List<Wishlist>;
            Assert.NotNull(wishlistList);
            Assert.AreEqual(10, wishlistList.Count);
        }

        [Test]
        public async Task _02Test_GetAllWishlist_ReturnsListWithValidCountEqualTo11()
        {
            // Arrange
            _eCommerceContext.Wishlists.Add(_wishlist);
            await _eCommerceContext.SaveChangesAsync();


            _controllerUnderTest = new WishlistsController(_eCommerceContext, _userManager, _roleManager, (AuthenticationContext)_authenticationContext);


            // Act
            var result = await _controllerUnderTest.GetWishlists();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var wishlistList = okResult.Value as List<Wishlist>;
            Assert.NotNull(wishlistList);
            Assert.AreEqual(11, wishlistList.Count);
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
        public async Task _03Test_GetWishlistByID_ReturnsaListwithCount1_WhenCorrectWishlistIDEntered(int idPassedIn)
        {
            //Arrange
            int id = idPassedIn;

            //Act            
            var result = await _controllerUnderTest.GetWishlists(id);
            var okResult = (OkObjectResult)result.Result;
            var actual = (Wishlist)okResult.Value;
            var expected = _eCommerceContext.Wishlists.FirstOrDefault(c => c.WishlistId == id);

            //Assert
            Assert.IsInstanceOf<ActionResult<Wishlist>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.AreEqual(actual.WishlistId, expected.WishlistId);

        }


        [Test]
        public async Task _04Test_GetWishlistByID_ReturnsaBackActionResult_WhenWishlistIDEnteredDoesNotExist()
        {
            //Arrange
            int id = 20;

            //Act            
            var result = await _controllerUnderTest.GetWishlists(id);
            var badResult = (NotFoundObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "No Wishlist with that ID exists, please try again";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 404);

        }

        [Test]
        public async Task _05Test_PostAwishlist_ReturnsBadRequest_WhenEmptyWishlistAdded()
        {
            //Arrange 

            //Act            
            var result = await _controllerUnderTest.PostWishlists(new WishlistsVM());
            var badResult = (BadRequestObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "Cannot Add an empty wishlist";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.AreEqual(10, _eCommerceContext.Wishlists.Count());

        }

        [Test]
        public async Task _06Test_PutWishlists_ReturnsOkObjectResult()
        {
            //Arrange 
            var authdb = _authenticationContext;
            var sellerRole = authdb.Roles.FirstOrDefault(c => c.Name == "Administrator").Id;
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
            _controllerUnderTest = new WishlistsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var wishlist1 = new Wishlist()
            {
                WishlistId = 12,
                EfUserId = 22,
                ProductId = 2,
                AddedDate = new DateTime(2022, 12, 14, 12, 55, 00)
            };
            var wishlistsVM1 = new WishlistsVM()
            {
                ProductId = 8
            };

            var wishlist2 = new Wishlist()
            {
                WishlistId = 13,
                EfUserId = 23,
                ProductId = 4,
                AddedDate = new DateTime(2022, 10, 14, 12, 55, 00)
            };
            var wishlistsVM2 = new WishlistsVM()
            {
                ProductId = 7
            };

            var wishlist3 = new Wishlist()
            {
                WishlistId = 14,
                EfUserId = 24,
                ProductId = 3,
                AddedDate = new DateTime(2021, 12, 04, 12, 55, 00)
            };
            var wishlistsVM3 = new WishlistsVM()
            {
                ProductId = 6
            };


            //Act            
            var result1 = await _controllerUnderTest.PutWishlists(1, wishlistsVM1);
            var result2 = await _controllerUnderTest.PutWishlists(2, wishlistsVM2);
            var result3 = await _controllerUnderTest.PutWishlists(3, wishlistsVM3);

            var okResult1 = (OkObjectResult)result1;
            var actual1 = okResult1.Value.ToString();
            var expected1 = "Wishlist Updated";

            var okResult2 = (OkObjectResult)result2;
            var actual2 = okResult2.Value.ToString();
            var expected2 = "Wishlist Updated";

            var okResult3 = (OkObjectResult)result3;
            var actual3 = okResult3.Value.ToString();
            var expected3 = "Wishlist Updated";

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

        }

        [Test]
        public async Task _07Test_PutWishlists_ReturnsNotUpdated_WhenSeller()
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
            _controllerUnderTest = new WishlistsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var wishlist1 = new Wishlist()
            {
                WishlistId = 12,
                EfUserId = 22,
                ProductId = 2,
                AddedDate = new DateTime(2022, 12, 14, 12, 55, 00)
            };
            var wishlistsVM1 = new WishlistsVM()
            {
                ProductId = 2
            };

            var wishlist2 = new Wishlist()
            {
                WishlistId = 13,
                EfUserId = 23,
                ProductId = 4,
                AddedDate = new DateTime(2022, 10, 14, 12, 55, 00)
            };
            var wishlistsVM2 = new WishlistsVM()
            {
                ProductId = 4
            };

            var wishlist3 = new Wishlist()
            {
                WishlistId = 14,
                EfUserId = 24,
                ProductId = 3,
                AddedDate = new DateTime(2021, 12, 04, 12, 55, 00)
            };
            var wishlistsVM3 = new WishlistsVM()
            {
                ProductId = 3
            };


            //Act            
            var result1 = await _controllerUnderTest.PutWishlists(1, wishlistsVM1);
            var result2 = await _controllerUnderTest.PutWishlists(2, wishlistsVM2);
            var result3 = await _controllerUnderTest.PutWishlists(3, wishlistsVM3);

            var badResult1 = (BadRequestObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "Not authorised to update wishlists";

            var badResult2 = (BadRequestObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "Not authorised to update wishlists";

            var badResult3 = (BadRequestObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "Not authorised to update wishlists";

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
        public async Task _08Test_PutWishlists_ReturnsNotFoundResult()
        {
            //Arrange 
            var authdb = _authenticationContext;
            var sellerRole = authdb.Roles.FirstOrDefault(c => c.Name == "Administrator").Id;
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
            _controllerUnderTest = new WishlistsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };
            var wishlist1 = new Wishlist()
            {
                WishlistId = 12,
                EfUserId = 22,
                ProductId = 2,
                AddedDate = new DateTime(2022, 12, 14, 12, 55, 00)
            };
            var wishlistsVM1 = new WishlistsVM()
            {
                ProductId = 2
            };

            var wishlist2 = new Wishlist()
            {
                WishlistId = 13,
                EfUserId = 23,
                ProductId = 4,
                AddedDate = new DateTime(2022, 10, 14, 12, 55, 00)
            };
            var wishlistsVM2 = new WishlistsVM()
            {
                ProductId = 4
            };

            var wishlist3 = new Wishlist()
            {
                WishlistId = 14,
                EfUserId = 24,
                ProductId = 3,
                AddedDate = new DateTime(2021, 12, 04, 12, 55, 00)
            };
            var wishlistsVM3 = new WishlistsVM()
            {
                ProductId = 3
            };


            //Act            
            var result1 = await _controllerUnderTest.PutWishlists(11, wishlistsVM1);
            var result2 = await _controllerUnderTest.PutWishlists(12, wishlistsVM2);
            var result3 = await _controllerUnderTest.PutWishlists(13, wishlistsVM3);

            var badResult1 = (NotFoundObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "No Wishlist with that ID exists, please try again";

            var badResult2 = (NotFoundObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "No Wishlist with that ID exists, please try again";

            var badResult3 = (NotFoundObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "No Wishlist with that ID exists, please try again";

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
        public async Task _09Test_PostWishlist_ReturnsActionResultObjectWith13Wishlists_WhenAdded3Wishlists()
        {
            //Arrange
            var wishlist1 = new Wishlist()
            {
                WishlistId = 12,
                EfUserId = 22,
                ProductId = 2,
                AddedDate = new DateTime(2022, 12, 14, 12, 55, 00)
            };
            var wishlistsVM1 = new WishlistsVM()
            {
                ProductId = 2
            };

            var wishlist2 = new Wishlist()
            {
                WishlistId = 13,
                EfUserId = 23,
                ProductId = 4,
                AddedDate = new DateTime(2022, 10, 14, 12, 55, 00)
            };
            var wishlistsVM2 = new WishlistsVM()
            {
                ProductId = 4
            };

            var wishlist3 = new Wishlist()
            {
                WishlistId = 14,
                EfUserId = 24,
                ProductId = 3,
                AddedDate = new DateTime(2021, 12, 04, 12, 55, 00)
            };
            var wishlistsVM3 = new WishlistsVM()
            {
                ProductId = 3
            };

            //Act            
            var result1 = await _controllerUnderTest.PostWishlists(_wishlistVM);
            var result2 = await _controllerUnderTest.PostWishlists(wishlistsVM2);
            var result3 = await _controllerUnderTest.PostWishlists(wishlistsVM3);

            //Assert
            Assert.NotNull(_eCommerceContext.Wishlists);
            Assert.AreEqual(13, _eCommerceContext.Wishlists.Count());

        }


        [TestCase(11)]
        [TestCase(12)]
        [TestCase(10)]
        [TestCase(9)]
        [TestCase(8)]
        [TestCase(7)]
        public async Task _10Test_GetWishlistByID_ReturnsWithCorrectType_WhenPassedInID(int id)
        {
            //Arrange 
            var wishlist1 = new Wishlist()
            {
                WishlistId = 12,
                EfUserId = 22,
                ProductId = 2,
                AddedDate = new DateTime(2022, 12, 14, 12, 55, 00)
            };
            var wishlistsVM1 = new WishlistsVM()
            {
                ProductId = 2
            };

            var wishlist2 = new Wishlist()
            {
                WishlistId = 13,
                EfUserId = 23,
                ProductId = 4,
                AddedDate = new DateTime(2022, 10, 14, 12, 55, 00)
            };
            var wishlistsVM2 = new WishlistsVM()
            {
                ProductId = 4
            };

            var wishlist3 = new Wishlist()
            {
                WishlistId = 14,
                EfUserId = 24,
                ProductId = 3,
                AddedDate = new DateTime(2021, 12, 04, 12, 55, 00)
            };
            var wishlistsVM3 = new WishlistsVM()
            {
                ProductId = 3
            };

            await _controllerUnderTest.PostWishlists(wishlistsVM1);
            await _controllerUnderTest.PostWishlists(wishlistsVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetWishlists(id);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Wishlist>>(actionResult);
        }



        [Test]
        public async Task _11Test_GetAllwishlist_ReturnsWithCorrectTypeAndCount()
        {
            //Arrange 
            var wishlist1 = new Wishlist()
            {
                WishlistId = 12,
                EfUserId = 22,
                ProductId = 2,
                AddedDate = new DateTime(2022, 12, 14, 12, 55, 00)
            };
            var wishlistsVM1 = new WishlistsVM()
            {
                ProductId = 2
            };

            var wishlist2 = new Wishlist()
            {
                WishlistId = 13,
                EfUserId = 23,
                ProductId = 4,
                AddedDate = new DateTime(2022, 10, 14, 12, 55, 00)
            };
            var wishlistsVM2 = new WishlistsVM()
            {
                ProductId = 4
            };

            var wishlist3 = new Wishlist()
            {
                WishlistId = 14,
                EfUserId = 24,
                ProductId = 3,
                AddedDate = new DateTime(2021, 12, 04, 12, 55, 00)
            };
            var wishlistsVM3 = new WishlistsVM()
            {
                ProductId = 3
            };

            await _controllerUnderTest.PostWishlists(wishlistsVM1);
            await _controllerUnderTest.PostWishlists(wishlistsVM2);
            await _controllerUnderTest.PostWishlists(wishlistsVM3);

            //Act            
            var actionResult = await _controllerUnderTest.GetWishlists();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<IEnumerable<Wishlist>>>(actionResult);
            var result = (OkObjectResult)actionResult.Result;
            var value = (List<Wishlist>)result.Value;
            Assert.AreEqual(_eCommerceContext.Wishlists.Count(), value.Count);
        }

        [Test]
        public async Task _12Test_GetwishlistById_ReturnsWithCorrectType()
        {
            //Arrange 
            var wishlist1 = new Wishlist()
            {
                WishlistId = 12,
                EfUserId = 22,
                ProductId = 2,
                AddedDate = new DateTime(2022, 12, 14, 12, 55, 00)
            };
            var wishlistsVM1 = new WishlistsVM()
            {
                ProductId = 2
            };

            var wishlist2 = new Wishlist()
            {
                WishlistId = 13,
                EfUserId = 23,
                ProductId = 4,
                AddedDate = new DateTime(2022, 10, 14, 12, 55, 00)
            };
            var wishlistsVM2 = new WishlistsVM()
            {
                ProductId = 4
            };

            var wishlist3 = new Wishlist()
            {
                WishlistId = 14,
                EfUserId = 24,
                ProductId = 3,
                AddedDate = new DateTime(2021, 12, 04, 12, 55, 00)
            };
            var wishlistsVM3 = new WishlistsVM()
            {
                ProductId = 3
            };


            await _controllerUnderTest.PostWishlists(_wishlistVM);
            await _controllerUnderTest.PostWishlists(wishlistsVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetWishlists(11);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Wishlist>>(actionResult);
        }


        [Test]
        public async Task _13Test_PostWishlists_NotAddedAndShowsInContextCount_WhenUserIsASeller()
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
            _controllerUnderTest = new WishlistsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            //Act            
            var actionResult = await _controllerUnderTest.PostWishlists(_wishlistVM);
            //Assert
            Assert.IsFalse(_eCommerceContext.Wishlists.Where(c => c.WishlistId == 11).Count() > 0);
            Assert.IsTrue(_eCommerceContext.Wishlists.Count() == 10);

        }


        [Test]
        public async Task _14Test_PostWishlists_ReturnsBadObjectResult_WhenUserIsACustomer()
        {
            //Arrange

            //Act            
            var actionResult = await _controllerUnderTest.PostWishlists(_wishlistVM);

            //Assert
            var result = (ActionResult<Wishlist>)actionResult.Result;
            var goodResult = (OkObjectResult)actionResult.Result;
            var expected = "Not authorised to add wishlists as a customer";
            var actual = goodResult.Value.ToString();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Wishlist>>(actionResult);
            Assert.AreEqual(goodResult.StatusCode, 200);
            Assert.IsFalse(actual.Contains(expected));
            Assert.AreEqual(11, _eCommerceContext.Wishlists.Count());
        }


        [Test]
        public async Task _15Test_DeleteWishlists_ReturnsMessageThatSellerCannotDeleteWishlist()
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
            _controllerUnderTest = new WishlistsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var wishlist1 = new Wishlist()
            {
                WishlistId = 12,
                EfUserId = 22,
                ProductId = 2,
                AddedDate = new DateTime(2022, 12, 14, 12, 55, 00)
            };
            var wishlistsVM1 = new WishlistsVM()
            {
                ProductId = 2
            };

            var wishlist2 = new Wishlist()
            {
                WishlistId = 13,
                EfUserId = 23,
                ProductId = 4,
                AddedDate = new DateTime(2022, 10, 14, 12, 55, 00)
            };
            var wishlistsVM2 = new WishlistsVM()
            {
                ProductId = 4
            };

            var wishlist3 = new Wishlist()
            {
                WishlistId = 14,
                EfUserId = 24,
                ProductId = 3,
                AddedDate = new DateTime(2021, 12, 04, 12, 55, 00)
            };
            var wishlistsVM3 = new WishlistsVM()
            {
                ProductId = 3
            };

            //Act

            var actionResultDeleted = await _controllerUnderTest.DeleteWishlists(8);
            var result = (ActionResult<Wishlist>)actionResultDeleted.Result;
            var badResult = (BadRequestObjectResult)actionResultDeleted.Result;
            var expected = "Not authorised to delete wishlists";
            var actual = badResult.Value.ToString();

            //Assert
            Assert.IsInstanceOf<ActionResult<Wishlist>>(actionResultDeleted);
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(10, _eCommerceContext.Wishlists.Count());
        }



        [Test]
        public async Task _16Test_DeleteWishlists_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount_WhenAdministrator()
        {
            //Arrange
            var authdb = _authenticationContext;
            var sellerRole = authdb.Roles.FirstOrDefault(c => c.Name == "Administrator").Id;
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
            _controllerUnderTest = new WishlistsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            var wishlist2 = new Wishlist()
            {
                WishlistId = 13,
                EfUserId = 23,
                ProductId = 4,
                AddedDate = new DateTime(2022, 10, 14, 12, 55, 00)
            };
            var wishlistsVM2 = new WishlistsVM()
            {
                ProductId = 4
            };


            //Act
            var actionResult = await _controllerUnderTest.PostWishlists(wishlistsVM2);
            var actionResultDeleted = await _controllerUnderTest.DeleteWishlists(wishlist2.WishlistId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Wishlist>>(actionResultDeleted);
            Assert.AreEqual(10, _eCommerceContext.Wishlists.Count());
        }


        [Test]
        public async Task _17Test_DeleteWishlists_AddMultipleDeleteOne_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount_WhenAdministrator()
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
            _controllerUnderTest = new WishlistsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var wishlist2 = new Wishlist()
            {
                WishlistId = 13,
                EfUserId = 23,
                ProductId = 4,
                AddedDate = new DateTime(2022, 10, 14, 12, 55, 00)
            };
            var wishlistsVM2 = new WishlistsVM()
            {
                ProductId = 4
            };

            //Act
            await _controllerUnderTest.PostWishlists(_wishlistVM);
            await _controllerUnderTest.PostWishlists(wishlistsVM2);

            var actionResultDeleted = await _controllerUnderTest.DeleteWishlists(_wishlist.WishlistId);
            var actionResultDeleted2 = await _controllerUnderTest.DeleteWishlists(wishlist2.WishlistId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Wishlist>>(actionResultDeleted);
            Assert.NotNull(actionResultDeleted2);
            Assert.IsInstanceOf<ActionResult<Wishlist>>(actionResultDeleted2);
            Assert.AreEqual(10, _eCommerceContext.Wishlists.Count());
        }

        [TestCase("2020-12-14")]
        [TestCase("2023-01-14")]
        [TestCase("2023-12-15")]
        [TestCase("2023-03-17")]
        [TestCase("2023-08-18")]
        [TestCase("2023-07-12")]
        [TestCase("2023-07-01")]
        [TestCase("2023-02-02")]
        [TestCase("2023-06-14")]
        [TestCase("2023-05-13")]
        public async Task _18Test_GetWishlistByDate_ReturnsaListwithCount1_WhenCorrectWishlistDateEntered(DateTime datePassedIn)
        {
            //Arrange
            DateTime date = datePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetWishlistByDate(date);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Wishlist>)okResult.Value;
            var expected = _eCommerceContext.Wishlists.FirstOrDefault(c => c.AddedDate == date);

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Wishlist>>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.IsTrue(actual.Count() > 0);

        }

        [TestCase("2023-12-12")]
        [TestCase("2017-05-22")]
        [TestCase("2018-11-22")]
        [TestCase("2018-03-22")]
        [TestCase("2018-03-02")]
        [TestCase("2012-10-13")]
        [TestCase("2022-02-02")]
        [TestCase("2012-04-22")]
        [TestCase("2022-12-12")]
        [TestCase("2023-12-10")]
        [TestCase("2021-03-22")]
        public async Task _19Test_GetWishlistByDate_ReturnsaListwithCount0_WhenWrongWishlistDateEntered(DateTime datePassedIn)
        {
            //Arrange
            DateTime date = datePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetWishlistByDate(date);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = "No Wishlist with that date exists, please try again";
            var expected = "No Wishlist with that date exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }


        [TestCase("2023-01-01", "2023-12-12")]
        [TestCase("2020-01-01", "2022-12-12")]
        public async Task _20Test_GetWishlistByDate_ReturnsaListwithCount1_WhenCorrectWishlistDateEntered(DateTime date1PassedIn, DateTime date2PassedIn)
        {
            //Arrange
            DateTime date1 = date1PassedIn;
            DateTime date2 = date2PassedIn;

            //Act            
            var result = await _controllerUnderTest.GetWishlistByBetweenDates(date1, date2);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Wishlist>)okResult.Value;

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Wishlist>>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.IsTrue(actual.Count() > 0);

        }

        [TestCase("2012-01-01", "2012-12-12")]
        [TestCase("2013-01-01", "2013-12-12")]
        [TestCase("2014-01-01", "2014-12-12")]
        [TestCase("2015-01-01", "2015-12-12")]
        [TestCase("2016-01-01", "2016-12-12")]
        public async Task _21Test_GetWishlistByDate_ReturnsaListwithCount0_WhenWrongWishlistDateEntered(DateTime date1PassedIn, DateTime date2PassedIn)
        {
            //Arrange
            DateTime date1 = date1PassedIn;
            DateTime date2 = date2PassedIn;

            //Act            
            var result = await _controllerUnderTest.GetWishlistByBetweenDates(date1, date2);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = "No Wishlist with that date range exists, please try again";
            var expected = "No Wishlist with that date range exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }
    }
}

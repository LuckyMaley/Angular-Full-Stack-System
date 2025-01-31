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
    public class ShippingsControllerTests
    {
        private LLM_eCommerce_EFDBContext _eCommerceContext;
        private ShippingsController _controllerUnderTest;
        private List<Shipping> _shippingList;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AuthenticationContext _authenticationContext;
        ShippingsVM _shippingVM;
        Shipping _shipping;
        IdentityUser identityUser;
        ClaimsPrincipal principal;

        [SetUp]
        public void Initialiser()
        {
            _eCommerceContext = (LLM_eCommerce_EFDBContext)InMemoryContext.GeneratedDB();
            var prod = _eCommerceContext.Shippings.Count();
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
            _controllerUnderTest = new ShippingsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            _shippingList = new List<Shipping>();
            _shipping = new Shipping()
            {
                ShippingId = 11,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "15 Zuma avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "2D222236",
                DeliveryStatus = "Delivered"
            };
            _shippingVM = new ShippingsVM()
            {
                ShippingAddress = _shipping.ShippingAddress,
                ShippingMethod = _shipping.ShippingMethod,
                TrackingNumber = _shipping.TrackingNumber,
                DeliveryStatus = _shipping.DeliveryStatus
            };

        }



        [TearDown]
        public void CleanUpObject()
        {
            _eCommerceContext.Database.EnsureDeleted();
            _controllerUnderTest = null;
            _shippingList = null;
            _shipping = null;
            _userManager = null;

            _roleManager = null;
            _authenticationContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task _01Test_GetAllShipping_ReturnsListWithValidCount0()
        {
            // Arrange


            // Act
            var result = await _controllerUnderTest.GetShippings();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var shippingList = okResult.Value as List<Shipping>;
            Assert.NotNull(shippingList);
            Assert.AreEqual(10, shippingList.Count);
        }

        [Test]
        public async Task _02Test_GetAllShipping_ReturnsListWithValidCountEqualTo11()
        {
            // Arrange
            _eCommerceContext.Shippings.Add(_shipping);
            await _eCommerceContext.SaveChangesAsync();


            _controllerUnderTest = new ShippingsController(_eCommerceContext, _userManager, _roleManager, (AuthenticationContext)_authenticationContext);


            // Act
            var result = await _controllerUnderTest.GetShippings();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var shippingList = okResult.Value as List<Shipping>;
            Assert.NotNull(shippingList);
            Assert.AreEqual(11, shippingList.Count);
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
        public async Task _03Test_GetShippingByID_ReturnsaListwithCount1_WhenCorrectShippingIDEntered(int idPassedIn)
        {
            //Arrange
            int id = idPassedIn;

            //Act            
            var result = await _controllerUnderTest.GetShippings(id);
            var okResult = (OkObjectResult)result.Result;
            var actual = (Shipping)okResult.Value;
            var expected = _eCommerceContext.Shippings.FirstOrDefault(c => c.ShippingId == id);

            //Assert
            Assert.IsInstanceOf<ActionResult<Shipping>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.AreEqual(actual.ShippingId, expected.ShippingId);

        }


        [TestCase("Courier")]
        public async Task _04Test_GetShippingByTitle_ReturnsaListwithCount1_WhenCorrectShippingIDEntered(string methodPassedIn)
        {
            //Arrange
            string method = methodPassedIn;

            //Act            
            var result = await _controllerUnderTest.GetShippingByMethod(method);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Shipping>)okResult.Value;
            var expected = _eCommerceContext.Shippings.FirstOrDefault(c => c.ShippingMethod == method);

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Shipping>>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.IsTrue(actual.Count() > 0);

        }

        [TestCase("Car")]
        [TestCase("Pep")]
        [TestCase("Ship")]
        public async Task _05Test_GetShippingByName_ReturnsNotFound_WhenIncorrectShippingNameEntered(string methodPassedIn)
        {
            //Arrange
            string method = methodPassedIn;

            //Act            
            var result = await _controllerUnderTest.GetShippingByMethod(method);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = notFoundResult.Value.ToString();
            var expected = "No Shipping with that method exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }

        [Test]
        public async Task _06Test_GetShippingByID_ReturnsaBackActionResult_WhenShippingIDEnteredDoesNotExist()
        {
            //Arrange
            int id = 20;

            //Act            
            var result = await _controllerUnderTest.GetShippings(id);
            var badResult = (NotFoundObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "No Shipping with that ID exists, please try again";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 404);

        }

        [Test]
        public async Task _07Test_PostAshipping_ReturnsBadRequest_WhenEmptyShippingAdded()
        {
            //Arrange 

            //Act            
            var result = await _controllerUnderTest.PostShippings(new ShippingsVM());
            var badResult = (BadRequestObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "Cannot Add an empty shipping";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.AreEqual(10, _eCommerceContext.Shippings.Count());

        }

        [Test]
        public async Task _08Test_PutShippings_ReturnsOkObjectResult()
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
            _controllerUnderTest = new ShippingsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var shipping1 = new Shipping()
            {
                ShippingId = 12,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Zuma avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "2D223236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM1 = new ShippingsVM()
            {
                ShippingAddress = shipping1.ShippingAddress,
                ShippingMethod = shipping1.ShippingMethod,
                TrackingNumber = shipping1.TrackingNumber,
                DeliveryStatus = shipping1.DeliveryStatus
            };

            var shipping2 = new Shipping()
            {
                ShippingId = 13,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Jack avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "eD2d3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM2 = new ShippingsVM()
            {
                ShippingAddress = shipping2.ShippingAddress,
                ShippingMethod = shipping2.ShippingMethod,
                TrackingNumber = shipping2.TrackingNumber,
                DeliveryStatus = shipping2.DeliveryStatus
            };

            var shipping3 = new Shipping()
            {
                ShippingId = 14,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "17 Zuma avenue, 2023",
                ShippingMethod = "Courier",
                TrackingNumber = "2Dee3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM3 = new ShippingsVM()
            {
                ShippingAddress = shipping3.ShippingAddress,
                ShippingMethod = shipping3.ShippingMethod,
                TrackingNumber = shipping3.TrackingNumber,
                DeliveryStatus = shipping3.DeliveryStatus
            };


            //Act            
            var result1 = await _controllerUnderTest.PutShippings(1, shippingsVM1);
            var result2 = await _controllerUnderTest.PutShippings(2, shippingsVM2);
            var result3 = await _controllerUnderTest.PutShippings(3, shippingsVM3);

            var okResult1 = (OkObjectResult)result1;
            var actual1 = okResult1.Value.ToString();
            var expected1 = "Shipping Updated";

            var okResult2 = (OkObjectResult)result2;
            var actual2 = okResult2.Value.ToString();
            var expected2 = "Shipping Updated";

            var okResult3 = (OkObjectResult)result3;
            var actual3 = okResult3.Value.ToString();
            var expected3 = "Shipping Updated";

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
        public async Task _09Test_PutShippings_ReturnsNotUpdated_WhenSeller()
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
            _controllerUnderTest = new ShippingsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var shipping1 = new Shipping()
            {
                ShippingId = 12,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Zuma avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "2D223236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM1 = new ShippingsVM()
            {
                ShippingAddress = shipping1.ShippingAddress,
                ShippingMethod = shipping1.ShippingMethod,
                TrackingNumber = shipping1.TrackingNumber,
                DeliveryStatus = shipping1.DeliveryStatus
            };

            var shipping2 = new Shipping()
            {
                ShippingId = 13,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Jack avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "eD2d3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM2 = new ShippingsVM()
            {
                ShippingAddress = shipping2.ShippingAddress,
                ShippingMethod = shipping2.ShippingMethod,
                TrackingNumber = shipping2.TrackingNumber,
                DeliveryStatus = shipping2.DeliveryStatus
            };

            var shipping3 = new Shipping()
            {
                ShippingId = 14,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "17 Zuma avenue, 2023",
                ShippingMethod = "Courier",
                TrackingNumber = "2Dee3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM3 = new ShippingsVM()
            {
                ShippingAddress = shipping3.ShippingAddress,
                ShippingMethod = shipping3.ShippingMethod,
                TrackingNumber = shipping3.TrackingNumber,
                DeliveryStatus = shipping3.DeliveryStatus
            };


            //Act            
            var result1 = await _controllerUnderTest.PutShippings(1, shippingsVM1);
            var result2 = await _controllerUnderTest.PutShippings(2, shippingsVM2);
            var result3 = await _controllerUnderTest.PutShippings(3, shippingsVM3);

            var badResult1 = (BadRequestObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "Not authorised to update shippings";

            var badResult2 = (BadRequestObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "Not authorised to update shippings";

            var badResult3 = (BadRequestObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "Not authorised to update shippings";

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
        public async Task _10Test_PutShippings_ReturnsNotFoundResult()
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
            _controllerUnderTest = new ShippingsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };
            var shipping1 = new Shipping()
            {
                ShippingId = 12,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Zuma avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "2D223236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM1 = new ShippingsVM()
            {
                ShippingAddress = shipping1.ShippingAddress,
                ShippingMethod = shipping1.ShippingMethod,
                TrackingNumber = shipping1.TrackingNumber,
                DeliveryStatus = shipping1.DeliveryStatus
            };

            var shipping2 = new Shipping()
            {
                ShippingId = 13,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Jack avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "eD2d3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM2 = new ShippingsVM()
            {
                ShippingAddress = shipping2.ShippingAddress,
                ShippingMethod = shipping2.ShippingMethod,
                TrackingNumber = shipping2.TrackingNumber,
                DeliveryStatus = shipping2.DeliveryStatus
            };

            var shipping3 = new Shipping()
            {
                ShippingId = 14,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "17 Zuma avenue, 2023",
                ShippingMethod = "Courier",
                TrackingNumber = "2Dee3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM3 = new ShippingsVM()
            {
                ShippingAddress = shipping3.ShippingAddress,
                ShippingMethod = shipping3.ShippingMethod,
                TrackingNumber = shipping3.TrackingNumber,
                DeliveryStatus = shipping3.DeliveryStatus
            };




            //Act            
            var result1 = await _controllerUnderTest.PutShippings(11, shippingsVM1);
            var result2 = await _controllerUnderTest.PutShippings(12, shippingsVM2);
            var result3 = await _controllerUnderTest.PutShippings(13, shippingsVM3);

            var badResult1 = (NotFoundObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "No Shipping with that ID exists, please try again";

            var badResult2 = (NotFoundObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "No Shipping with that ID exists, please try again";

            var badResult3 = (NotFoundObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "No Shipping with that ID exists, please try again";

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
        public async Task _11Test_PostShipping_ReturnsActionResultObjectWith13Shippings_WhenAdded3Shippings()
        {
            //Arrange
            var shipping1 = new Shipping()
            {
                ShippingId = 12,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Zuma avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "2D223236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM1 = new ShippingsVM()
            {
                ShippingAddress = shipping1.ShippingAddress,
                ShippingMethod = shipping1.ShippingMethod,
                TrackingNumber = shipping1.TrackingNumber,
                DeliveryStatus = shipping1.DeliveryStatus
            };

            var shipping2 = new Shipping()
            {
                ShippingId = 13,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Jack avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "eD2d3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM2 = new ShippingsVM()
            {
                ShippingAddress = shipping2.ShippingAddress,
                ShippingMethod = shipping2.ShippingMethod,
                TrackingNumber = shipping2.TrackingNumber,
                DeliveryStatus = shipping2.DeliveryStatus
            };

            var shipping3 = new Shipping()
            {
                ShippingId = 14,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "17 Zuma avenue, 2023",
                ShippingMethod = "Courier",
                TrackingNumber = "2Dee3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM3 = new ShippingsVM()
            {
                ShippingAddress = shipping3.ShippingAddress,
                ShippingMethod = shipping3.ShippingMethod,
                TrackingNumber = shipping3.TrackingNumber,
                DeliveryStatus = shipping3.DeliveryStatus
            };



            //Act            
            var result1 = await _controllerUnderTest.PostShippings(_shippingVM);
            var result2 = await _controllerUnderTest.PostShippings(shippingsVM2);
            var result3 = await _controllerUnderTest.PostShippings(shippingsVM3);

            //Assert
            Assert.NotNull(_eCommerceContext.Shippings);
            Assert.AreEqual(13, _eCommerceContext.Shippings.Count());

        }


        [TestCase(11)]
        [TestCase(12)]
        [TestCase(10)]
        [TestCase(9)]
        [TestCase(8)]
        [TestCase(7)]
        public async Task _12Test_GetShippingByID_ReturnsWithCorrectType_WhenPassedInID(int id)
        {
            //Arrange 
            var shipping1 = new Shipping()
            {
                ShippingId = 12,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Zuma avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "2D223236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM1 = new ShippingsVM()
            {
                ShippingAddress = shipping1.ShippingAddress,
                ShippingMethod = shipping1.ShippingMethod,
                TrackingNumber = shipping1.TrackingNumber,
                DeliveryStatus = shipping1.DeliveryStatus
            };

            var shipping2 = new Shipping()
            {
                ShippingId = 13,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Jack avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "eD2d3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM2 = new ShippingsVM()
            {
                ShippingAddress = shipping2.ShippingAddress,
                ShippingMethod = shipping2.ShippingMethod,
                TrackingNumber = shipping2.TrackingNumber,
                DeliveryStatus = shipping2.DeliveryStatus
            };

            var shipping3 = new Shipping()
            {
                ShippingId = 14,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "17 Zuma avenue, 2023",
                ShippingMethod = "Courier",
                TrackingNumber = "2Dee3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM3 = new ShippingsVM()
            {
                ShippingAddress = shipping3.ShippingAddress,
                ShippingMethod = shipping3.ShippingMethod,
                TrackingNumber = shipping3.TrackingNumber,
                DeliveryStatus = shipping3.DeliveryStatus
            };



            await _controllerUnderTest.PostShippings(shippingsVM1);
            await _controllerUnderTest.PostShippings(shippingsVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetShippings(id);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Shipping>>(actionResult);
        }



        [Test]
        public async Task _12Test_GetAllshipping_ReturnsWithCorrectTypeAndCount()
        {
            //Arrange 
            var shipping1 = new Shipping()
            {
                ShippingId = 12,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Zuma avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "2D223236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM1 = new ShippingsVM()
            {
                ShippingAddress = shipping1.ShippingAddress,
                ShippingMethod = shipping1.ShippingMethod,
                TrackingNumber = shipping1.TrackingNumber,
                DeliveryStatus = shipping1.DeliveryStatus
            };

            var shipping2 = new Shipping()
            {
                ShippingId = 13,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Jack avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "eD2d3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM2 = new ShippingsVM()
            {
                ShippingAddress = shipping2.ShippingAddress,
                ShippingMethod = shipping2.ShippingMethod,
                TrackingNumber = shipping2.TrackingNumber,
                DeliveryStatus = shipping2.DeliveryStatus
            };

            var shipping3 = new Shipping()
            {
                ShippingId = 14,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "17 Zuma avenue, 2023",
                ShippingMethod = "Courier",
                TrackingNumber = "2Dee3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM3 = new ShippingsVM()
            {
                ShippingAddress = shipping3.ShippingAddress,
                ShippingMethod = shipping3.ShippingMethod,
                TrackingNumber = shipping3.TrackingNumber,
                DeliveryStatus = shipping3.DeliveryStatus
            };



            await _controllerUnderTest.PostShippings(shippingsVM1);
            await _controllerUnderTest.PostShippings(shippingsVM2);
            await _controllerUnderTest.PostShippings(shippingsVM3);

            //Act            
            var actionResult = await _controllerUnderTest.GetShippings();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<IEnumerable<Shipping>>>(actionResult);
            var result = (OkObjectResult)actionResult.Result;
            var value = (List<Shipping>)result.Value;
            Assert.AreEqual(_eCommerceContext.Shippings.Count(), value.Count);
        }

        [Test]
        public async Task _13Test_GetshippingById_ReturnsWithCorrectType()
        {
            //Arrange 
            var shipping1 = new Shipping()
            {
                ShippingId = 12,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Zuma avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "2D223236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM1 = new ShippingsVM()
            {
                ShippingAddress = shipping1.ShippingAddress,
                ShippingMethod = shipping1.ShippingMethod,
                TrackingNumber = shipping1.TrackingNumber,
                DeliveryStatus = shipping1.DeliveryStatus
            };

            var shipping2 = new Shipping()
            {
                ShippingId = 13,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Jack avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "eD2d3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM2 = new ShippingsVM()
            {
                ShippingAddress = shipping2.ShippingAddress,
                ShippingMethod = shipping2.ShippingMethod,
                TrackingNumber = shipping2.TrackingNumber,
                DeliveryStatus = shipping2.DeliveryStatus
            };

            var shipping3 = new Shipping()
            {
                ShippingId = 14,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "17 Zuma avenue, 2023",
                ShippingMethod = "Courier",
                TrackingNumber = "2Dee3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM3 = new ShippingsVM()
            {
                ShippingAddress = shipping3.ShippingAddress,
                ShippingMethod = shipping3.ShippingMethod,
                TrackingNumber = shipping3.TrackingNumber,
                DeliveryStatus = shipping3.DeliveryStatus
            };



            await _controllerUnderTest.PostShippings(_shippingVM);
            await _controllerUnderTest.PostShippings(shippingsVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetShippings(11);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Shipping>>(actionResult);
        }


        [Test]
        public async Task _14Test_PostShippings_NotAddedAndShowsInContextCount_WhenUserIsASeller()
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
            _controllerUnderTest = new ShippingsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            //Act            
            var actionResult = await _controllerUnderTest.PostShippings(_shippingVM);
            //Assert
            Assert.IsFalse(_eCommerceContext.Shippings.Where(c => c.ShippingId == 11).Count() > 0);
            Assert.IsTrue(_eCommerceContext.Shippings.Count() == 10);

        }


        [Test]
        public async Task _15Test_PostShippings_ReturnsBadObjectResult_WhenUserIsACustomer()
        {
            //Arrange

            //Act            
            var actionResult = await _controllerUnderTest.PostShippings(_shippingVM);

            //Assert
            var result = (ActionResult<Shipping>)actionResult.Result;
            var goodResult = (OkObjectResult)actionResult.Result;
            var expected = "Not authorised to add shippings as a customer";
            var actual = goodResult.Value.ToString();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Shipping>>(actionResult);
            Assert.AreEqual(goodResult.StatusCode, 200);
            Assert.IsFalse(actual.Contains(expected));
            Assert.AreEqual(11, _eCommerceContext.Shippings.Count());
        }


        [Test]
        public async Task _16Test_DeleteShippings_ReturnsMessageThatSellerCannotDeleteShipping()
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
            _controllerUnderTest = new ShippingsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var shipping1 = new Shipping()
            {
                ShippingId = 12,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Zuma avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "2D223236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM1 = new ShippingsVM()
            {
                ShippingAddress = shipping1.ShippingAddress,
                ShippingMethod = shipping1.ShippingMethod,
                TrackingNumber = shipping1.TrackingNumber,
                DeliveryStatus = shipping1.DeliveryStatus
            };

            var shipping2 = new Shipping()
            {
                ShippingId = 13,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Jack avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "eD2d3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM2 = new ShippingsVM()
            {
                ShippingAddress = shipping2.ShippingAddress,
                ShippingMethod = shipping2.ShippingMethod,
                TrackingNumber = shipping2.TrackingNumber,
                DeliveryStatus = shipping2.DeliveryStatus
            };

            var shipping3 = new Shipping()
            {
                ShippingId = 14,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "17 Zuma avenue, 2023",
                ShippingMethod = "Courier",
                TrackingNumber = "2Dee3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM3 = new ShippingsVM()
            {
                ShippingAddress = shipping3.ShippingAddress,
                ShippingMethod = shipping3.ShippingMethod,
                TrackingNumber = shipping3.TrackingNumber,
                DeliveryStatus = shipping3.DeliveryStatus
            };



            //Act

            var actionResultDeleted = await _controllerUnderTest.DeleteShippings(8);
            var result = (ActionResult<Shipping>)actionResultDeleted.Result;
            var badResult = (BadRequestObjectResult)actionResultDeleted.Result;
            var expected = "Not authorised to delete shippings";
            var actual = badResult.Value.ToString();

            //Assert
            Assert.IsInstanceOf<ActionResult<Shipping>>(actionResultDeleted);
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(10, _eCommerceContext.Shippings.Count());
        }



        [Test]
        public async Task _17Test_DeleteShippings_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount_WhenAdministrator()
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
            _controllerUnderTest = new ShippingsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            var shipping2 = new Shipping()
            {
                ShippingId = 13,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Jack avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "eD2d3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM2 = new ShippingsVM()
            {
                ShippingAddress = shipping2.ShippingAddress,
                ShippingMethod = shipping2.ShippingMethod,
                TrackingNumber = shipping2.TrackingNumber,
                DeliveryStatus = shipping2.DeliveryStatus
            };

            
            //Act
            var actionResult = await _controllerUnderTest.PostShippings(shippingsVM2);
            var actionResultDeleted = await _controllerUnderTest.DeleteShippings(shipping2.ShippingId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Shipping>>(actionResultDeleted);
            Assert.AreEqual(10, _eCommerceContext.Shippings.Count());
        }


        [Test]
        public async Task _18Test_DeleteShippings_AddMultipleDeleteOne_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount_WhenAdministrator()
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
            _controllerUnderTest = new ShippingsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var shipping2 = new Shipping()
            {
                ShippingId = 13,
                ShippingDate = new DateTime(2023, 11, 14, 12, 55, 00),
                ShippingAddress = "16 Jack avenue, 2001",
                ShippingMethod = "Courier",
                TrackingNumber = "eD2d3236",
                DeliveryStatus = "Pending"
            };
            var shippingsVM2 = new ShippingsVM()
            {
                ShippingAddress = shipping2.ShippingAddress,
                ShippingMethod = shipping2.ShippingMethod,
                TrackingNumber = shipping2.TrackingNumber,
                DeliveryStatus = shipping2.DeliveryStatus
            };

            //Act
            await _controllerUnderTest.PostShippings(_shippingVM);
            await _controllerUnderTest.PostShippings(shippingsVM2);

            var actionResultDeleted = await _controllerUnderTest.DeleteShippings(_shipping.ShippingId);
            var actionResultDeleted2 = await _controllerUnderTest.DeleteShippings(shipping2.ShippingId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Shipping>>(actionResultDeleted);
            Assert.NotNull(actionResultDeleted2);
            Assert.IsInstanceOf<ActionResult<Shipping>>(actionResultDeleted2);
            Assert.AreEqual(10, _eCommerceContext.Shippings.Count());
        }

        [TestCase("2023-11-14")]
        [TestCase("2017-04-25")]
        [TestCase("2019-11-16")]
        [TestCase("2018-05-16")]
        [TestCase("2018-03-16")]
        [TestCase("2019-10-17")]
        [TestCase("2020-02-05")]
        [TestCase("2021-12-16")]
        [TestCase("2022-12-14")]
        [TestCase("2020-03-25")]
        public async Task _19Test_GetShippingByDate_ReturnsaListwithCount1_WhenCorrectShippingDateEntered(DateTime datePassedIn)
        {
            //Arrange
            DateTime date = datePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetShippingByDate(date);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Shipping>)okResult.Value;
            var expected = _eCommerceContext.Shippings.FirstOrDefault(c => c.ShippingDate == date);

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Shipping>>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.IsTrue(actual.Count() > 0);

        }

        [TestCase("2015-12-12")]
        [TestCase("2016-05-22")]
        [TestCase("2016-11-22")]
        [TestCase("2014-03-22")]
        [TestCase("2015-03-02")]
        [TestCase("2013-10-13")]
        [TestCase("2014-02-02")]
        [TestCase("2013-04-22")]
        [TestCase("2013-12-12")]
        [TestCase("2013-12-10")]
        [TestCase("2020-01-01")]
        public async Task _20Test_GetShippingByDate_ReturnsaListwithCount0_WhenWrongShippingDateEntered(DateTime datePassedIn)
        {
            //Arrange
            DateTime date = datePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetShippingByDate(date);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = "No Shipping with that date exists, please try again";
            var expected = "No Shipping with that date exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }


        [TestCase("2017-01-01", "2017-12-12")]
        [TestCase("2018-01-01", "2018-12-12")]
        [TestCase("2019-01-01", "2019-12-12")]
        [TestCase("2020-01-01", "2022-12-12")]
        [TestCase("2021-01-01", "2023-12-12")]
        public async Task _21Test_GetShippingByDate_ReturnsaListwithCount1_WhenCorrectShippingDateEntered(DateTime date1PassedIn, DateTime date2PassedIn)
        {
            //Arrange
            DateTime date1 = date1PassedIn;
            DateTime date2 = date2PassedIn;

            //Act            
            var result = await _controllerUnderTest.GetShippingByBetweenDates(date1, date2);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Shipping>)okResult.Value;

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Shipping>>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.IsTrue(actual.Count() > 0);

        }

        [TestCase("2012-01-01", "2012-12-12")]
        [TestCase("2013-01-01", "2013-12-12")]
        [TestCase("2014-01-01", "2014-12-12")]
        [TestCase("2015-01-01", "2015-12-12")]
        [TestCase("2016-01-01", "2016-12-12")]
        public async Task _22Test_GetShippingByDate_ReturnsaListwithCount0_WhenWrongShippingDateEntered(DateTime date1PassedIn, DateTime date2PassedIn)
        {
            //Arrange
            DateTime date1 = date1PassedIn;
            DateTime date2 = date2PassedIn;

            //Act            
            var result = await _controllerUnderTest.GetShippingByBetweenDates(date1, date2);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = "No Shipping with that date range exists, please try again";
            var expected = "No Shipping with that date range exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }
    }
}

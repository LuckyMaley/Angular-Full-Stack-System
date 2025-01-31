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
    public class OrderDetailsControllerTests
    {
        private LLM_eCommerce_EFDBContext _eCommerceContext;
        private OrderDetailsController _controllerUnderTest;
        private List<OrderDetail> _orderDetailList;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AuthenticationContext _authenticationContext;
        OrderDetailsVM _orderDetailVM;
        OrderDetail _orderDetail;
        IdentityUser identityUser;
        ClaimsPrincipal principal;

        [SetUp]
        public void Initialiser()
        {
            _eCommerceContext = (LLM_eCommerce_EFDBContext)InMemoryContext.GeneratedDB();
            var prod = _eCommerceContext.OrderDetails.Count();
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
            _controllerUnderTest = new OrderDetailsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            _orderDetailList = new List<OrderDetail>();
            _orderDetail = new OrderDetail()
            {
                OrderDetailId = 11,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 2100.99
            };
            _orderDetailVM = new OrderDetailsVM()
            {
                OrderId = _orderDetail.OrderId,
                ProductId = _orderDetail.ProductId,
                Quantity = _orderDetail.Quantity,
                UnitPrice = _orderDetail.UnitPrice
            };

        }



        [TearDown]
        public void CleanUpObject()
        {
            _eCommerceContext.Database.EnsureDeleted();
            _controllerUnderTest = null;
            _orderDetailList = null;
            _orderDetail = null;
            _userManager = null;

            _roleManager = null;
            _authenticationContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task _01Test_GetAllOrderDetail_ReturnsListWithValidCount0()
        {
            // Arrange


            // Act
            var result = await _controllerUnderTest.GetOrderDetails();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var orderDetailList = okResult.Value as List<OrderDetail>;
            Assert.NotNull(orderDetailList);
            Assert.AreEqual(10, orderDetailList.Count);
        }

        [Test]
        public async Task _02Test_GetAllOrderDetail_ReturnsListWithValidCountEqualTo11()
        {
            // Arrange
            _eCommerceContext.OrderDetails.Add(_orderDetail);
            await _eCommerceContext.SaveChangesAsync();


            _controllerUnderTest = new OrderDetailsController(_eCommerceContext, _userManager, _roleManager, (AuthenticationContext)_authenticationContext);


            // Act
            var result = await _controllerUnderTest.GetOrderDetails();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var orderDetailList = okResult.Value as List<OrderDetail>;
            Assert.NotNull(orderDetailList);
            Assert.AreEqual(11, orderDetailList.Count);
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
        public async Task _03Test_GetOrderDetailByID_ReturnsaListwithCount1_WhenCorrectOrderDetailIDEntered(int idPassedIn)
        {
            //Arrange
            int id = idPassedIn;

            //Act            
            var result = await _controllerUnderTest.GetOrderDetails(id);
            var okResult = (OkObjectResult)result.Result;
            var actual = (OrderDetail)okResult.Value;
            var expected = _eCommerceContext.OrderDetails.FirstOrDefault(c => c.OrderDetailId == id);

            //Assert
            Assert.IsInstanceOf<ActionResult<OrderDetail>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.AreEqual(actual.OrderDetailId, expected.OrderDetailId);

        }


        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(16)]
        [TestCase(17)]
        [TestCase(18)]
        [TestCase(19)]
        [TestCase(20)]
        public async Task _04Test_GetOrderDetailByid_ReturnsaListwithCount1_WhenIncorrectOrderDetailIDEntered(int idPassedIn)
        {
            //Arrange
            int id = idPassedIn;

            //Act            
            var result = await _controllerUnderTest.GetOrderDetails(id);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = "No OrderDetail with that id exists, please try again";
            var expected = "No OrderDetail with that id exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }

        [Test]
        public async Task _05Test_PostAorderDetail_ReturnsBadRequest_WhenEmptyOrderDetailAdded()
        {
            //Arrange 

            //Act            
            var result = await _controllerUnderTest.PostOrderDetails(new OrderDetailsVM());
            var badResult = (BadRequestObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "Cannot Add an empty order detail";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.AreEqual(10, _eCommerceContext.OrderDetails.Count());

        }

        [Test]
        public async Task _06Test_PutOrderDetails_ReturnsOkObjectResult()
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
            _controllerUnderTest = new OrderDetailsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var orderDetail1 = new OrderDetail()
            {
                OrderDetailId = 11,
                OrderId = 2,
                ProductId = 2,
                Quantity = 6,
                UnitPrice = 2300.99
            };
            var orderDetailsVM1 = new OrderDetailsVM()
            {
                OrderId = orderDetail1.OrderId,
                ProductId = orderDetail1.ProductId,
                Quantity = orderDetail1.Quantity,
                UnitPrice = orderDetail1.UnitPrice
            };

            var orderDetail2 = new OrderDetail()
            {
                OrderDetailId = 12,
                OrderId = 2,
                ProductId = 3,
                Quantity = 2,
                UnitPrice = 1400.99
            };
            var orderDetailsVM2 = new OrderDetailsVM()
            {
                OrderId = orderDetail1.OrderId,
                ProductId = orderDetail1.ProductId,
                Quantity = orderDetail1.Quantity,
                UnitPrice = orderDetail1.UnitPrice
            };

            var orderDetail3 = new OrderDetail()
            {
                OrderDetailId = 13,
                OrderId = 1,
                ProductId = 10,
                Quantity = 4,
                UnitPrice = 2400.99
            };
            var orderDetailsVM3 = new OrderDetailsVM()
            {
                OrderId = orderDetail1.OrderId,
                ProductId = orderDetail1.ProductId,
                Quantity = orderDetail1.Quantity,
                UnitPrice = orderDetail1.UnitPrice
            };


            //Act            
            var result1 = await _controllerUnderTest.PutOrderDetails(1, orderDetailsVM1);
            var result2 = await _controllerUnderTest.PutOrderDetails(2, orderDetailsVM2);
            var result3 = await _controllerUnderTest.PutOrderDetails(3, orderDetailsVM3);

            var okResult1 = (OkObjectResult)result1;
            var actual1 = okResult1.Value.ToString();
            var expected1 = "OrderDetail Updated";

            var okResult2 = (OkObjectResult)result2;
            var actual2 = okResult2.Value.ToString();
            var expected2 = "OrderDetail Updated";

            var okResult3 = (OkObjectResult)result3;
            var actual3 = okResult3.Value.ToString();
            var expected3 = "OrderDetail Updated";

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
        public async Task _07Test_PutOrderDetails_ReturnsNotUpdated_WhenSeller()
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
            _controllerUnderTest = new OrderDetailsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var orderDetail1 = new OrderDetail()
            {
                OrderDetailId = 11,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 2100.99
            };
            var orderDetailsVM1 = new OrderDetailsVM()
            {
                OrderId = orderDetail1.OrderId,
                ProductId = orderDetail1.ProductId,
                Quantity = orderDetail1.Quantity,
                UnitPrice = orderDetail1.UnitPrice
            };

            var orderDetail2 = new OrderDetail()
            {
                OrderDetailId = 12,
                OrderId = 1,
                ProductId = 3,
                Quantity = 2,
                UnitPrice = 1100.99
            };
            var orderDetailsVM2 = new OrderDetailsVM()
            {
                OrderId = orderDetail2.OrderId,
                ProductId = orderDetail2.ProductId,
                Quantity = orderDetail2.Quantity,
                UnitPrice = orderDetail2.UnitPrice
            };

            var orderDetail3 = new OrderDetail()
            {
                OrderDetailId = 13,
                OrderId = 1,
                ProductId = 10,
                Quantity = 1,
                UnitPrice = 2400.99
            };
            var orderDetailsVM3 = new OrderDetailsVM()
            {
                OrderId = orderDetail3.OrderId,
                ProductId = orderDetail3.ProductId,
                Quantity = orderDetail3.Quantity,
                UnitPrice = orderDetail3.UnitPrice
            };

            //Act            
            var result1 = await _controllerUnderTest.PutOrderDetails(1, orderDetailsVM1);
            var result2 = await _controllerUnderTest.PutOrderDetails(2, orderDetailsVM2);
            var result3 = await _controllerUnderTest.PutOrderDetails(3, orderDetailsVM3);

            var badResult1 = (BadRequestObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "Not authorised to update orderDetails";

            var badResult2 = (BadRequestObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "Not authorised to update orderDetails";

            var badResult3 = (BadRequestObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "Not authorised to update orderDetails";

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
        public async Task _08Test_PutOrderDetails_ReturnsNotFoundResult()
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
            _controllerUnderTest = new OrderDetailsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };
            var orderDetail1 = new OrderDetail()
            {
                OrderDetailId = 11,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 2100.99
            };
            var orderDetailsVM1 = new OrderDetailsVM()
            {
                OrderId = orderDetail1.OrderId,
                ProductId = orderDetail1.ProductId,
                Quantity = orderDetail1.Quantity,
                UnitPrice = orderDetail1.UnitPrice
            };

            var orderDetail2 = new OrderDetail()
            {
                OrderDetailId = 12,
                OrderId = 1,
                ProductId = 3,
                Quantity = 2,
                UnitPrice = 1100.99
            };
            var orderDetailsVM2 = new OrderDetailsVM()
            {
                OrderId = orderDetail2.OrderId,
                ProductId = orderDetail2.ProductId,
                Quantity = orderDetail2.Quantity,
                UnitPrice = orderDetail2.UnitPrice
            };

            var orderDetail3 = new OrderDetail()
            {
                OrderDetailId = 13,
                OrderId = 1,
                ProductId = 10,
                Quantity = 1,
                UnitPrice = 2400.99
            };
            var orderDetailsVM3 = new OrderDetailsVM()
            {
                OrderId = orderDetail3.OrderId,
                ProductId = orderDetail3.ProductId,
                Quantity = orderDetail3.Quantity,
                UnitPrice = orderDetail3.UnitPrice
            };


            //Act            
            var result1 = await _controllerUnderTest.PutOrderDetails(11, orderDetailsVM1);
            var result2 = await _controllerUnderTest.PutOrderDetails(12, orderDetailsVM2);
            var result3 = await _controllerUnderTest.PutOrderDetails(13, orderDetailsVM3);

            var badResult1 = (NotFoundObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "No OrderDetail with that ID exists, please try again";

            var badResult2 = (NotFoundObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "No OrderDetail with that ID exists, please try again";

            var badResult3 = (NotFoundObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "No OrderDetail with that ID exists, please try again";

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
        public async Task _09Test_PostOrderDetail_ReturnsActionResultObjectWith13OrderDetails_WhenAdded3OrderDetails()
        {
            //Arrange
            var orderDetail1 = new OrderDetail()
            {
                OrderDetailId = 11,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 2100.99
            };
            var orderDetailsVM1 = new OrderDetailsVM()
            {
                OrderId = orderDetail1.OrderId,
                ProductId = orderDetail1.ProductId,
                Quantity = orderDetail1.Quantity,
                UnitPrice = orderDetail1.UnitPrice
            };

            var orderDetail2 = new OrderDetail()
            {
                OrderDetailId = 12,
                OrderId = 1,
                ProductId = 3,
                Quantity = 2,
                UnitPrice = 1100.99
            };
            var orderDetailsVM2 = new OrderDetailsVM()
            {
                OrderId = orderDetail2.OrderId,
                ProductId = orderDetail2.ProductId,
                Quantity = orderDetail2.Quantity,
                UnitPrice = orderDetail2.UnitPrice
            };

            var orderDetail3 = new OrderDetail()
            {
                OrderDetailId = 13,
                OrderId = 1,
                ProductId = 10,
                Quantity = 1,
                UnitPrice = 2400.99
            };
            var orderDetailsVM3 = new OrderDetailsVM()
            {
                OrderId = orderDetail3.OrderId,
                ProductId = orderDetail3.ProductId,
                Quantity = orderDetail3.Quantity,
                UnitPrice = orderDetail3.UnitPrice
            };

            //Act            
            var result1 = await _controllerUnderTest.PostOrderDetails(_orderDetailVM);
            var result2 = await _controllerUnderTest.PostOrderDetails(orderDetailsVM2);
            var result3 = await _controllerUnderTest.PostOrderDetails(orderDetailsVM3);

            //Assert
            Assert.NotNull(_eCommerceContext.OrderDetails);
            Assert.AreEqual(13, _eCommerceContext.OrderDetails.Count());

        }


        [TestCase(11)]
        [TestCase(12)]
        [TestCase(10)]
        [TestCase(9)]
        [TestCase(8)]
        [TestCase(7)]
        public async Task _10Test_GetOrderDetailByID_ReturnsWithCorrectType_WhenPassedInID(int id)
        {
            //Arrange 
            var orderDetail1 = new OrderDetail()
            {
                OrderDetailId = 11,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 2100.99
            };
            var orderDetailsVM1 = new OrderDetailsVM()
            {
                OrderId = orderDetail1.OrderId,
                ProductId = orderDetail1.ProductId,
                Quantity = orderDetail1.Quantity,
                UnitPrice = orderDetail1.UnitPrice
            };

            var orderDetail2 = new OrderDetail()
            {
                OrderDetailId = 12,
                OrderId = 1,
                ProductId = 3,
                Quantity = 2,
                UnitPrice = 1100.99
            };
            var orderDetailsVM2 = new OrderDetailsVM()
            {
                OrderId = orderDetail2.OrderId,
                ProductId = orderDetail2.ProductId,
                Quantity = orderDetail2.Quantity,
                UnitPrice = orderDetail2.UnitPrice
            };

            var orderDetail3 = new OrderDetail()
            {
                OrderDetailId = 13,
                OrderId = 1,
                ProductId = 10,
                Quantity = 1,
                UnitPrice = 2400.99
            };
            var orderDetailsVM3 = new OrderDetailsVM()
            {
                OrderId = orderDetail3.OrderId,
                ProductId = orderDetail3.ProductId,
                Quantity = orderDetail3.Quantity,
                UnitPrice = orderDetail3.UnitPrice
            };

            await _controllerUnderTest.PostOrderDetails(orderDetailsVM1);
            await _controllerUnderTest.PostOrderDetails(orderDetailsVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetOrderDetails(id);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<OrderDetail>>(actionResult);
        }



        [Test]
        public async Task _11Test_GetAllorderDetail_ReturnsWithCorrectTypeAndCount()
        {
            //Arrange 
            var orderDetail1 = new OrderDetail()
            {
                OrderDetailId = 11,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 2100.99
            };
            var orderDetailsVM1 = new OrderDetailsVM()
            {
                OrderId = orderDetail1.OrderId,
                ProductId = orderDetail1.ProductId,
                Quantity = orderDetail1.Quantity,
                UnitPrice = orderDetail1.UnitPrice
            };

            var orderDetail2 = new OrderDetail()
            {
                OrderDetailId = 12,
                OrderId = 1,
                ProductId = 3,
                Quantity = 2,
                UnitPrice = 1100.99
            };
            var orderDetailsVM2 = new OrderDetailsVM()
            {
                OrderId = orderDetail1.OrderId,
                ProductId = orderDetail1.ProductId,
                Quantity = orderDetail1.Quantity,
                UnitPrice = orderDetail1.UnitPrice
            };

            var orderDetail3 = new OrderDetail()
            {
                OrderDetailId = 13,
                OrderId = 1,
                ProductId = 10,
                Quantity = 1,
                UnitPrice = 2400.99
            };
            var orderDetailsVM3 = new OrderDetailsVM()
            {
                OrderId = orderDetail1.OrderId,
                ProductId = orderDetail1.ProductId,
                Quantity = orderDetail1.Quantity,
                UnitPrice = orderDetail1.UnitPrice
            };

            await _controllerUnderTest.PostOrderDetails(orderDetailsVM1);
            await _controllerUnderTest.PostOrderDetails(orderDetailsVM2);
            await _controllerUnderTest.PostOrderDetails(orderDetailsVM3);

            //Act            
            var actionResult = await _controllerUnderTest.GetOrderDetails();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<IEnumerable<OrderDetail>>>(actionResult);
            var result = (OkObjectResult)actionResult.Result;
            var value = (List<OrderDetail>)result.Value;
            Assert.AreEqual(_eCommerceContext.OrderDetails.Count(), value.Count);
        }

        [Test]
        public async Task _12Test_GetorderDetailById_ReturnsWithCorrectType()
        {
            //Arrange 
            var orderDetail1 = new OrderDetail()
            {
                OrderDetailId = 11,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 2100.99
            };
            var orderDetailsVM1 = new OrderDetailsVM()
            {
                OrderId = orderDetail1.OrderId,
                ProductId = orderDetail1.ProductId,
                Quantity = orderDetail1.Quantity,
                UnitPrice = orderDetail1.UnitPrice
            };

            var orderDetail2 = new OrderDetail()
            {
                OrderDetailId = 12,
                OrderId = 1,
                ProductId = 3,
                Quantity = 2,
                UnitPrice = 1100.99
            };
            var orderDetailsVM2 = new OrderDetailsVM()
            {
                OrderId = orderDetail2.OrderId,
                ProductId = orderDetail2.ProductId,
                Quantity = orderDetail2.Quantity,
                UnitPrice = orderDetail2.UnitPrice
            };

            var orderDetail3 = new OrderDetail()
            {
                OrderDetailId = 13,
                OrderId = 1,
                ProductId = 10,
                Quantity = 1,
                UnitPrice = 2400.99
            };
            var orderDetailsVM3 = new OrderDetailsVM()
            {
                OrderId = orderDetail3.OrderId,
                ProductId = orderDetail3.ProductId,
                Quantity = orderDetail3.Quantity,
                UnitPrice = orderDetail3.UnitPrice
            };


            await _controllerUnderTest.PostOrderDetails(_orderDetailVM);
            await _controllerUnderTest.PostOrderDetails(orderDetailsVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetOrderDetails(11);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<OrderDetail>>(actionResult);
        }


        [Test]
        public async Task _13Test_PostOrderDetails_NotAddedAndShowsInContextCount_WhenUserIsASeller()
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
            _controllerUnderTest = new OrderDetailsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            //Act            
            var actionResult = await _controllerUnderTest.PostOrderDetails(_orderDetailVM);
            //Assert
            Assert.IsFalse(_eCommerceContext.OrderDetails.Where(c => c.OrderDetailId == 11).Count() > 0);
            Assert.IsTrue(_eCommerceContext.OrderDetails.Count() == 10);

        }


        [Test]
        public async Task _14Test_PostOrderDetails_ReturnsBadObjectResult_WhenUserIsACustomer()
        {
            //Arrange

            //Act            
            var actionResult = await _controllerUnderTest.PostOrderDetails(_orderDetailVM);

            //Assert
            var result = (ActionResult<OrderDetail>)actionResult.Result;
            var goodResult = (OkObjectResult)actionResult.Result;
            var expected = "Not authorised to add orderDetails as a customer";
            var actual = goodResult.Value.ToString();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<OrderDetail>>(actionResult);
            Assert.AreEqual(goodResult.StatusCode, 200);
            Assert.IsFalse(actual.Contains(expected));
            Assert.AreEqual(11, _eCommerceContext.OrderDetails.Count());
        }


        [Test]
        public async Task _16Test_DeleteOrderDetails_ReturnsMessageThatSellerCannotDeleteOrderDetail()
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
            _controllerUnderTest = new OrderDetailsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var orderDetail1 = new OrderDetail()
            {
                OrderDetailId = 11,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 2100.99
            };
            var orderDetailsVM1 = new OrderDetailsVM()
            {
                OrderId = orderDetail1.OrderId,
                ProductId = orderDetail1.ProductId,
                Quantity = orderDetail1.Quantity,
                UnitPrice = orderDetail1.UnitPrice
            };

            var orderDetail2 = new OrderDetail()
            {
                OrderDetailId = 12,
                OrderId = 1,
                ProductId = 3,
                Quantity = 2,
                UnitPrice = 1100.99
            };
            var orderDetailsVM2 = new OrderDetailsVM()
            {
                OrderId = orderDetail2.OrderId,
                ProductId = orderDetail2.ProductId,
                Quantity = orderDetail2.Quantity,
                UnitPrice = orderDetail2.UnitPrice
            };

            var orderDetail3 = new OrderDetail()
            {
                OrderDetailId = 13,
                OrderId = 1,
                ProductId = 10,
                Quantity = 1,
                UnitPrice = 2400.99
            };
            var orderDetailsVM3 = new OrderDetailsVM()
            {
                OrderId = orderDetail3.OrderId,
                ProductId = orderDetail3.ProductId,
                Quantity = orderDetail3.Quantity,
                UnitPrice = orderDetail3.UnitPrice
            };

            //Act

            var actionResultDeleted = await _controllerUnderTest.DeleteOrderDetails(8);
            var result = (ActionResult<OrderDetail>)actionResultDeleted.Result;
            var badResult = (BadRequestObjectResult)actionResultDeleted.Result;
            var expected = "Not authorised to delete orderDetails";
            var actual = badResult.Value.ToString();

            //Assert
            Assert.IsInstanceOf<ActionResult<OrderDetail>>(actionResultDeleted);
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(10, _eCommerceContext.OrderDetails.Count());
        }



        [Test]
        public async Task _15Test_DeleteOrderDetails_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount_WhenAdministrator()
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
            _controllerUnderTest = new OrderDetailsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            var orderDetail2 = new OrderDetail()
            {
                OrderDetailId = 12,
                OrderId = 1,
                ProductId = 3,
                Quantity = 2,
                UnitPrice = 1100.99
            };
            var orderDetailsVM2 = new OrderDetailsVM()
            {
                OrderId = orderDetail2.OrderId,
                ProductId = orderDetail2.ProductId,
                Quantity = orderDetail2.Quantity,
                UnitPrice = orderDetail2.UnitPrice
            };
            

            //Act
            var actionResult = await _controllerUnderTest.PostOrderDetails(orderDetailsVM2);
            var actionResultDeleted = await _controllerUnderTest.DeleteOrderDetails(orderDetail2.OrderDetailId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<OrderDetail>>(actionResultDeleted);
            Assert.AreEqual(10, _eCommerceContext.OrderDetails.Count());
        }


        [Test]
        public async Task _16Test_DeleteOrderDetails_AddMultipleDeleteOne_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount_WhenAdministrator()
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
            _controllerUnderTest = new OrderDetailsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            var orderDetail2 = new OrderDetail()
            {
                OrderDetailId = 12,
                OrderId = 1,
                ProductId = 3,
                Quantity = 2,
                UnitPrice = 1100.99
            };
            var orderDetailsVM2 = new OrderDetailsVM()
            {
                OrderId = orderDetail2.OrderId,
                ProductId = orderDetail2.ProductId,
                Quantity = orderDetail2.Quantity,
                UnitPrice = orderDetail2.UnitPrice
            };

            //Act
            await _controllerUnderTest.PostOrderDetails(_orderDetailVM);
            await _controllerUnderTest.PostOrderDetails(orderDetailsVM2);

            var actionResultDeleted = await _controllerUnderTest.DeleteOrderDetails(_orderDetail.OrderDetailId);
            var actionResultDeleted2 = await _controllerUnderTest.DeleteOrderDetails(orderDetail2.OrderDetailId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<OrderDetail>>(actionResultDeleted);
            Assert.NotNull(actionResultDeleted2);
            Assert.IsInstanceOf<ActionResult<OrderDetail>>(actionResultDeleted2);
            Assert.AreEqual(10, _eCommerceContext.OrderDetails.Count());
        }
    }
}

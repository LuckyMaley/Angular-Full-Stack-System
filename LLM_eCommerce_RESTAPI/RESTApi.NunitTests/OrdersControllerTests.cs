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
    public class OrdersControllerTests
    {
        private LLM_eCommerce_EFDBContext _eCommerceContext;
        private OrdersController _controllerUnderTest;
        private List<Order> _orderList;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AuthenticationContext _authenticationContext;
        OrdersVM _orderVM;
        Order _order;
        IdentityUser identityUser;
        ClaimsPrincipal principal;

        [SetUp]
        public void Initialiser()
        {
            _eCommerceContext = (LLM_eCommerce_EFDBContext)InMemoryContext.GeneratedDB();
            var prod = _eCommerceContext.Orders.Count();
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
            _controllerUnderTest = new OrdersController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            _orderList = new List<Order>();
            _order = new Order()
            {
                OrderId = 11,
                EfUserId = 21,
                ShippingId = 1,
                OrderDate = new DateTime(2023, 11, 12, 12, 45, 00),
                TotalAmount = 2100.99
            };
            _orderVM = new OrdersVM()
            {
                TotalAmount = _order.TotalAmount
            };

        }



        [TearDown]
        public void CleanUpObject()
        {
            _eCommerceContext.Database.EnsureDeleted();
            _controllerUnderTest = null;
            _orderList = null;
            _order = null;
            _userManager = null;

            _roleManager = null;
            _authenticationContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task _01Test_GetAllOrder_ReturnsListWithValidCount0()
        {
            // Arrange


            // Act
            var result = await _controllerUnderTest.GetOrders();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var orderList = okResult.Value as List<Order>;
            Assert.NotNull(orderList);
            Assert.AreEqual(10, orderList.Count);
        }

        [Test]
        public async Task _02Test_GetAllOrder_ReturnsListWithValidCountEqualTo11()
        {
            // Arrange
            _eCommerceContext.Orders.Add(_order);
            await _eCommerceContext.SaveChangesAsync();


            _controllerUnderTest = new OrdersController(_eCommerceContext, _userManager, _roleManager, (AuthenticationContext)_authenticationContext);


            // Act
            var result = await _controllerUnderTest.GetOrders();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var orderList = okResult.Value as List<Order>;
            Assert.NotNull(orderList);
            Assert.AreEqual(11, orderList.Count);
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
        public async Task _03Test_GetOrderByID_ReturnsaListwithCount1_WhenCorrectOrderIDEntered(int idPassedIn)
        {
            //Arrange
            int id = idPassedIn;

            //Act            
            var result = await _controllerUnderTest.GetOrders(id);
            var okResult = (OkObjectResult)result.Result;
            var actual = (Order)okResult.Value;
            var expected = _eCommerceContext.Orders.FirstOrDefault(c => c.OrderId == id);

            //Assert
            Assert.IsInstanceOf<ActionResult<Order>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.AreEqual(actual.OrderId, expected.OrderId);

        }


        [Test]
        public async Task _04Test_GetOrderByID_ReturnsaBackActionResult_WhenOrderIDEnteredDoesNotExist()
        {
            //Arrange
            int id = 20;

            //Act            
            var result = await _controllerUnderTest.GetOrders(id);
            var badResult = (NotFoundObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "No Order with that ID exists, please try again";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 404);

        }

        [Test]
        public async Task _05Test_PostAorder_ReturnsBadRequest_WhenEmptyOrderAdded()
        {
            //Arrange 

            //Act            
            var result = await _controllerUnderTest.PostOrders(new OrdersVM());
            var badResult = (BadRequestObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "Cannot Add an empty order";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.AreEqual(10, _eCommerceContext.Orders.Count());

        }

        [Test]
        public async Task _06Test_PutOrders_ReturnsOkObjectResult()
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
            _controllerUnderTest = new OrdersController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var order1 = new Order()
            {
                OrderId = 12,
                EfUserId = 21,
                ShippingId = 1,
                OrderDate = new DateTime(2023, 11, 12, 12, 45, 00),
                TotalAmount = 2100.99
            };
            var ordersVM1 = new OrdersVM()
            {
                TotalAmount = order1.TotalAmount
            };

            var order2 = new Order()
            {
                OrderId = 13,
                EfUserId = 22,
                ShippingId = 12,
                OrderDate = new DateTime(2023, 11, 22, 12, 45, 00),
                TotalAmount = 2200.99
            };
            var ordersVM2 = new OrdersVM()
            {
                TotalAmount = order2.TotalAmount
            };

            var order3 = new Order()
            {
                OrderId = 14,
                EfUserId = 23,
                ShippingId = 14,
                OrderDate = new DateTime(2023, 12, 22, 12, 45, 00),
                TotalAmount = 2300.99
            };
            var ordersVM3 = new OrdersVM()
            {
                TotalAmount = order3.TotalAmount
            };


            //Act            
            var result1 = await _controllerUnderTest.PutOrders(1, ordersVM1);
            var result2 = await _controllerUnderTest.PutOrders(2, ordersVM2);
            var result3 = await _controllerUnderTest.PutOrders(3, ordersVM3);

            var okResult1 = (OkObjectResult)result1;
            var actual1 = okResult1.Value.ToString();
            var expected1 = "Order Updated";

            var okResult2 = (OkObjectResult)result2;
            var actual2 = okResult2.Value.ToString();
            var expected2 = "Order Updated";

            var okResult3 = (OkObjectResult)result3;
            var actual3 = okResult3.Value.ToString();
            var expected3 = "Order Updated";

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
        public async Task _07Test_PutOrders_ReturnsNotUpdated_WhenSeller()
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
            _controllerUnderTest = new OrdersController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var order1 = new Order()
            {
                OrderId = 12,
                EfUserId = 21,
                ShippingId = 1,
                OrderDate = new DateTime(2023, 11, 12, 12, 45, 00),
                TotalAmount = 2100.99
            };
            var ordersVM1 = new OrdersVM()
            {
                TotalAmount = order1.TotalAmount
            };

            var order2 = new Order()
            {
                OrderId = 13,
                EfUserId = 22,
                ShippingId = 12,
                OrderDate = new DateTime(2023, 11, 22, 12, 45, 00),
                TotalAmount = 2200.99
            };
            var ordersVM2 = new OrdersVM()
            {
                TotalAmount = order2.TotalAmount
            };

            var order3 = new Order()
            {
                OrderId = 14,
                EfUserId = 23,
                ShippingId = 14,
                OrderDate = new DateTime(2023, 12, 22, 12, 45, 00),
                TotalAmount = 2300.99
            };
            var ordersVM3 = new OrdersVM()
            {
                TotalAmount = order3.TotalAmount
            };


            //Act            
            var result1 = await _controllerUnderTest.PutOrders(1, ordersVM1);
            var result2 = await _controllerUnderTest.PutOrders(2, ordersVM2);
            var result3 = await _controllerUnderTest.PutOrders(3, ordersVM3);

            var badResult1 = (BadRequestObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "Not authorised to update orders";

            var badResult2 = (BadRequestObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "Not authorised to update orders";

            var badResult3 = (BadRequestObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "Not authorised to update orders";

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
        public async Task _08Test_PutOrders_ReturnsNotFoundResult()
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
            _controllerUnderTest = new OrdersController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };
            var order1 = new Order()
            {
                OrderId = 12,
                EfUserId = 21,
                ShippingId = 1,
                OrderDate = new DateTime(2023, 11, 12, 12, 45, 00),
                TotalAmount = 2100.99
            };
            var ordersVM1 = new OrdersVM()
            {
                TotalAmount = order1.TotalAmount
            };

            var order2 = new Order()
            {
                OrderId = 13,
                EfUserId = 22,
                ShippingId = 12,
                OrderDate = new DateTime(2023, 11, 22, 12, 45, 00),
                TotalAmount = 2200.99
            };
            var ordersVM2 = new OrdersVM()
            {
                TotalAmount = order2.TotalAmount
            };

            var order3 = new Order()
            {
                OrderId = 14,
                EfUserId = 23,
                ShippingId = 14,
                OrderDate = new DateTime(2023, 12, 22, 12, 45, 00),
                TotalAmount = 2300.99
            };
            var ordersVM3 = new OrdersVM()
            {
                TotalAmount = order3.TotalAmount
            };


            //Act            
            var result1 = await _controllerUnderTest.PutOrders(11, ordersVM1);
            var result2 = await _controllerUnderTest.PutOrders(12, ordersVM2);
            var result3 = await _controllerUnderTest.PutOrders(13, ordersVM3);

            var badResult1 = (NotFoundObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "No Order with that ID exists, please try again";

            var badResult2 = (NotFoundObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "No Order with that ID exists, please try again";

            var badResult3 = (NotFoundObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "No Order with that ID exists, please try again";

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
        public async Task _09Test_PostOrder_ReturnsActionResultObjectWith13Orders_WhenAdded3Orders()
        {
            //Arrange
            var order1 = new Order()
            {
                OrderId = 12,
                EfUserId = 21,
                ShippingId = 1,
                OrderDate = new DateTime(2023, 11, 12, 12, 45, 00),
                TotalAmount = 2100.99
            };
            var ordersVM1 = new OrdersVM()
            {
                TotalAmount = order1.TotalAmount
            };

            var order2 = new Order()
            {
                OrderId = 13,
                EfUserId = 22,
                ShippingId = 12,
                OrderDate = new DateTime(2023, 11, 22, 12, 45, 00),
                TotalAmount = 2200.99
            };
            var ordersVM2 = new OrdersVM()
            {
                TotalAmount = order2.TotalAmount
            };

            var order3 = new Order()
            {
                OrderId = 14,
                EfUserId = 23,
                ShippingId = 14,
                OrderDate = new DateTime(2023, 12, 22, 12, 45, 00),
                TotalAmount = 2300.99
            };
            var ordersVM3 = new OrdersVM()
            {
                TotalAmount = order3.TotalAmount
            };

            //Act            
            var result1 = await _controllerUnderTest.PostOrders(_orderVM);
            var result2 = await _controllerUnderTest.PostOrders(ordersVM2);
            var result3 = await _controllerUnderTest.PostOrders(ordersVM3);

            //Assert
            Assert.NotNull(_eCommerceContext.Orders);
            Assert.AreEqual(13, _eCommerceContext.Orders.Count());

        }


        [TestCase(11)]
        [TestCase(12)]
        [TestCase(10)]
        [TestCase(9)]
        [TestCase(8)]
        [TestCase(7)]
        public async Task _10Test_GetOrderByID_ReturnsWithCorrectType_WhenPassedInID(int id)
        {
            //Arrange 
            var order1 = new Order()
            {
                OrderId = 12,
                EfUserId = 21,
                ShippingId = 1,
                OrderDate = new DateTime(2023, 11, 12, 12, 45, 00),
                TotalAmount = 2100.99
            };
            var ordersVM1 = new OrdersVM()
            {
                TotalAmount = order1.TotalAmount
            };

            var order2 = new Order()
            {
                OrderId = 13,
                EfUserId = 22,
                ShippingId = 12,
                OrderDate = new DateTime(2023, 11, 22, 12, 45, 00),
                TotalAmount = 2200.99
            };
            var ordersVM2 = new OrdersVM()
            {
                TotalAmount = order2.TotalAmount
            };

            var order3 = new Order()
            {
                OrderId = 14,
                EfUserId = 23,
                ShippingId = 14,
                OrderDate = new DateTime(2023, 12, 22, 12, 45, 00),
                TotalAmount = 2300.99
            };
            var ordersVM3 = new OrdersVM()
            {
                TotalAmount = order3.TotalAmount
            };

            await _controllerUnderTest.PostOrders(ordersVM1);
            await _controllerUnderTest.PostOrders(ordersVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetOrders(id);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Order>>(actionResult);
        }



        [Test]
        public async Task _11Test_GetAllorder_ReturnsWithCorrectTypeAndCount()
        {
            //Arrange 
            var order1 = new Order()
            {
                OrderId = 12,
                EfUserId = 21,
                ShippingId = 1,
                OrderDate = new DateTime(2023, 11, 12, 12, 45, 00),
                TotalAmount = 2100.99
            };
            var ordersVM1 = new OrdersVM()
            {
                TotalAmount = order1.TotalAmount
            };

            var order2 = new Order()
            {
                OrderId = 13,
                EfUserId = 22,
                ShippingId = 12,
                OrderDate = new DateTime(2023, 11, 22, 12, 45, 00),
                TotalAmount = 2200.99
            };
            var ordersVM2 = new OrdersVM()
            {
                TotalAmount = order2.TotalAmount
            };

            var order3 = new Order()
            {
                OrderId = 14,
                EfUserId = 23,
                ShippingId = 14,
                OrderDate = new DateTime(2023, 12, 22, 12, 45, 00),
                TotalAmount = 2300.99
            };
            var ordersVM3 = new OrdersVM()
            {
                TotalAmount = order3.TotalAmount
            };

            await _controllerUnderTest.PostOrders(ordersVM1);
            await _controllerUnderTest.PostOrders(ordersVM2);
            await _controllerUnderTest.PostOrders(ordersVM3);

            //Act            
            var actionResult = await _controllerUnderTest.GetOrders();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<IEnumerable<Order>>>(actionResult);
            var result = (OkObjectResult)actionResult.Result;
            var value = (List<Order>)result.Value;
            Assert.AreEqual(_eCommerceContext.Orders.Count(), value.Count);
        }

        [Test]
        public async Task _12Test_GetorderById_ReturnsWithCorrectType()
        {
            //Arrange 
            var order1 = new Order()
            {
                OrderId = 12,
                EfUserId = 21,
                ShippingId = 1,
                OrderDate = new DateTime(2023, 11, 12, 12, 45, 00),
                TotalAmount = 2100.99
            };
            var ordersVM1 = new OrdersVM()
            {
                TotalAmount = order1.TotalAmount
            };

            var order2 = new Order()
            {
                OrderId = 13,
                EfUserId = 22,
                ShippingId = 12,
                OrderDate = new DateTime(2023, 11, 22, 12, 45, 00),
                TotalAmount = 2200.99
            };
            var ordersVM2 = new OrdersVM()
            {
                TotalAmount = order2.TotalAmount
            };

            var order3 = new Order()
            {
                OrderId = 14,
                EfUserId = 23,
                ShippingId = 14,
                OrderDate = new DateTime(2023, 12, 22, 12, 45, 00),
                TotalAmount = 2300.99
            };
            var ordersVM3 = new OrdersVM()
            {
                TotalAmount = order3.TotalAmount
            };


            await _controllerUnderTest.PostOrders(_orderVM);
            await _controllerUnderTest.PostOrders(ordersVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetOrders(11);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Order>>(actionResult);
        }


        [Test]
        public async Task _13Test_PostOrders_NotAddedAndShowsInContextCount_WhenUserIsASeller()
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
            _controllerUnderTest = new OrdersController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            //Act            
            var actionResult = await _controllerUnderTest.PostOrders(_orderVM);
            //Assert
            Assert.IsFalse(_eCommerceContext.Orders.Where(c => c.OrderId == 11).Count() > 0);
            Assert.IsTrue(_eCommerceContext.Orders.Count() == 10);

        }


        [Test]
        public async Task _14Test_PostOrders_ReturnsBadObjectResult_WhenUserIsACustomer()
        {
            //Arrange

            //Act            
            var actionResult = await _controllerUnderTest.PostOrders(_orderVM);

            //Assert
            var result = (ActionResult<Order>)actionResult.Result;
            var goodResult = (OkObjectResult)actionResult.Result;
            var expected = "Not authorised to add orders as a customer";
            var actual = goodResult.Value.ToString();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Order>>(actionResult);
            Assert.AreEqual(goodResult.StatusCode, 200);
            Assert.IsFalse(actual.Contains(expected));
            Assert.AreEqual(11, _eCommerceContext.Orders.Count());
        }


        [Test]
        public async Task _15Test_DeleteOrders_ReturnsMessageThatSellerCannotDeleteOrder()
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
            _controllerUnderTest = new OrdersController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var order1 = new Order()
            {
                OrderId = 12,
                EfUserId = 21,
                ShippingId = 1,
                OrderDate = new DateTime(2023, 11, 12, 12, 45, 00),
                TotalAmount = 2100.99
            };
            var ordersVM1 = new OrdersVM()
            {
                TotalAmount = order1.TotalAmount
            };

            var order2 = new Order()
            {
                OrderId = 13,
                EfUserId = 22,
                ShippingId = 12,
                OrderDate = new DateTime(2023, 11, 22, 12, 45, 00),
                TotalAmount = 2200.99
            };
            var ordersVM2 = new OrdersVM()
            {
                TotalAmount = order2.TotalAmount
            };

            var order3 = new Order()
            {
                OrderId = 14,
                EfUserId = 23,
                ShippingId = 14,
                OrderDate = new DateTime(2023, 12, 22, 12, 45, 00),
                TotalAmount = 2300.99
            };
            var ordersVM3 = new OrdersVM()
            {
                TotalAmount = order3.TotalAmount
            };

            //Act

            var actionResultDeleted = await _controllerUnderTest.DeleteOrders(8);
            var result = (ActionResult<Order>)actionResultDeleted.Result;
            var badResult = (BadRequestObjectResult)actionResultDeleted.Result;
            var expected = "Not authorised to delete orders";
            var actual = badResult.Value.ToString();

            //Assert
            Assert.IsInstanceOf<ActionResult<Order>>(actionResultDeleted);
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(10, _eCommerceContext.Orders.Count());
        }



        [Test]
        public async Task _16Test_DeleteOrders_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount_WhenAdministrator()
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
            _controllerUnderTest = new OrdersController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var order2 = new Order()
            {
                OrderId = 11,
                EfUserId = 21,
                ShippingId = 1,
                OrderDate = new DateTime(2023, 11, 12, 12, 45, 00),
                TotalAmount = 2100.99
            };
            var ordersVM2 = new OrdersVM()
            {
                TotalAmount = order2.TotalAmount
            };

            //Act
            var actionResult = await _controllerUnderTest.PostOrders(ordersVM2);
            var actionResultDeleted = await _controllerUnderTest.DeleteOrders(order2.OrderId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Order>>(actionResultDeleted);
            Assert.AreEqual(10, _eCommerceContext.Orders.Count());
        }


        [Test]
        public async Task _17Test_DeleteOrders_AddMultipleDeleteOne_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount_WhenAdministrator()
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
            _controllerUnderTest = new OrdersController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var order2 = new Order()
            {
                OrderId = 11,
                EfUserId = 21,
                ShippingId = 1,
                OrderDate = new DateTime(2023, 11, 12, 12, 45, 00),
                TotalAmount = 2100.99
            };
            var ordersVM2 = new OrdersVM()
            {
                TotalAmount = order2.TotalAmount
            };

            //Act
            await _controllerUnderTest.PostOrders(_orderVM);
            await _controllerUnderTest.PostOrders(ordersVM2);

            var actionResultDeleted = await _controllerUnderTest.DeleteOrders(_order.OrderId);
            var actionResultDeleted2 = await _controllerUnderTest.DeleteOrders(order2.OrderId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Order>>(actionResultDeleted);
            Assert.NotNull(actionResultDeleted2);
            Assert.IsInstanceOf<ActionResult<Order>>(actionResultDeleted2);
            Assert.AreEqual(10, _eCommerceContext.Orders.Count());
        }

        [TestCase("2023-11-12")]
        [TestCase("2017-04-22")]
        [TestCase("2019-11-12")]
        [TestCase("2018-05-12")]
        [TestCase("2018-03-21")]
        [TestCase("2019-10-08")]
        [TestCase("2020-02-13")]
        [TestCase("2021-12-12")]
        [TestCase("2022-12-10")]
        [TestCase("2020-03-16")]
        public async Task _18Test_GetOrderByDate_ReturnsaListwithCount1_WhenCorrectOrderDateEntered(DateTime datePassedIn)
        {
            //Arrange
            DateTime date = datePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetOrderByDate(date);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Order>)okResult.Value;
            var expected = _eCommerceContext.Orders.FirstOrDefault(c => c.OrderDate == date);

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Order>>>(result);
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
        public async Task _19Test_GetOrderByDate_ReturnsaListwithCount0_WhenWrongOrderDateEntered(DateTime datePassedIn)
        {
            //Arrange
            DateTime date = datePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetOrderByDate(date);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = "No Order with that date exists, please try again";
            var expected = "No Order with that date exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }


        [TestCase("2017-01-01", "2017-12-12")]
        [TestCase("2018-01-01", "2018-12-12")]
        [TestCase("2019-01-01", "2019-12-12")]
        [TestCase("2020-01-01", "2022-12-12")]
        [TestCase("2021-01-01", "2023-12-12")]
        public async Task _20Test_GetOrderByDate_ReturnsaListwithCount1_WhenCorrectOrderDateEntered(DateTime date1PassedIn, DateTime date2PassedIn)
        {
            //Arrange
            DateTime date1 = date1PassedIn;
            DateTime date2 = date2PassedIn;

            //Act            
            var result = await _controllerUnderTest.GetOrderByBetweenDates(date1, date2);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Order>)okResult.Value;

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Order>>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.IsTrue(actual.Count() > 0);

        }

        [TestCase("2012-01-01", "2012-12-12")]
        [TestCase("2013-01-01", "2013-12-12")]
        [TestCase("2014-01-01", "2014-12-12")]
        [TestCase("2015-01-01", "2015-12-12")]
        [TestCase("2016-01-01", "2016-12-12")]
        public async Task _21Test_GetOrderByDate_ReturnsaListwithCount0_WhenWrongOrderDateEntered(DateTime date1PassedIn, DateTime date2PassedIn)
        {
            //Arrange
            DateTime date1 = date1PassedIn;
            DateTime date2 = date2PassedIn;

            //Act            
            var result = await _controllerUnderTest.GetOrderByBetweenDates(date1, date2);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = "No Order with that date range exists, please try again";
            var expected = "No Order with that date range exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }
    }
}

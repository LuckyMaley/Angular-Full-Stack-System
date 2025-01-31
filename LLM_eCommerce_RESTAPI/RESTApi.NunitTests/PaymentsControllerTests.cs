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
    public class PaymentsControllerTests
    {
        private LLM_eCommerce_EFDBContext _eCommerceContext;
        private PaymentsController _controllerUnderTest;
        private List<Payment> _paymentList;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AuthenticationContext _authenticationContext;
        PaymentsVM _paymentVM;
        Payment _payment;
        IdentityUser identityUser;
        ClaimsPrincipal principal;

        [SetUp]
        public void Initialiser()
        {
            _eCommerceContext = (LLM_eCommerce_EFDBContext)InMemoryContext.GeneratedDB();
            var prod = _eCommerceContext.Payments.Count();
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
            _controllerUnderTest = new PaymentsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            _paymentList = new List<Payment>();
            _payment = new Payment()
            {
                PaymentId = 11,
                OrderId = 1,
                PaymentDate = new DateTime(2023, 11, 12, 12, 55, 00),
                PaymentMethod = "Card",
                Amount = 2100.99,
                Status = "Paid"
            };
            _paymentVM = new PaymentsVM()
            {
                OrderId = _payment.OrderId,
                PaymentMethod = _payment.PaymentMethod,
                Amount = _payment.Amount,
                Status = _payment.Status,
            };

        }



        [TearDown]
        public void CleanUpObject()
        {
            _eCommerceContext.Database.EnsureDeleted();
            _controllerUnderTest = null;
            _paymentList = null;
            _payment = null;
            _userManager = null;

            _roleManager = null;
            _authenticationContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task _01Test_GetAllPayment_ReturnsListWithValidCount0()
        {
            // Arrange


            // Act
            var result = await _controllerUnderTest.GetPayments();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var paymentList = okResult.Value as List<Payment>;
            Assert.NotNull(paymentList);
            Assert.AreEqual(10, paymentList.Count);
        }

        [Test]
        public async Task _02Test_GetAllPayment_ReturnsListWithValidCountEqualTo11()
        {
            // Arrange
            _eCommerceContext.Payments.Add(_payment);
            await _eCommerceContext.SaveChangesAsync();


            _controllerUnderTest = new PaymentsController(_eCommerceContext, _userManager, _roleManager, (AuthenticationContext)_authenticationContext);


            // Act
            var result = await _controllerUnderTest.GetPayments();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var paymentList = okResult.Value as List<Payment>;
            Assert.NotNull(paymentList);
            Assert.AreEqual(11, paymentList.Count);
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
        public async Task _03Test_GetPaymentByID_ReturnsaListwithCount1_WhenCorrectPaymentIDEntered(int idPassedIn)
        {
            //Arrange
            int id = idPassedIn;

            //Act            
            var result = await _controllerUnderTest.GetPayments(id);
            var okResult = (OkObjectResult)result.Result;
            var actual = (Payment)okResult.Value;
            var expected = _eCommerceContext.Payments.FirstOrDefault(c => c.PaymentId == id);

            //Assert
            Assert.IsInstanceOf<ActionResult<Payment>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.AreEqual(actual.PaymentId, expected.PaymentId);

        }


        [TestCase("Card")]
        public async Task _04Test_GetPaymentByName_ReturnsaListwithCount1_WhenCorrectPaymentIDEntered(string namePassedIn)
        {
            //Arrange
            string name = namePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetPaymentByMethod(name);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Payment>)okResult.Value;
            var expected = _eCommerceContext.Payments.FirstOrDefault(c => c.PaymentMethod == name);

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Payment>>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.IsTrue(actual.Count() > 0);

        }

        [TestCase("EFT")]
        [TestCase("Coupon")]
        [TestCase("LayBy")]
        [TestCase("Credit")]
        [TestCase("Cheque")]
        public async Task _05Test_GetPaymentByName_ReturnsNotFound_WhenIncorrectPaymentNameEntered(string namePassedIn)
        {
            //Arrange
            string name = namePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetPaymentByMethod(name);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = notFoundResult.Value.ToString();
            var expected = "No Payment with that method exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }

        [Test]
        public async Task _06Test_GetPaymentByID_ReturnsaBackActionResult_WhenPaymentIDEnteredDoesNotExist()
        {
            //Arrange
            int id = 20;

            //Act            
            var result = await _controllerUnderTest.GetPayments(id);
            var badResult = (NotFoundObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "No Payment with that ID exists, please try again";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 404);

        }

        [Test]
        public async Task _07Test_PostApayment_ReturnsBadRequest_WhenEmptyPaymentAdded()
        {
            //Arrange 

            //Act            
            var result = await _controllerUnderTest.PostPayments(new PaymentsVM());
            var badResult = (BadRequestObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "Cannot Add an empty payment";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.AreEqual(10, _eCommerceContext.Payments.Count());

        }

        [Test]
        public async Task _08Test_PutPayments_ReturnsOkObjectResult()
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
            _controllerUnderTest = new PaymentsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var payment1 = new Payment()
            {
                PaymentId = 12,
                OrderId = 1,
                PaymentDate = new DateTime(2023, 11, 12, 12, 55, 00),
                PaymentMethod = "Cash",
                Amount = 2100.99,
                Status = "Paid"
            };
            var paymentsVM1 = new PaymentsVM()
            {
                OrderId = payment1.OrderId,
                PaymentMethod = payment1.PaymentMethod,
                Amount = payment1.Amount,
                Status = payment1.Status,
            };

            var payment2 = new Payment()
            {
                PaymentId = 13,
                OrderId = 6,
                PaymentDate = new DateTime(2024, 11, 11, 12, 55, 00),
                PaymentMethod = "Card",
                Amount = 2400.99,
                Status = "Paid"
            };
            var paymentsVM2 = new PaymentsVM()
            {
                OrderId = payment2.OrderId,
                PaymentMethod = payment2.PaymentMethod,
                Amount = payment2.Amount,
                Status = payment2.Status,
            };

            var payment3 = new Payment()
            {
                PaymentId = 14,
                OrderId = 3,
                PaymentDate = new DateTime(2022, 11, 11, 12, 00, 00),
                PaymentMethod = "Card",
                Amount = 3400.99,
                Status = "Paid"
            };
            var paymentsVM3 = new PaymentsVM()
            {
                OrderId = payment3.OrderId,
                PaymentMethod = payment3.PaymentMethod,
                Amount = payment3.Amount,
                Status = payment3.Status,
            };


            //Act            
            var result1 = await _controllerUnderTest.PutPayments(1, paymentsVM1);
            var result2 = await _controllerUnderTest.PutPayments(2, paymentsVM2);
            var result3 = await _controllerUnderTest.PutPayments(3, paymentsVM3);

            var okResult1 = (OkObjectResult)result1;
            var actual1 = okResult1.Value.ToString();
            var expected1 = "Payment Updated";

            var okResult2 = (OkObjectResult)result2;
            var actual2 = okResult2.Value.ToString();
            var expected2 = "Payment Updated";

            var okResult3 = (OkObjectResult)result3;
            var actual3 = okResult3.Value.ToString();
            var expected3 = "Payment Updated";

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
        public async Task _09Test_PutPayments_ReturnsNotUpdated_WhenSeller()
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
            _controllerUnderTest = new PaymentsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var payment1 = new Payment()
            {
                PaymentId = 12,
                OrderId = 1,
                PaymentDate = new DateTime(2023, 11, 12, 12, 55, 00),
                PaymentMethod = "Cash",
                Amount = 2100.99,
                Status = "Paid"
            };
            var paymentsVM1 = new PaymentsVM()
            {
                OrderId = payment1.OrderId,
                PaymentMethod = payment1.PaymentMethod,
                Amount = payment1.Amount,
                Status = payment1.Status,
            };

            var payment2 = new Payment()
            {
                PaymentId = 13,
                OrderId = 6,
                PaymentDate = new DateTime(2024, 11, 11, 12, 55, 00),
                PaymentMethod = "Card",
                Amount = 2400.99,
                Status = "Paid"
            };
            var paymentsVM2 = new PaymentsVM()
            {
                OrderId = payment2.OrderId,
                PaymentMethod = payment2.PaymentMethod,
                Amount = payment2.Amount,
                Status = payment2.Status,
            };

            var payment3 = new Payment()
            {
                PaymentId = 14,
                OrderId = 3,
                PaymentDate = new DateTime(2022, 11, 11, 12, 00, 00),
                PaymentMethod = "Card",
                Amount = 3400.99,
                Status = "Paid"
            };
            var paymentsVM3 = new PaymentsVM()
            {
                OrderId = payment3.OrderId,
                PaymentMethod = payment3.PaymentMethod,
                Amount = payment3.Amount,
                Status = payment3.Status,
            };




            //Act            
            var result1 = await _controllerUnderTest.PutPayments(1, paymentsVM1);
            var result2 = await _controllerUnderTest.PutPayments(2, paymentsVM2);
            var result3 = await _controllerUnderTest.PutPayments(3, paymentsVM3);

            var badResult1 = (BadRequestObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "Not authorised to update payments";

            var badResult2 = (BadRequestObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "Not authorised to update payments";

            var badResult3 = (BadRequestObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "Not authorised to update payments";

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
        public async Task _10Test_PutPayments_ReturnsNotFoundResult()
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
            _controllerUnderTest = new PaymentsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };
            var payment1 = new Payment()
            {
                PaymentId = 12,
                OrderId = 1,
                PaymentDate = new DateTime(2023, 11, 12, 12, 55, 00),
                PaymentMethod = "Cash",
                Amount = 2100.99,
                Status = "Paid"
            };
            var paymentsVM1 = new PaymentsVM()
            {
                OrderId = payment1.OrderId,
                PaymentMethod = payment1.PaymentMethod,
                Amount = payment1.Amount,
                Status = payment1.Status,
            };

            var payment2 = new Payment()
            {
                PaymentId = 13,
                OrderId = 6,
                PaymentDate = new DateTime(2024, 11, 11, 12, 55, 00),
                PaymentMethod = "Card",
                Amount = 2400.99,
                Status = "Paid"
            };
            var paymentsVM2 = new PaymentsVM()
            {
                OrderId = payment2.OrderId,
                PaymentMethod = payment2.PaymentMethod,
                Amount = payment2.Amount,
                Status = payment2.Status,
            };

            var payment3 = new Payment()
            {
                PaymentId = 14,
                OrderId = 3,
                PaymentDate = new DateTime(2022, 11, 11, 12, 00, 00),
                PaymentMethod = "Card",
                Amount = 3400.99,
                Status = "Paid"
            };
            var paymentsVM3 = new PaymentsVM()
            {
                OrderId = payment3.OrderId,
                PaymentMethod = payment3.PaymentMethod,
                Amount = payment3.Amount,
                Status = payment3.Status,
            };


            //Act            
            var result1 = await _controllerUnderTest.PutPayments(11, paymentsVM1);
            var result2 = await _controllerUnderTest.PutPayments(12, paymentsVM2);
            var result3 = await _controllerUnderTest.PutPayments(13, paymentsVM3);

            var badResult1 = (NotFoundObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "No Payment with that ID exists, please try again";

            var badResult2 = (NotFoundObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "No Payment with that ID exists, please try again";

            var badResult3 = (NotFoundObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "No Payment with that ID exists, please try again";

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
        public async Task _11Test_PostPayment_ReturnsActionResultObjectWith13Payments_WhenAdded3Payments()
        {
            //Arrange
            var payment1 = new Payment()
            {
                PaymentId = 12,
                OrderId = 1,
                PaymentDate = new DateTime(2023, 11, 12, 12, 55, 00),
                PaymentMethod = "Cash",
                Amount = 2100.99,
                Status = "Paid"
            };
            var paymentsVM1 = new PaymentsVM()
            {
                OrderId = payment1.OrderId,
                PaymentMethod = payment1.PaymentMethod,
                Amount = payment1.Amount,
                Status = payment1.Status,
            };

            var payment2 = new Payment()
            {
                PaymentId = 13,
                OrderId = 6,
                PaymentDate = new DateTime(2024, 11, 11, 12, 55, 00),
                PaymentMethod = "Card",
                Amount = 2400.99,
                Status = "Paid"
            };
            var paymentsVM2 = new PaymentsVM()
            {
                OrderId = payment2.OrderId,
                PaymentMethod = payment2.PaymentMethod,
                Amount = payment2.Amount,
                Status = payment2.Status,
            };

            var payment3 = new Payment()
            {
                PaymentId = 14,
                OrderId = 3,
                PaymentDate = new DateTime(2022, 11, 11, 12, 00, 00),
                PaymentMethod = "Card",
                Amount = 3400.99,
                Status = "Paid"
            };
            var paymentsVM3 = new PaymentsVM()
            {
                OrderId = payment3.OrderId,
                PaymentMethod = payment3.PaymentMethod,
                Amount = payment3.Amount,
                Status = payment3.Status,
            };

            //Act            
            var result1 = await _controllerUnderTest.PostPayments(_paymentVM);
            var result2 = await _controllerUnderTest.PostPayments(paymentsVM2);
            var result3 = await _controllerUnderTest.PostPayments(paymentsVM3);

            //Assert
            Assert.NotNull(_eCommerceContext.Payments);
            Assert.AreEqual(13, _eCommerceContext.Payments.Count());

        }


        [TestCase(11)]
        [TestCase(12)]
        [TestCase(10)]
        [TestCase(9)]
        [TestCase(8)]
        [TestCase(7)]
        public async Task _12Test_GetPaymentByID_ReturnsWithCorrectType_WhenPassedInID(int id)
        {
            //Arrange 
            var payment1 = new Payment()
            {
                PaymentId = 12,
                OrderId = 1,
                PaymentDate = new DateTime(2023, 11, 12, 12, 55, 00),
                PaymentMethod = "Cash",
                Amount = 2100.99,
                Status = "Paid"
            };
            var paymentsVM1 = new PaymentsVM()
            {
                OrderId = payment1.OrderId,
                PaymentMethod = payment1.PaymentMethod,
                Amount = payment1.Amount,
                Status = payment1.Status,
            };

            var payment2 = new Payment()
            {
                PaymentId = 13,
                OrderId = 6,
                PaymentDate = new DateTime(2024, 11, 11, 12, 55, 00),
                PaymentMethod = "Card",
                Amount = 2400.99,
                Status = "Paid"
            };
            var paymentsVM2 = new PaymentsVM()
            {
                OrderId = payment2.OrderId,
                PaymentMethod = payment2.PaymentMethod,
                Amount = payment2.Amount,
                Status = payment2.Status,
            };

            var payment3 = new Payment()
            {
                PaymentId = 14,
                OrderId = 3,
                PaymentDate = new DateTime(2022, 11, 11, 12, 00, 00),
                PaymentMethod = "Card",
                Amount = 3400.99,
                Status = "Paid"
            };
            var paymentsVM3 = new PaymentsVM()
            {
                OrderId = payment3.OrderId,
                PaymentMethod = payment3.PaymentMethod,
                Amount = payment3.Amount,
                Status = payment3.Status,
            };

            await _controllerUnderTest.PostPayments(paymentsVM1);
            await _controllerUnderTest.PostPayments(paymentsVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetPayments(id);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Payment>>(actionResult);
        }



        [Test]
        public async Task _12Test_GetAllpayment_ReturnsWithCorrectTypeAndCount()
        {
            //Arrange 
            var payment1 = new Payment()
            {
                PaymentId = 12,
                OrderId = 1,
                PaymentDate = new DateTime(2023, 11, 12, 12, 55, 00),
                PaymentMethod = "Cash",
                Amount = 2100.99,
                Status = "Paid"
            };
            var paymentsVM1 = new PaymentsVM()
            {
                OrderId = payment1.OrderId,
                PaymentMethod = payment1.PaymentMethod,
                Amount = payment1.Amount,
                Status = payment1.Status,
            };

            var payment2 = new Payment()
            {
                PaymentId = 13,
                OrderId = 6,
                PaymentDate = new DateTime(2024, 11, 11, 12, 55, 00),
                PaymentMethod = "Card",
                Amount = 2400.99,
                Status = "Paid"
            };
            var paymentsVM2 = new PaymentsVM()
            {
                OrderId = payment2.OrderId,
                PaymentMethod = payment2.PaymentMethod,
                Amount = payment2.Amount,
                Status = payment2.Status,
            };

            var payment3 = new Payment()
            {
                PaymentId = 14,
                OrderId = 3,
                PaymentDate = new DateTime(2022, 11, 11, 12, 00, 00),
                PaymentMethod = "Card",
                Amount = 3400.99,
                Status = "Paid"
            };
            var paymentsVM3 = new PaymentsVM()
            {
                OrderId = payment3.OrderId,
                PaymentMethod = payment3.PaymentMethod,
                Amount = payment3.Amount,
                Status = payment3.Status,
            };

            await _controllerUnderTest.PostPayments(paymentsVM1);
            await _controllerUnderTest.PostPayments(paymentsVM2);
            await _controllerUnderTest.PostPayments(paymentsVM3);

            //Act            
            var actionResult = await _controllerUnderTest.GetPayments();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<IEnumerable<Payment>>>(actionResult);
            var result = (OkObjectResult)actionResult.Result;
            var value = (List<Payment>)result.Value;
            Assert.AreEqual(_eCommerceContext.Payments.Count(), value.Count);
        }

        [Test]
        public async Task _13Test_GetpaymentById_ReturnsWithCorrectType()
        {
            //Arrange 
            var payment1 = new Payment()
            {
                PaymentId = 12,
                OrderId = 1,
                PaymentDate = new DateTime(2023, 11, 12, 12, 55, 00),
                PaymentMethod = "Cash",
                Amount = 2100.99,
                Status = "Paid"
            };
            var paymentsVM1 = new PaymentsVM()
            {
                OrderId = payment1.OrderId,
                PaymentMethod = payment1.PaymentMethod,
                Amount = payment1.Amount,
                Status = payment1.Status,
            };

            var payment2 = new Payment()
            {
                PaymentId = 13,
                OrderId = 6,
                PaymentDate = new DateTime(2024, 11, 11, 12, 55, 00),
                PaymentMethod = "Card",
                Amount = 2400.99,
                Status = "Paid"
            };
            var paymentsVM2 = new PaymentsVM()
            {
                OrderId = payment2.OrderId,
                PaymentMethod = payment2.PaymentMethod,
                Amount = payment2.Amount,
                Status = payment2.Status,
            };

            var payment3 = new Payment()
            {
                PaymentId = 14,
                OrderId = 3,
                PaymentDate = new DateTime(2022, 11, 11, 12, 00, 00),
                PaymentMethod = "Card",
                Amount = 3400.99,
                Status = "Paid"
            };
            var paymentsVM3 = new PaymentsVM()
            {
                OrderId = payment3.OrderId,
                PaymentMethod = payment3.PaymentMethod,
                Amount = payment3.Amount,
                Status = payment3.Status,
            };


            await _controllerUnderTest.PostPayments(_paymentVM);
            await _controllerUnderTest.PostPayments(paymentsVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetPayments(11);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Payment>>(actionResult);
        }


        [Test]
        public async Task _14Test_PostPayments_NotAddedAndShowsInContextCount_WhenUserIsASeller()
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
            _controllerUnderTest = new PaymentsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            //Act            
            var actionResult = await _controllerUnderTest.PostPayments(_paymentVM);
            //Assert
            Assert.IsFalse(_eCommerceContext.Payments.Where(c => c.PaymentId == 11).Count() > 0);
            Assert.IsTrue(_eCommerceContext.Payments.Count() == 10);

        }


        [Test]
        public async Task _15Test_PostPayments_ReturnsBadObjectResult_WhenUserIsACustomer()
        {
            //Arrange

            //Act            
            var actionResult = await _controllerUnderTest.PostPayments(_paymentVM);

            //Assert
            var result = (ActionResult<Payment>)actionResult.Result;
            var goodResult = (OkObjectResult)actionResult.Result;
            var expected = "Not authorised to add payments as a customer";
            var actual = goodResult.Value.ToString();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Payment>>(actionResult);
            Assert.AreEqual(goodResult.StatusCode, 200);
            Assert.IsFalse(actual.Contains(expected));
            Assert.AreEqual(11, _eCommerceContext.Payments.Count());
        }


        [Test]
        public async Task _16Test_DeletePayments_ReturnsMessageThatSellerCannotDeletePayment()
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
            _controllerUnderTest = new PaymentsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var payment1 = new Payment()
            {
                PaymentId = 12,
                OrderId = 1,
                PaymentDate = new DateTime(2023, 11, 12, 12, 55, 00),
                PaymentMethod = "Cash",
                Amount = 2100.99,
                Status = "Paid"
            };
            var paymentsVM1 = new PaymentsVM()
            {
                OrderId = payment1.OrderId,
                PaymentMethod = payment1.PaymentMethod,
                Amount = payment1.Amount,
                Status = payment1.Status,
            };

            var payment2 = new Payment()
            {
                PaymentId = 13,
                OrderId = 6,
                PaymentDate = new DateTime(2024, 11, 11, 12, 55, 00),
                PaymentMethod = "Card",
                Amount = 2400.99,
                Status = "Paid"
            };
            var paymentsVM2 = new PaymentsVM()
            {
                OrderId = payment2.OrderId,
                PaymentMethod = payment2.PaymentMethod,
                Amount = payment2.Amount,
                Status = payment2.Status,
            };

            var payment3 = new Payment()
            {
                PaymentId = 14,
                OrderId = 3,
                PaymentDate = new DateTime(2022, 11, 11, 12, 00, 00),
                PaymentMethod = "Card",
                Amount = 3400.99,
                Status = "Paid"
            };
            var paymentsVM3 = new PaymentsVM()
            {
                OrderId = payment3.OrderId,
                PaymentMethod = payment3.PaymentMethod,
                Amount = payment3.Amount,
                Status = payment3.Status,
            };

            //Act

            var actionResultDeleted = await _controllerUnderTest.DeletePayments(8);
            var result = (ActionResult<Payment>)actionResultDeleted.Result;
            var badResult = (BadRequestObjectResult)actionResultDeleted.Result;
            var expected = "Not authorised to delete payments";
            var actual = badResult.Value.ToString();

            //Assert
            Assert.IsInstanceOf<ActionResult<Payment>>(actionResultDeleted);
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(10, _eCommerceContext.Payments.Count());
        }



        [Test]
        public async Task _17Test_DeletePayments_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount_WhenAdministrator()
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
            _controllerUnderTest = new PaymentsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var payment2 = new Payment()
            {
                PaymentId = 11,
                OrderId = 6,
                PaymentDate = new DateTime(2024, 11, 11, 12, 55, 00),
                PaymentMethod = "Card",
                Amount = 2400.99,
                Status = "Paid"
            };
            var paymentsVM2 = new PaymentsVM()
            {
                OrderId = payment2.OrderId,
                PaymentMethod = payment2.PaymentMethod,
                Amount = payment2.Amount,
                Status = payment2.Status,
            };

            //Act
            var actionResult = await _controllerUnderTest.PostPayments(paymentsVM2);
            var actionResultDeleted = await _controllerUnderTest.DeletePayments(payment2.PaymentId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Payment>>(actionResultDeleted);
            Assert.AreEqual(10, _eCommerceContext.Payments.Count());
        }


        [Test]
        public async Task _18Test_DeletePayments_AddMultipleDeleteOne_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount_WhenAdministrator()
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
            _controllerUnderTest = new PaymentsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var payment2 = new Payment()
            {
                PaymentId = 11,
                OrderId = 6,
                PaymentDate = new DateTime(2024, 11, 11, 12, 55, 00),
                PaymentMethod = "Card",
                Amount = 2400.99,
                Status = "Paid"
            };
            var paymentsVM2 = new PaymentsVM()
            {
                OrderId = payment2.OrderId,
                PaymentMethod = payment2.PaymentMethod,
                Amount = payment2.Amount,
                Status = payment2.Status,
            };

            //Act
            await _controllerUnderTest.PostPayments(_paymentVM);
            await _controllerUnderTest.PostPayments(paymentsVM2);

            var actionResultDeleted = await _controllerUnderTest.DeletePayments(_payment.PaymentId);
            var actionResultDeleted2 = await _controllerUnderTest.DeletePayments(payment2.PaymentId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Payment>>(actionResultDeleted);
            Assert.NotNull(actionResultDeleted2);
            Assert.IsInstanceOf<ActionResult<Payment>>(actionResultDeleted2);
            Assert.AreEqual(10, _eCommerceContext.Payments.Count());
        }

        [TestCase("2023-11-12")]
        [TestCase("2017-04-22")]
        [TestCase("2019-11-12")]
        [TestCase("2018-05-12")]
        [TestCase("2018-03-12")]
        [TestCase("2019-10-13")]
        [TestCase("2020-02-02")]
        [TestCase("2021-12-12")]
        [TestCase("2022-12-10")]
        [TestCase("2020-03-22")]
        public async Task _19Test_GetPaymentByDate_ReturnsaListwithCount1_WhenCorrectPaymentDateEntered(DateTime datePassedIn)
        {
            //Arrange
            DateTime date = datePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetPaymentByDate(date);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Payment>)okResult.Value;
            var expected = _eCommerceContext.Payments.FirstOrDefault(c => c.PaymentDate == date);

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Payment>>>(result);
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
        public async Task _20Test_GetPaymentByDate_ReturnsaListwithCount0_WhenWrongPaymentDateEntered(DateTime datePassedIn)
        {
            //Arrange
            DateTime date = datePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetPaymentByDate(date);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = "No Payment with that date exists, please try again";
            var expected = "No Payment with that date exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }

        
        [TestCase("2017-01-01", "2017-12-12")]
        [TestCase("2018-01-01", "2018-12-12")]
        [TestCase("2019-01-01", "2019-12-12")]
        [TestCase("2020-01-01", "2022-12-12")]
        [TestCase("2021-01-01", "2023-12-12")]
        public async Task _21Test_GetPaymentByDate_ReturnsaListwithCount1_WhenCorrectPaymentDateEntered(DateTime date1PassedIn, DateTime date2PassedIn)
        {
            //Arrange
            DateTime date1 = date1PassedIn;
            DateTime date2 = date2PassedIn;

            //Act            
            var result = await _controllerUnderTest.GetPaymentByBetweenDates(date1, date2);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Payment>)okResult.Value;

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Payment>>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.IsTrue(actual.Count() > 0);

        }

        [TestCase("2012-01-01", "2012-12-12")]
        [TestCase("2013-01-01", "2013-12-12")]
        [TestCase("2014-01-01", "2014-12-12")]
        [TestCase("2015-01-01", "2015-12-12")]
        [TestCase("2016-01-01", "2016-12-12")]
        public async Task _22Test_GetPaymentByDate_ReturnsaListwithCount0_WhenWrongPaymentDateEntered(DateTime date1PassedIn, DateTime date2PassedIn)
        {
            //Arrange
            DateTime date1 = date1PassedIn;
            DateTime date2 = date2PassedIn;

            //Act            
            var result = await _controllerUnderTest.GetPaymentByBetweenDates(date1, date2);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = "No Payment with that date range exists, please try again";
            var expected = "No Payment with that date range exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }
    }
}

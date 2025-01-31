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
    public class ReviewsControllerTests
    {
        private LLM_eCommerce_EFDBContext _eCommerceContext;
        private ReviewsController _controllerUnderTest;
        private List<Review> _reviewList;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AuthenticationContext _authenticationContext;
        ReviewsVM _reviewVM;
        Review _review;
        IdentityUser identityUser;
        ClaimsPrincipal principal;

        [SetUp]
        public void Initialiser()
        {
            _eCommerceContext = (LLM_eCommerce_EFDBContext)InMemoryContext.GeneratedDB();
            var prod = _eCommerceContext.Reviews.Count();
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
            _controllerUnderTest = new ReviewsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            _reviewList = new List<Review>();
            _review = new Review()
            {
                ReviewId = 11,
                ProductId = 1,
                EfUserId = 21,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 12, 12, 12, 55, 00)
            };
            _reviewVM = new ReviewsVM()
            {
                ProductId = _review.ProductId,
                Rating = _review.Rating,
                Title = _review.Title,
                Comment = _review.Comment
            };

        }



        [TearDown]
        public void CleanUpObject()
        {
            _eCommerceContext.Database.EnsureDeleted();
            _controllerUnderTest = null;
            _reviewList = null;
            _review = null;
            _userManager = null;

            _roleManager = null;
            _authenticationContext.Database.EnsureDeleted();
        }

        [Test]
        public async Task _01Test_GetAllReview_ReturnsListWithValidCount0()
        {
            // Arrange


            // Act
            var result = await _controllerUnderTest.GetReviews();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var reviewList = okResult.Value as List<Review>;
            Assert.NotNull(reviewList);
            Assert.AreEqual(10, reviewList.Count);
        }

        [Test]
        public async Task _02Test_GetAllReview_ReturnsListWithValidCountEqualTo11()
        {
            // Arrange
            _eCommerceContext.Reviews.Add(_review);
            await _eCommerceContext.SaveChangesAsync();


            _controllerUnderTest = new ReviewsController(_eCommerceContext, _userManager, _roleManager, (AuthenticationContext)_authenticationContext);


            // Act
            var result = await _controllerUnderTest.GetReviews();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            var reviewList = okResult.Value as List<Review>;
            Assert.NotNull(reviewList);
            Assert.AreEqual(11, reviewList.Count);
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
        public async Task _03Test_GetReviewByID_ReturnsaListwithCount1_WhenCorrectReviewIDEntered(int idPassedIn)
        {
            //Arrange
            int id = idPassedIn;

            //Act            
            var result = await _controllerUnderTest.GetReviews(id);
            var okResult = (OkObjectResult)result.Result;
            var actual = (Review)okResult.Value;
            var expected = _eCommerceContext.Reviews.FirstOrDefault(c => c.ReviewId == id);

            //Assert
            Assert.IsInstanceOf<ActionResult<Review>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.AreEqual(actual.ReviewId, expected.ReviewId);

        }


        [TestCase("Great Product")]
        public async Task _04Test_GetReviewByTitle_ReturnsaListwithCount1_WhenCorrectReviewIDEntered(string titlePassedIn)
        {
            //Arrange
            string title = titlePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetReviewByTitle(title);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Review>)okResult.Value;
            var expected = _eCommerceContext.Reviews.FirstOrDefault(c => c.Title == title);

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Review>>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.IsTrue(actual.Count() > 0);

        }

        [TestCase("bad")]
        [TestCase("interesting")]
        [TestCase("test")]
        [TestCase("Not Good")]
        [TestCase("Refund")]
        public async Task _05Test_GetReviewByName_ReturnsNotFound_WhenIncorrectReviewNameEntered(string titlePassedIn)
        {
            //Arrange
            string title = titlePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetReviewByTitle(title);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = notFoundResult.Value.ToString();
            var expected = "No Review with that title exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }

        [Test]
        public async Task _06Test_GetReviewByID_ReturnsaBackActionResult_WhenReviewIDEnteredDoesNotExist()
        {
            //Arrange
            int id = 20;

            //Act            
            var result = await _controllerUnderTest.GetReviews(id);
            var badResult = (NotFoundObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "No Review with that ID exists, please try again";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 404);

        }

        [Test]
        public async Task _07Test_PostAreview_ReturnsBadRequest_WhenEmptyReviewAdded()
        {
            //Arrange 

            //Act            
            var result = await _controllerUnderTest.PostReviews(new ReviewsVM());
            var badResult = (BadRequestObjectResult)result.Result;
            var actual = badResult.Value.ToString();
            var expected = "Cannot Add an empty review";

            //Assert
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.AreEqual(10, _eCommerceContext.Reviews.Count());

        }

        [Test]
        public async Task _08Test_PutReviews_ReturnsOkObjectResult()
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
            _controllerUnderTest = new ReviewsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var review1 = new Review()
            {
                ReviewId = 12,
                ProductId = 1,
                EfUserId = 21,
                Rating = 2,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 12, 22, 12, 55, 00)
            };
            var reviewsVM1 = new ReviewsVM()
            {
                ProductId = review1.ProductId,
                Rating = review1.Rating,
                Title = review1.Title,
                Comment = review1.Comment
            };

            var review2 = new Review()
            {
                ReviewId = 13,
                ProductId = 2,
                EfUserId = 22,
                Rating = 3,
                Title = "Good Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2022, 12, 22, 12, 55, 00)
            };
            var reviewsVM2 = new ReviewsVM()
            {
                ProductId = review2.ProductId,
                Rating = review2.Rating,
                Title = review2.Title,
                Comment = review2.Comment
            };

            var review3 = new Review()
            {
                ReviewId = 14,
                ProductId = 4,
                EfUserId = 24,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 02, 22, 12, 55, 00)
            };
            var reviewsVM3 = new ReviewsVM()
            {
                ProductId = review3.ProductId,
                Rating = review3.Rating,
                Title = review3.Title,
                Comment = review3.Comment
            };


            //Act            
            var result1 = await _controllerUnderTest.PutReviews(1, reviewsVM1);
            var result2 = await _controllerUnderTest.PutReviews(2, reviewsVM2);
            var result3 = await _controllerUnderTest.PutReviews(3, reviewsVM3);

            var okResult1 = (OkObjectResult)result1;
            var actual1 = okResult1.Value.ToString();
            var expected1 = "Review Updated";

            var okResult2 = (OkObjectResult)result2;
            var actual2 = okResult2.Value.ToString();
            var expected2 = "Review Updated";

            var okResult3 = (OkObjectResult)result3;
            var actual3 = okResult3.Value.ToString();
            var expected3 = "Review Updated";

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
        public async Task _09Test_PutReviews_ReturnsNotUpdated_WhenSeller()
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
            _controllerUnderTest = new ReviewsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var review1 = new Review()
            {
                ReviewId = 12,
                ProductId = 1,
                EfUserId = 21,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 12, 22, 12, 55, 00)
            };
            var reviewsVM1 = new ReviewsVM()
            {
                ProductId = review1.ProductId,
                Rating = review1.Rating,
                Title = review1.Title,
                Comment = review1.Comment
            };

            var review2 = new Review()
            {
                ReviewId = 13,
                ProductId = 2,
                EfUserId = 22,
                Rating = 4,
                Title = "Good Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2022, 12, 22, 12, 55, 00)
            };
            var reviewsVM2 = new ReviewsVM()
            {
                ProductId = review2.ProductId,
                Rating = review2.Rating,
                Title = review2.Title,
                Comment = review2.Comment
            };

            var review3 = new Review()
            {
                ReviewId = 14,
                ProductId = 4,
                EfUserId = 24,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 02, 22, 12, 55, 00)
            };
            var reviewsVM3 = new ReviewsVM()
            {
                ProductId = review3.ProductId,
                Rating = review3.Rating,
                Title = review3.Title,
                Comment = review3.Comment
            };

            //Act            
            var result1 = await _controllerUnderTest.PutReviews(1, reviewsVM1);
            var result2 = await _controllerUnderTest.PutReviews(2, reviewsVM2);
            var result3 = await _controllerUnderTest.PutReviews(3, reviewsVM3);

            var badResult1 = (BadRequestObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "Not authorised to update reviews";

            var badResult2 = (BadRequestObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "Not authorised to update reviews";

            var badResult3 = (BadRequestObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "Not authorised to update reviews";

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
        public async Task _10Test_PutReviews_ReturnsNotFoundResult()
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
            _controllerUnderTest = new ReviewsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };
            var review1 = new Review()
            {
                ReviewId = 12,
                ProductId = 1,
                EfUserId = 21,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 12, 22, 12, 55, 00)
            };
            var reviewsVM1 = new ReviewsVM()
            {
                ProductId = review1.ProductId,
                Rating = review1.Rating,
                Title = review1.Title,
                Comment = review1.Comment
            };

            var review2 = new Review()
            {
                ReviewId = 13,
                ProductId = 2,
                EfUserId = 22,
                Rating = 4,
                Title = "Good Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2022, 12, 22, 12, 55, 00)
            };
            var reviewsVM2 = new ReviewsVM()
            {
                ProductId = review2.ProductId,
                Rating = review2.Rating,
                Title = review2.Title,
                Comment = review2.Comment
            };

            var review3 = new Review()
            {
                ReviewId = 14,
                ProductId = 4,
                EfUserId = 24,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 02, 22, 12, 55, 00)
            };
            var reviewsVM3 = new ReviewsVM()
            {
                ProductId = review3.ProductId,
                Rating = review3.Rating,
                Title = review3.Title,
                Comment = review3.Comment
            };



            //Act            
            var result1 = await _controllerUnderTest.PutReviews(11, reviewsVM1);
            var result2 = await _controllerUnderTest.PutReviews(12, reviewsVM2);
            var result3 = await _controllerUnderTest.PutReviews(13, reviewsVM3);

            var badResult1 = (NotFoundObjectResult)result1;
            var actual1 = badResult1.Value.ToString();
            var expected1 = "No Review with that ID exists, please try again";

            var badResult2 = (NotFoundObjectResult)result2;
            var actual2 = badResult2.Value.ToString();
            var expected2 = "No Review with that ID exists, please try again";

            var badResult3 = (NotFoundObjectResult)result3;
            var actual3 = badResult3.Value.ToString();
            var expected3 = "No Review with that ID exists, please try again";

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
        public async Task _11Test_PostReview_ReturnsActionResultObjectWith13Reviews_WhenAdded3Reviews()
        {
            //Arrange
            var review1 = new Review()
            {
                ReviewId = 12,
                ProductId = 1,
                EfUserId = 21,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 12, 22, 12, 55, 00)
            };
            var reviewsVM1 = new ReviewsVM()
            {
                ProductId = review1.ProductId,
                Rating = review1.Rating,
                Title = review1.Title,
                Comment = review1.Comment
            };

            var review2 = new Review()
            {
                ReviewId = 13,
                ProductId = 2,
                EfUserId = 22,
                Rating = 4,
                Title = "Good Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2022, 12, 22, 12, 55, 00)
            };
            var reviewsVM2 = new ReviewsVM()
            {
                ProductId = review2.ProductId,
                Rating = review2.Rating,
                Title = review2.Title,
                Comment = review2.Comment
            };

            var review3 = new Review()
            {
                ReviewId = 14,
                ProductId = 4,
                EfUserId = 24,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 02, 22, 12, 55, 00)
            };
            var reviewsVM3 = new ReviewsVM()
            {
                ProductId = review3.ProductId,
                Rating = review3.Rating,
                Title = review3.Title,
                Comment = review3.Comment
            };


            //Act            
            var result1 = await _controllerUnderTest.PostReviews(_reviewVM);
            var result2 = await _controllerUnderTest.PostReviews(reviewsVM2);
            var result3 = await _controllerUnderTest.PostReviews(reviewsVM3);

            //Assert
            Assert.NotNull(_eCommerceContext.Reviews);
            Assert.AreEqual(13, _eCommerceContext.Reviews.Count());

        }


        [TestCase(11)]
        [TestCase(12)]
        [TestCase(10)]
        [TestCase(9)]
        [TestCase(8)]
        [TestCase(7)]
        public async Task _12Test_GetReviewByID_ReturnsWithCorrectType_WhenPassedInID(int id)
        {
            //Arrange 
            var review1 = new Review()
            {
                ReviewId = 12,
                ProductId = 1,
                EfUserId = 21,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 12, 22, 12, 55, 00)
            };
            var reviewsVM1 = new ReviewsVM()
            {
                ProductId = review1.ProductId,
                Rating = review1.Rating,
                Title = review1.Title,
                Comment = review1.Comment
            };

            var review2 = new Review()
            {
                ReviewId = 13,
                ProductId = 2,
                EfUserId = 22,
                Rating = 4,
                Title = "Good Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2022, 12, 22, 12, 55, 00)
            };
            var reviewsVM2 = new ReviewsVM()
            {
                ProductId = review2.ProductId,
                Rating = review2.Rating,
                Title = review2.Title,
                Comment = review2.Comment
            };

            var review3 = new Review()
            {
                ReviewId = 14,
                ProductId = 4,
                EfUserId = 24,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 02, 22, 12, 55, 00)
            };
            var reviewsVM3 = new ReviewsVM()
            {
                ProductId = review3.ProductId,
                Rating = review3.Rating,
                Title = review3.Title,
                Comment = review3.Comment
            };


            await _controllerUnderTest.PostReviews(reviewsVM1);
            await _controllerUnderTest.PostReviews(reviewsVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetReviews(id);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Review>>(actionResult);
        }



        [Test]
        public async Task _12Test_GetAllreview_ReturnsWithCorrectTypeAndCount()
        {
            //Arrange 
            var review1 = new Review()
            {
                ReviewId = 12,
                ProductId = 1,
                EfUserId = 21,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 12, 22, 12, 55, 00)
            };
            var reviewsVM1 = new ReviewsVM()
            {
                ProductId = review1.ProductId,
                Rating = review1.Rating,
                Title = review1.Title,
                Comment = review1.Comment
            };

            var review2 = new Review()
            {
                ReviewId = 13,
                ProductId = 2,
                EfUserId = 22,
                Rating = 4,
                Title = "Good Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2022, 12, 22, 12, 55, 00)
            };
            var reviewsVM2 = new ReviewsVM()
            {
                ProductId = review2.ProductId,
                Rating = review2.Rating,
                Title = review2.Title,
                Comment = review2.Comment
            };

            var review3 = new Review()
            {
                ReviewId = 14,
                ProductId = 4,
                EfUserId = 24,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 02, 22, 12, 55, 00)
            };
            var reviewsVM3 = new ReviewsVM()
            {
                ProductId = review3.ProductId,
                Rating = review3.Rating,
                Title = review3.Title,
                Comment = review3.Comment
            };


            await _controllerUnderTest.PostReviews(reviewsVM1);
            await _controllerUnderTest.PostReviews(reviewsVM2);
            await _controllerUnderTest.PostReviews(reviewsVM3);

            //Act            
            var actionResult = await _controllerUnderTest.GetReviews();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<IEnumerable<Review>>>(actionResult);
            var result = (OkObjectResult)actionResult.Result;
            var value = (List<Review>)result.Value;
            Assert.AreEqual(_eCommerceContext.Reviews.Count(), value.Count);
        }

        [Test]
        public async Task _13Test_GetreviewById_ReturnsWithCorrectType()
        {
            //Arrange 
            var review1 = new Review()
            {
                ReviewId = 12,
                ProductId = 1,
                EfUserId = 21,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 12, 22, 12, 55, 00)
            };
            var reviewsVM1 = new ReviewsVM()
            {
                ProductId = review1.ProductId,
                Rating = review1.Rating,
                Title = review1.Title,
                Comment = review1.Comment
            };

            var review2 = new Review()
            {
                ReviewId = 13,
                ProductId = 2,
                EfUserId = 22,
                Rating = 4,
                Title = "Good Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2022, 12, 22, 12, 55, 00)
            };
            var reviewsVM2 = new ReviewsVM()
            {
                ProductId = review2.ProductId,
                Rating = review2.Rating,
                Title = review2.Title,
                Comment = review2.Comment
            };

            var review3 = new Review()
            {
                ReviewId = 14,
                ProductId = 4,
                EfUserId = 24,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 02, 22, 12, 55, 00)
            };
            var reviewsVM3 = new ReviewsVM()
            {
                ProductId = review3.ProductId,
                Rating = review3.Rating,
                Title = review3.Title,
                Comment = review3.Comment
            };



            await _controllerUnderTest.PostReviews(_reviewVM);
            await _controllerUnderTest.PostReviews(reviewsVM2);

            //Act            
            var actionResult = await _controllerUnderTest.GetReviews(11);

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Review>>(actionResult);
        }


        [Test]
        public async Task _14Test_PostReviews_NotAddedAndShowsInContextCount_WhenUserIsASeller()
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
            _controllerUnderTest = new ReviewsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };


            //Act            
            var actionResult = await _controllerUnderTest.PostReviews(_reviewVM);
            //Assert
            Assert.IsFalse(_eCommerceContext.Reviews.Where(c => c.ReviewId == 11).Count() > 0);
            Assert.IsTrue(_eCommerceContext.Reviews.Count() == 10);

        }


        [Test]
        public async Task _15Test_PostReviews_ReturnsBadObjectResult_WhenUserIsACustomer()
        {
            //Arrange

            //Act            
            var actionResult = await _controllerUnderTest.PostReviews(_reviewVM);

            //Assert
            var result = (ActionResult<Review>)actionResult.Result;
            var goodResult = (OkObjectResult)actionResult.Result;
            var expected = "Not authorised to add reviews as a customer";
            var actual = goodResult.Value.ToString();

            //Assert
            Assert.NotNull(actionResult);
            Assert.IsInstanceOf<ActionResult<Review>>(actionResult);
            Assert.AreEqual(goodResult.StatusCode, 200);
            Assert.IsFalse(actual.Contains(expected));
            Assert.AreEqual(11, _eCommerceContext.Reviews.Count());
        }


        [Test]
        public async Task _16Test_DeleteReviews_ReturnsMessageThatSellerCannotDeleteReview()
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
            _controllerUnderTest = new ReviewsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var review1 = new Review()
            {
                ReviewId = 12,
                ProductId = 1,
                EfUserId = 21,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 12, 22, 12, 55, 00)
            };
            var reviewsVM1 = new ReviewsVM()
            {
                ProductId = review1.ProductId,
                Rating = review1.Rating,
                Title = review1.Title,
                Comment = review1.Comment
            };

            var review2 = new Review()
            {
                ReviewId = 13,
                ProductId = 2,
                EfUserId = 22,
                Rating = 4,
                Title = "Good Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2022, 12, 22, 12, 55, 00)
            };
            var reviewsVM2 = new ReviewsVM()
            {
                ProductId = review2.ProductId,
                Rating = review2.Rating,
                Title = review2.Title,
                Comment = review2.Comment
            };

            var review3 = new Review()
            {
                ReviewId = 14,
                ProductId = 4,
                EfUserId = 24,
                Rating = 4,
                Title = "Great Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2023, 02, 22, 12, 55, 00)
            };
            var reviewsVM3 = new ReviewsVM()
            {
                ProductId = review3.ProductId,
                Rating = review3.Rating,
                Title = review3.Title,
                Comment = review3.Comment
            };


            //Act

            var actionResultDeleted = await _controllerUnderTest.DeleteReviews(8);
            var result = (ActionResult<Review>)actionResultDeleted.Result;
            var badResult = (BadRequestObjectResult)actionResultDeleted.Result;
            var expected = "Not authorised to delete reviews";
            var actual = badResult.Value.ToString();

            //Assert
            Assert.IsInstanceOf<ActionResult<Review>>(actionResultDeleted);
            Assert.AreEqual(badResult.StatusCode, 400);
            Assert.IsTrue(actual.Contains(expected));
            Assert.AreEqual(10, _eCommerceContext.Reviews.Count());
        }



        [Test]
        public async Task _17Test_DeleteReviews_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount_WhenAdministrator()
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
            _controllerUnderTest = new ReviewsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var review2 = new Review()
            {
                ReviewId = 13,
                ProductId = 2,
                EfUserId = 22,
                Rating = 4,
                Title = "Good Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2022, 12, 22, 12, 55, 00)
            };
            var reviewsVM2 = new ReviewsVM()
            {
                ProductId = review2.ProductId,
                Rating = review2.Rating,
                Title = review2.Title,
                Comment = review2.Comment
            };


            //Act
            var actionResult = await _controllerUnderTest.PostReviews(reviewsVM2);
            var actionResultDeleted = await _controllerUnderTest.DeleteReviews(review2.ReviewId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Review>>(actionResultDeleted);
            Assert.AreEqual(10, _eCommerceContext.Reviews.Count());
        }


        [Test]
        public async Task _18Test_DeleteReviews_AddMultipleDeleteOne_DeleteSuccessfullyReturnsWithCorrectTypeAndShowsInContextCount_WhenAdministrator()
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
            _controllerUnderTest = new ReviewsController(_eCommerceContext, _userManager, _roleManager, _authenticationContext)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = principal }
                }
            };

            var review2 = new Review()
            {
                ReviewId = 13,
                ProductId = 2,
                EfUserId = 22,
                Rating = 4,
                Title = "Good Product",
                Comment = "I liked the product",
                ReviewDate = new DateTime(2022, 12, 22, 12, 55, 00)
            };
            var reviewsVM2 = new ReviewsVM()
            {
                ProductId = review2.ProductId,
                Rating = review2.Rating,
                Title = review2.Title,
                Comment = review2.Comment
            };

            //Act
            await _controllerUnderTest.PostReviews(_reviewVM);
            await _controllerUnderTest.PostReviews(reviewsVM2);

            var actionResultDeleted = await _controllerUnderTest.DeleteReviews(_review.ReviewId);
            var actionResultDeleted2 = await _controllerUnderTest.DeleteReviews(review2.ReviewId);

            //Assert
            Assert.NotNull(actionResultDeleted);
            Assert.IsInstanceOf<ActionResult<Review>>(actionResultDeleted);
            Assert.NotNull(actionResultDeleted2);
            Assert.IsInstanceOf<ActionResult<Review>>(actionResultDeleted2);
            Assert.AreEqual(10, _eCommerceContext.Reviews.Count());
        }

        [TestCase("2023-12-12")]
        [TestCase("2017-05-22")]
        [TestCase("2019-12-12")]
        [TestCase("2018-06-12")]
        [TestCase("2018-05-12")]
        [TestCase("2019-11-13")]
        [TestCase("2021-03-02")]
        [TestCase("2022-01-12")]
        [TestCase("2023-12-10")]
        [TestCase("2021-04-22")]
        public async Task _19Test_GetReviewByDate_ReturnsaListwithCount1_WhenCorrectReviewDateEntered(DateTime datePassedIn)
        {
            //Arrange
            DateTime date = datePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetReviewByDate(date);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Review>)okResult.Value;
            var expected = _eCommerceContext.Reviews.FirstOrDefault(c => c.ReviewDate == date);

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Review>>>(result);
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
        public async Task _20Test_GetReviewByDate_ReturnsaListwithCount0_WhenWrongReviewDateEntered(DateTime datePassedIn)
        {
            //Arrange
            DateTime date = datePassedIn;

            //Act            
            var result = await _controllerUnderTest.GetReviewByDate(date);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = "No Review with that date exists, please try again";
            var expected = "No Review with that date exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }


        [TestCase("2017-01-01", "2017-12-12")]
        [TestCase("2018-01-01", "2018-12-12")]
        [TestCase("2019-01-01", "2019-12-12")]
        [TestCase("2020-01-01", "2022-12-12")]
        [TestCase("2021-01-01", "2023-12-12")]
        public async Task _21Test_GetReviewByDate_ReturnsaListwithCount1_WhenCorrectReviewDateEntered(DateTime date1PassedIn, DateTime date2PassedIn)
        {
            //Arrange
            DateTime date1 = date1PassedIn;
            DateTime date2 = date2PassedIn;

            //Act            
            var result = await _controllerUnderTest.GetReviewByBetweenDates(date1, date2);
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<Review>)okResult.Value;

            //Assert
            Assert.IsInstanceOf<ActionResult<List<Review>>>(result);
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.IsTrue(actual.Count() > 0);

        }

        [TestCase("2012-01-01", "2012-12-12")]
        [TestCase("2013-01-01", "2013-12-12")]
        [TestCase("2014-01-01", "2014-12-12")]
        [TestCase("2015-01-01", "2015-12-12")]
        [TestCase("2016-01-01", "2016-12-12")]
        public async Task _22Test_GetReviewByDate_ReturnsaListwithCount0_WhenWrongReviewDateEntered(DateTime date1PassedIn, DateTime date2PassedIn)
        {
            //Arrange
            DateTime date1 = date1PassedIn;
            DateTime date2 = date2PassedIn;

            //Act            
            var result = await _controllerUnderTest.GetReviewByBetweenDates(date1, date2);
            var notFoundResult = (NotFoundObjectResult)result.Result;
            var actual = "No Review with that date range exists, please try again";
            var expected = "No Review with that date range exists, please try again";

            //Assert
            Assert.AreEqual(notFoundResult.StatusCode, 404);
            Assert.IsTrue(actual.Contains(expected));

        }
    }
}

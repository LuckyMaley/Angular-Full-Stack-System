using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.Repository;
using LLM_eCommerce_RESTAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace RESTApi.NunitTests
{
    public class UsersProductsRepoTests
    {
        private List<UsersProductsVM> _usersProductsList;
        private EfUser _user;
        private EfUserProduct _userProduct;
        private Product _product;
        private Category _category;
        private Mock<DbSet<EfUser>> _mockUsersDBSet;
        private Mock<DbSet<EfUserProduct>> _mockUsersProductsDBSet;
        private Mock<DbSet<Product>> _mockProductDBSet;
        private Mock<LLM_eCommerce_EFDBContext> _mockContext;

        private UsersProductsRepo _RepoUnderTest;



        [SetUp]
        public void Initialiser()
        {
            _usersProductsList = new List<UsersProductsVM>();
            _user = new EfUser();
            _userProduct = new EfUserProduct();
            _product = new Product();
            _mockUsersDBSet = new Mock<DbSet<EfUser>>();
            _mockUsersProductsDBSet = new Mock<DbSet<EfUserProduct>>();
            _mockProductDBSet = new Mock<DbSet<Product>>();

            _mockContext = new Mock<LLM_eCommerce_EFDBContext>();
            _RepoUnderTest = new UsersProductsRepo(_mockContext.Object);
        }


        [TearDown]
        public void Cleanup()
        {
            _mockContext = null;
            _RepoUnderTest = null;

            // Get All Companies with SharesPrice and Currency information
            _usersProductsList = null;
            _user = null;
            _userProduct = null;
            _product = null;
            _mockUsersDBSet = null;
            _mockUsersProductsDBSet = null;
            _mockProductDBSet = null;
        }


        [Test]
        public void _01Test_GetAllUsersProducts_ReturnsAListOfUsersProductsVM()
        {
            //Arrange 

            var userData = new List<EfUser> { _user, }.AsQueryable();
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.Provider).Returns(userData.Provider);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.Expression).Returns(userData.Expression);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.GetEnumerator()).Returns(() => userData.GetEnumerator());
            _mockContext.Setup(m => m.EfUsers).Returns(_mockUsersDBSet.Object);


            var userProductData = new List<EfUserProduct> { _userProduct, }.AsQueryable();
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.Provider).Returns(userProductData.Provider);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.Expression).Returns(userProductData.Expression);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.ElementType).Returns(userProductData.ElementType);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.GetEnumerator()).Returns(() => userProductData.GetEnumerator());
            _mockContext.Setup(m => m.EfUserProducts).Returns(_mockUsersProductsDBSet.Object);


            var productData = new List<Product> { _product, }.AsQueryable();
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(productData.Provider);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(productData.Expression);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(productData.ElementType);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => productData.GetEnumerator());
            _mockContext.Setup(m => m.Products).Returns(_mockProductDBSet.Object);


            //Act
            var actual = _RepoUnderTest.GetUsersProducts(0);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
        }

        [Test]
        public void _02Test_GetAllUsersProducts_ReturnsAListOfUsersProductsVM_WhenPassingSpecificUserId1()
        {
            //Arrange 
            _user = new EfUser()
            {
                EfUserId = 1,
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
            };
            var userData = new List<EfUser> { _user, }.AsQueryable();
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.Provider).Returns(userData.Provider);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.Expression).Returns(userData.Expression);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.GetEnumerator()).Returns(() => userData.GetEnumerator());
            _mockContext.Setup(m => m.EfUsers).Returns(_mockUsersDBSet.Object);

            
            _userProduct = new EfUserProduct()
            {
                EfUserProductId = 1,
                EfUserId = 1,
                ProductId = 1
            };
            var userProductData = new List<EfUserProduct> { _userProduct, }.AsQueryable();
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.Provider).Returns(userProductData.Provider);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.Expression).Returns(userProductData.Expression);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.ElementType).Returns(userProductData.ElementType);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.GetEnumerator()).Returns(() => userProductData.GetEnumerator());
            _mockContext.Setup(m => m.EfUserProducts).Returns(_mockUsersProductsDBSet.Object);

            _product = new Product()
            {
                ProductId = 1,
                Name = "Nike Air Force 2",
                Brand = "Nike",
                Description = "Nike Air Force 2 sneakers for track wear",
                Type = "Men",
                Price = 2100.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2018, 11, 12, 12, 45, 00)
            };
            var productData = new List<Product> { _product, }.AsQueryable();
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(productData.Provider);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(productData.Expression);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(productData.ElementType);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => productData.GetEnumerator());
            _mockContext.Setup(m => m.Products).Returns(_mockProductDBSet.Object);


            //Act
            var actual = _RepoUnderTest.GetUsersProducts(1);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
        }

        [Test]
        public void _03Test_GetAllUsersProducts_ReturnsAListOfUsersProductsVM_WhenPassingSpecificProductId21()
        {
            //Arrange 
            _user = new EfUser()
            {
                EfUserId = 1,
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
            };
            var userData = new List<EfUser> { _user, }.AsQueryable();
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.Provider).Returns(userData.Provider);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.Expression).Returns(userData.Expression);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.GetEnumerator()).Returns(() => userData.GetEnumerator());
            _mockContext.Setup(m => m.EfUsers).Returns(_mockUsersDBSet.Object);


            _userProduct = new EfUserProduct()
            {
                EfUserProductId = 1,
                EfUserId = 1,
                ProductId = 21
            };
            var userProductData = new List<EfUserProduct> { _userProduct, }.AsQueryable();
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.Provider).Returns(userProductData.Provider);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.Expression).Returns(userProductData.Expression);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.ElementType).Returns(userProductData.ElementType);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.GetEnumerator()).Returns(() => userProductData.GetEnumerator());
            _mockContext.Setup(m => m.EfUserProducts).Returns(_mockUsersProductsDBSet.Object);

            _product = new Product()
            {
                ProductId = 21,
                Name = "Nike Air Force 2",
                Brand = "Nike",
                Description = "Nike Air Force 2 sneakers for track wear",
                Type = "Men",
                Price = 2100.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2018, 11, 12, 12, 45, 00)
            };
            var productData = new List<Product> { _product, }.AsQueryable();
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(productData.Provider);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(productData.Expression);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(productData.ElementType);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => productData.GetEnumerator());
            _mockContext.Setup(m => m.Products).Returns(_mockProductDBSet.Object);


            //Act
            var actual = _RepoUnderTest.GetProductDetails(21);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(_product.ProductId, actual.ProductId);
        }

        [Test]
        public void _04Test_GetAllUsersProducts_ReturnsAListOfUsersProductsVM_WhenPassingSpecificUserProductId12()
        {
            //Arrange 
            _user = new EfUser()
            {
                EfUserId = 1,
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
            };
            var userData = new List<EfUser> { _user, }.AsQueryable();
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.Provider).Returns(userData.Provider);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.Expression).Returns(userData.Expression);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.GetEnumerator()).Returns(() => userData.GetEnumerator());
            _mockContext.Setup(m => m.EfUsers).Returns(_mockUsersDBSet.Object);


            _userProduct = new EfUserProduct()
            {
                EfUserProductId = 12,
                EfUserId = 1,
                ProductId = 21
            };
            var userProductData = new List<EfUserProduct> { _userProduct, }.AsQueryable();
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.Provider).Returns(userProductData.Provider);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.Expression).Returns(userProductData.Expression);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.ElementType).Returns(userProductData.ElementType);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.GetEnumerator()).Returns(() => userProductData.GetEnumerator());
            _mockContext.Setup(m => m.EfUserProducts).Returns(_mockUsersProductsDBSet.Object);

            _product = new Product()
            {
                ProductId = 21,
                Name = "Nike Air Force 2",
                Brand = "Nike",
                Description = "Nike Air Force 2 sneakers for track wear",
                Type = "Men",
                Price = 2100.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2018, 11, 12, 12, 45, 00)
            };
            var productData = new List<Product> { _product, }.AsQueryable();
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(productData.Provider);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(productData.Expression);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(productData.ElementType);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => productData.GetEnumerator());
            _mockContext.Setup(m => m.Products).Returns(_mockProductDBSet.Object);


            //Act
            var actual = _RepoUnderTest.GetProductsByUserProductId(12);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Count);
        }

        [Test]
        public void _05Test_GetAllUsersProducts_ReturnsZeroListOfUsersProductsVM_WhenPassingAnInvalidUserId()
        {
            //Arrange 
            _user = new EfUser()
            {
                EfUserId = 1,
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
            };
            var userData = new List<EfUser> { _user, }.AsQueryable();
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.Provider).Returns(userData.Provider);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.Expression).Returns(userData.Expression);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.GetEnumerator()).Returns(() => userData.GetEnumerator());
            _mockContext.Setup(m => m.EfUsers).Returns(_mockUsersDBSet.Object);


            _userProduct = new EfUserProduct()
            {
                EfUserProductId = 12,
                EfUserId = 1,
                ProductId = 21
            };
            var userProductData = new List<EfUserProduct> { _userProduct, }.AsQueryable();
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.Provider).Returns(userProductData.Provider);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.Expression).Returns(userProductData.Expression);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.ElementType).Returns(userProductData.ElementType);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.GetEnumerator()).Returns(() => userProductData.GetEnumerator());
            _mockContext.Setup(m => m.EfUserProducts).Returns(_mockUsersProductsDBSet.Object);

            _product = new Product()
            {
                ProductId = 21,
                Name = "Nike Air Force 2",
                Brand = "Nike",
                Description = "Nike Air Force 2 sneakers for track wear",
                Type = "Men",
                Price = 2100.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2018, 11, 12, 12, 45, 00)
            };
            var productData = new List<Product> { _product, }.AsQueryable();
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(productData.Provider);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(productData.Expression);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(productData.ElementType);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => productData.GetEnumerator());
            _mockContext.Setup(m => m.Products).Returns(_mockProductDBSet.Object);


            //Act
            var actual = _RepoUnderTest.GetUsersProducts(22);

            //Assert
            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public void _06Test_GetAllUsersProducts_ReturnsZeroListOfUsersProductsVM_WhenPassingAnInvalidUserProductId()
        {
            //Arrange 
            _user = new EfUser()
            {
                EfUserId = 1,
                FirstName = "Test",
                LastName = "Test",
                Email = "Test",
            };
            var userData = new List<EfUser> { _user, }.AsQueryable();
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.Provider).Returns(userData.Provider);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.Expression).Returns(userData.Expression);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            _mockUsersDBSet.As<IQueryable<EfUser>>().Setup(m => m.GetEnumerator()).Returns(() => userData.GetEnumerator());
            _mockContext.Setup(m => m.EfUsers).Returns(_mockUsersDBSet.Object);


            _userProduct = new EfUserProduct()
            {
                EfUserProductId = 12,
                EfUserId = 1,
                ProductId = 21
            };
            var userProductData = new List<EfUserProduct> { _userProduct, }.AsQueryable();
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.Provider).Returns(userProductData.Provider);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.Expression).Returns(userProductData.Expression);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.ElementType).Returns(userProductData.ElementType);
            _mockUsersProductsDBSet.As<IQueryable<EfUserProduct>>().Setup(m => m.GetEnumerator()).Returns(() => userProductData.GetEnumerator());
            _mockContext.Setup(m => m.EfUserProducts).Returns(_mockUsersProductsDBSet.Object);

            _product = new Product()
            {
                ProductId = 21,
                Name = "Nike Air Force 2",
                Brand = "Nike",
                Description = "Nike Air Force 2 sneakers for track wear",
                Type = "Men",
                Price = 2100.99f,
                CategoryId = 1,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2018, 11, 12, 12, 45, 00)
            };
            var productData = new List<Product> { _product, }.AsQueryable();
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(productData.Provider);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(productData.Expression);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(productData.ElementType);
            _mockProductDBSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(() => productData.GetEnumerator());
            _mockContext.Setup(m => m.Products).Returns(_mockProductDBSet.Object);


            //Act
            var actual = _RepoUnderTest.GetProductsByUserProductId(20);

            //Assert
            Assert.AreEqual(0, actual.Count);
        }
    }
}

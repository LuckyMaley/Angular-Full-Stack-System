using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.Repository;
using LLM_eCommerce_RESTAPI.ViewModels;
using Moq;

namespace RESTApi.NunitTests
{
    [TestFixture]
    public class CustomerProductsRepoTests
    {
        private Mock<LLM_eCommerce_EFDBContext> _mockContext;
        private Mock<CategoriesProductsRepo> _mockCategoriesProductsRepo;

        private List<Category> _categoriesList;
        private Category _categories;

        private List<CategoriesProductsVM> _categoriesProductsList;
        private CategoriesProductsVM _categoriesProducts;

        [SetUp]
        public void Initialiser()
        {
            _mockContext = new Mock<LLM_eCommerce_EFDBContext>();
            _mockCategoriesProductsRepo = new Mock<CategoriesProductsRepo>(_mockContext.Object);
            _categoriesList = new List<Category>();
            _categories = new Category()
            {
                CategoryId = 1,
                Name = "Sweater"
            };

            _categoriesProductsList = new List<CategoriesProductsVM>();
            _categoriesProducts = new CategoriesProductsVM()
            {
                CategoryId = 2,
                CategoryName = "Test",
                ProductId = 243,
                Name = "Nike Air Force 2",
                Brand = "Nike",
                Description = "Nike Air Force 2 sneakers for track wear",
                Type = "Men",
                Price = 2100.99f,
                StockQuantity = 100,
                ModifiedDate = new DateTime(2018, 11, 12, 12, 45, 00)
            };
        }

        [TearDown]
        public void Cleanup()
        {
            _mockContext = null;
            _mockCategoriesProductsRepo = null;
            _categoriesList = null;
            _categories = null;
            _categoriesProductsList = null;
            _categoriesProducts = null;
        }

        [Test]
        public void _01Test_GetAllCategories_IsCalledOnce()
        {
            //Act
            _categoriesList = _mockCategoriesProductsRepo.Object.GetAllCategories();

            //Assert
            _mockCategoriesProductsRepo.Verify(n => n.GetAllCategories(), Times.Once);
        }

        [Test]
        public void _02Test_GetAllCategories_ReturnsEmptyList()
        {
            //Arrange
            _mockCategoriesProductsRepo.Setup(n => n.GetAllCategories()).Returns(_categoriesList);

            //Act
            var actual = _mockCategoriesProductsRepo.Object.GetAllCategories();
            var expected = _categoriesList;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void _03Test_GetAllCategories_ReturnsListOf3_WhenCalledWith3Entries()
        {
            //Arrange
            _categoriesList.Add(_categories);
            _categoriesList.Add(_categories);
            _categoriesList.Add(_categories);
            _mockCategoriesProductsRepo.Setup(n => n.GetAllCategories()).Returns(_categoriesList);

            //Act
            var actual = _mockCategoriesProductsRepo.Object.GetAllCategories();
            var expected = _categoriesList;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void _04Test_GetCategoriesWithId_IsCalledOnce()
        {
            //Arrange
            int id = _categories.CategoryId;

            //Act
            _categories = _mockCategoriesProductsRepo.Object.GetCategoriesWithId(id);

            //Assert
            _mockCategoriesProductsRepo.Verify(n => n.GetCategoriesWithId(id), Times.Once);
        }

        [Test]
        public void _05Test_GetCategoriesWithId_ReturnsAValidStockEx_WhenCalledWithId1()
        {
            //Arrange
            int id = _categories.CategoryId;
            _mockCategoriesProductsRepo.Setup(n => n.GetCategoriesWithId(id)).Returns(_categories);

            //Act
            var actual = _mockCategoriesProductsRepo.Object.GetCategoriesWithId(id);
            var expected = _categories;

            //Assert
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void _06Test_GetAllCategoriesProducts_IsCalledOnce()
        {
            //Arrange

            //Act
            _categoriesProductsList = _mockCategoriesProductsRepo.Object.GetAllCategoriesProducts();

            //Assert
            _mockCategoriesProductsRepo.Verify(n => n.GetAllCategoriesProducts(), Times.Once);
        }



        [Test]
        public void _07Test_GetAllCategoriesProducts_ReturnsListOf3_WhenCalledWith3Entries()
        {
            //Arrange
            _categoriesProductsList.Add(_categoriesProducts);
            _categoriesProductsList.Add(_categoriesProducts);
            _categoriesProductsList.Add(_categoriesProducts);
            _mockCategoriesProductsRepo.Setup(n => n.GetAllCategoriesProducts()).Returns(_categoriesProductsList);

            //Act
            var actual = _mockCategoriesProductsRepo.Object.GetAllCategoriesProducts();
            var expected = _categoriesProductsList;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

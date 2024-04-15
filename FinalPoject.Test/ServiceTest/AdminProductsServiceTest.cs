using System.Text.Json;
using Core.Services.Services;
using FinalProject.Configurations;
using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using FinalProject.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic.FileIO;
using Moq;

namespace FinalPoject.Test.ServiceTest;

public class AdminProductsServiceTest
{
    private AdminProductsService _adminProductsService;
    private Mock<IAdminProductsRepository<Product>> _productsRepositoryMock;
    private Mock<IWebHostEnvironment> _environmentMock;

    private Mock<DependencyConfiguration> _mockDependencyConfiguration;
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;

    [SetUp]
    public void Setup()
    {
        _mockDependencyConfiguration = new Mock<DependencyConfiguration>();
        _productsRepositoryMock = new Mock<IAdminProductsRepository<Product>>();
        _environmentMock = new Mock<IWebHostEnvironment>();

        _adminProductsService = new AdminProductsService(_productsRepositoryMock.Object, _environmentMock.Object,
            _mockDependencyConfiguration.Object);

        _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null,
            null, null, null, null, null, null);
        _roleManagerMock =
            new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManagerMock.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            null,
            null,
            null,
            null
        );
        _mockDependencyConfiguration.Setup(x => x._userManager).Returns(_userManagerMock.Object);
        _mockDependencyConfiguration.Setup(x => x._roleManager).Returns(_roleManagerMock.Object);
        _mockDependencyConfiguration.Setup(x => x._signInManager).Returns(_signInManagerMock.Object);
    }

    [Test]
    public void FilterProductsByItsInput_ShouldReturnProductsByDescending_WhenActionFilterIsHighToLow()
    {
        // Arrange
        var currentPage = 1;
        var numberOfItems = 6;
        var actionName = "High to Low";

        var products = new List<Product>()
        {
            new Product() { Id = 1, Name = "name1", Price = 10 },
            new Product() { Id = 2, Name = "name2", Price = 20 },
            new Product() { Id = 3, Name = "name3", Price = 30 }
        };

        _productsRepositoryMock.Setup(x => x.GetAllProducts()).Returns(products);

        var expected = products.OrderByDescending(x => x.Price);

        // Act
        var result =
            _adminProductsService.FilterProductsByItsInput(currentPage, numberOfItems, actionName,
                filterInput: string.Empty);

        // Assert
        Assert.IsTrue(expected.SequenceEqual(result));
    }

    [Test]
    public void FilterProductsByItsInput_ShouldReturnProductsByAscending_WhenActionFilterIsLowToHigh()
    {
        // Arrange
        var currentPage = 1;
        var numberOfItems = 6;
        var actionName = "Low to Hight";

        var products = new List<Product>()
        {
            new Product() { Id = 1, Name = "name1", Price = 10 },
            new Product() { Id = 2, Name = "name2", Price = 20 },
            new Product() { Id = 3, Name = "name3", Price = 30 }
        };

        _productsRepositoryMock.Setup(x => x.GetAllProducts()).Returns(products);

        var expected = products.OrderBy(x => x.Price);

        // Act
        var result =
            _adminProductsService.FilterProductsByItsInput(currentPage, numberOfItems, actionName,
                filterInput: string.Empty);

        // Assert
        Assert.IsTrue(expected.SequenceEqual(result));
    }

    [Test]
    public void FilterProductsByItsInput_ShouldReturnProductsBySearchInput_WhenActionFilterIsSearch()
    {
        // Arrange
        var currentPage = 1;
        var numberOfItems = 6;
        var actionName = "Search";
        var filterInput = "name1";


        var products = new List<Product>()
        {
            new Product() { Id = 1, Name = "name1", Price = 10 },
            new Product() { Id = 2, Name = "name2", Price = 20 },
            new Product() { Id = 3, Name = "name3", Price = 30 }
        };

        _productsRepositoryMock.Setup(x => x.GetAllProducts()).Returns(products);

        var expected = products.Where(x => x.Name.Contains("name1"));

        // Act
        var result =
            _adminProductsService.FilterProductsByItsInput(currentPage, numberOfItems, actionName, filterInput);

        // Assert
        Assert.IsTrue(expected.SequenceEqual(result));
    }

    [Test]
    public void FilterProductsByItsInput_ShouldReturnProducts_WhenActionFilterAndFilterInputIsEmpty()
    {
        // Arrange
        var currentPage = 1;
        var numberOfItems = 6;

        var products = new List<Product>()
        {
            new Product() { Id = 1, Name = "name1", Price = 10 },
            new Product() { Id = 2, Name = "name2", Price = 20 },
            new Product() { Id = 3, Name = "name3", Price = 30 }
        };

        _productsRepositoryMock.Setup(x => x.GetAllProducts()).Returns(products);

        var expected = products;

        // Act
        var result = _adminProductsService.FilterProductsByItsInput(currentPage, numberOfItems,
            actionForFilter: string.Empty, filterInput: string.Empty);

        // Assert
        Assert.IsTrue(expected.SequenceEqual(result));
    }

    [Test]
    public void DeleteItem_WhenProductWontDelete_ReturnsFalse()
    {
        //Arrange
        var id = 1;
        _productsRepositoryMock.Setup(x => x.GetAllProducts()).Returns(Enumerable.Empty<Product>());

        //Act
        var result = _adminProductsService.DeleteItem(id);

        //Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void DeleteItem_WhenProductDelete_ReturnsTrue()
    {
        // Arrange
        var id = 1;
        var product = new Product { Id = id, Name = "test", Image = "image" };
        _productsRepositoryMock.Setup(x => x.GetAllProducts()).Returns(new List<Product> { product });
        _productsRepositoryMock.Setup(x => x.Delete(It.IsAny<Product>())).Verifiable();
        _environmentMock.Setup(x => x.WebRootPath).Returns("C:\\SomePath");

        // Act
        var result = _adminProductsService.DeleteItem(id);

        // Assert
        Assert.IsTrue(result);
        _productsRepositoryMock.Verify(x => x.Delete(product), Times.Once);
    }

    [Test]
    public void UpdateEditedItemAsync_WhenFileIsNull_UpdateItemWithNewItemProperties()
    {
        //Arrange
        var model = new Product { Name = "Test", Price = 1 };
        _productsRepositoryMock.Setup(x => x.GetProductById(It.IsAny<int>())).Returns(model);
        _productsRepositoryMock.Setup(x => x.Update(model)).Verifiable();

        //Act
        var result = _adminProductsService.UpdateEditedItemAsync(model, It.IsAny<IFormFile>());

        //Assert
        _productsRepositoryMock.Verify(x => x.Update(model),Times.Once());
    }
}
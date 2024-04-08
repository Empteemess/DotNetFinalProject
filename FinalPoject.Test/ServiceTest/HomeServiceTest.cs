using System.Text.Json;
using FinalProject.Interfaces;
using FinalProject.Models;
using FinalProject.Services;
using Moq;

namespace FinalPoject.Test.ServiceTest;

public class HomeServiceTest
{
    private Mock<IHomeRepository<Product>> _mockHomeRepository;
    private HomeService _homeService;

    [SetUp]
    public void Setup()
    {
        _mockHomeRepository = new Mock<IHomeRepository<Product>>();
        _homeService = new HomeService(_mockHomeRepository.Object);
    }

    [Test]
    public void DivideDataForPaging_WhenCalled_ReturnDividedDataForPaging()
    {
        //Arrange
        var currentPage = 1;
        var numberOfItems = 3;
        
        var products = new List<Product>()
        {
            new Product() { Name = "Product1", Description = "Description1", Price = 1 },
            new Product() { Name = "Product2", Description = "Description2", Price = 2 },
            new Product() { Name = "Product3", Description = "Description3", Price = 3 },
            new Product() { Name = "Product4", Description = "Description4", Price = 4 }
        };

        _mockHomeRepository.Setup(x => x.GetAllProducts()).Returns(products);
        
        var exeptProducts = new List<Product>()
        {
            new Product() { Name = "Product1", Description = "Description1", Price = 1 },
            new Product() { Name = "Product2", Description = "Description2", Price = 2 },
            new Product() { Name = "Product3", Description = "Description3", Price = 3 },
        };
        var exeptProoductsJson = JsonSerializer.Serialize(exeptProducts);
        
        //Act

        var result = _homeService.DivideDataForPaging(currentPage, numberOfItems);
        var resultJson = JsonSerializer.Serialize(result);

        //Assert
        Assert.AreEqual(exeptProoductsJson,resultJson);
    }
    [Test]
    public void CheckPageNum_ShouldReturnFalse_WhenCurrentPageNumIsMoreThanNumberOfItems()
    {
        // Arrange
        var currentPage = 4;
        var numberOfItems = 6;
        _mockHomeRepository.Setup(x => x.GetAllProducts()).Returns(new List<Product>());

        // Act
        var result = _homeService.CheckPageNum(currentPage, numberOfItems);

        // Assert
        Assert.IsFalse(result);
    }
    [Test]
    public void CheckPageNum_ShouldReturnTrue_WhenCurrentPageNumIsLessThanNumberOfItems()
    {
        // Arrange
        var currentPage = -1;
        var numberOfItems = 6;
        _mockHomeRepository.Setup(x => x.GetAllProducts()).Returns(new List<Product>());

        // Act
        var result = _homeService.CheckPageNum(currentPage, numberOfItems);

        // Assert
        Assert.IsTrue(result);
    }
}
using FinalProject.Interfaces;
using FinalProject.Models;
using FinalProject.Services;
using Moq;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FinalPoject.Test.ServiceTest;

public class SingleProductServiceTest
{
    private Mock<ISingleProductRepository<Product>> _mockSingleProductRepository;
    private SingleProductService _singleProductService;

    [SetUp]
    public void Setup()
    {
        _mockSingleProductRepository = new Mock<ISingleProductRepository<Product>>();
        _singleProductService = new SingleProductService(_mockSingleProductRepository.Object);
    }

    [Test]
    public void MapViewModelToDto_WhenCalled_MapProductDataToProductViewModel()
    {
        //Arrange
        var id = 1;
        var currentPage = 1;
        var numberOfItems = 6;

        var product = new Product()
        {
            Name = "Product1",
            Description = "TestProduct",
            Price = 12
        };

        var products = new List<Product>()
        {
            new Product() { Name = "Product1", Description = "TestProduct1", Price = 1 },
            new Product() { Name = "Product2", Description = "TestProduct2", Price = 12 },
            new Product() { Name = "Product3", Description = "TestProduct3", Price = 123 }
        };

        _mockSingleProductRepository.Setup(x => x.GetProductById(It.IsAny<int>())).Returns(product);
        _mockSingleProductRepository.Setup(x => x.GetAllProducts()).Returns(products);

        var expect = new ProductViewModel()
        {
            Name = "Product1",
            Description = "TestProduct",
            Price = 12,
            Product = products
        };

        var expectJson = JsonSerializer.Serialize(expect);

        //Act

        var result = _singleProductService.MapViewModelToDto(id, currentPage, numberOfItems);
        var resultJson = JsonSerializer.Serialize(result);

        //Assert
        Assert.AreEqual(expectJson, resultJson);
    }
    [Test]
    public void CheckProduct_WhenCalled_ReturnsFalseIfProductIsNull()
    {
        //Arrange
        var id = 1;
        _mockSingleProductRepository.Setup(x => x.GetProductById(It.IsAny<int>())).Returns((int id) => null);

        //Act
        var result = _singleProductService.CheckProduct(id);

        //Assert
        Assert.IsFalse(result);
    }
    [Test]
    public void CheckProduct_WhenCalled_ReturnsTrueIfProductIsNotNull()
    {
        //Arrange
        var id = 1;
        _mockSingleProductRepository.Setup(x => x.GetProductById(It.IsAny<int>())).Returns(new Product());

        //Act
        var result = _singleProductService.CheckProduct(id);

        //Assert
        Assert.IsTrue(result);
    }
}
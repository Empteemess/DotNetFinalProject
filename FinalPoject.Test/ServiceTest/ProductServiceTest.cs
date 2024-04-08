using System.Text.Json;
using Core.Services.Services;
using FakeItEasy;
using FinalProject.Interfaces;
using FinalProject.Models;
using Moq;
namespace FinalPoject.Test.ServiceTest;

public class ProductServiceTest
{
    private Mock<IProductRepository<Product>> _mockProductRepository;
    private ProductService _productService;

    [SetUp]
    public void Setup()
    {
        _mockProductRepository = new Mock<IProductRepository<Product>>();
        _productService = new ProductService(_mockProductRepository.Object);
    }

    [Test]
    public void CheckPageNum_ShouldReturnFalse_WhenCurrentPageNumIsMoreThanNumberOfItems()
    {
        // Arrange
        var currentPage = 4;
        var numberOfItems = 6;
        _mockProductRepository.Setup(x => x.GetAllProducts()).Returns(new List<Product>());

        // Act
        var result = _productService.CheckPageNum(currentPage, numberOfItems);

        // Assert
        Assert.IsFalse(result);
    }
    [Test]
    public void CheckPageNum_ShouldReturnTrue_WhenCurrentPageNumIsLessThanNumberOfItems()
    {
        // Arrange
        var currentPage = -1;
        var numberOfItems = 6;
        _mockProductRepository.Setup(x => x.GetAllProducts()).Returns(new List<Product>());

        // Act
        var result = _productService.CheckPageNum(currentPage, numberOfItems);

        // Assert
        Assert.IsTrue(result);
    }
    [Test]
    public void GetProductsByItsInput_ShouldReturnProductsByDescending_WhenActionFilterIsHighToLow()
    {
        //Arrange
        var currentPage = 1;
        var numberOfItems = 6;
        var actionName = "High to Low";

        var products = new List<Product>()
        {
            new Product() { Id = 1, Name = "name1", Price = 10 },
            new Product() { Id = 2, Name = "name2", Price = 20 },
            new Product() { Id = 3, Name = "name3", Price = 30 }
        };

        _mockProductRepository.Setup(x => x.GetAllProducts()).Returns(products);

        var expected = products.OrderByDescending(x => x.Price).Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        });

        var expectedJson = JsonSerializer.Serialize(expected);
        //Act

        var result =
            _productService.GetProductsByItsInput(currentPage, numberOfItems, actionName, filterInput: string.Empty);
        var resultJson = JsonSerializer.Serialize(result);
        
        //Assert
        Assert.AreEqual(expectedJson,resultJson);
    }
    [Test]
    public void GetProductsByItsInput_ShouldReturnProductsByAscending_WhenActionFilterIsLowToHigh()
    {
        //Arrange
        var currentPage = 1;
        var numberOfItems = 6;
        var actionName = "Low to Hight";

        var products = new List<Product>()
        {
            new Product() { Id = 1, Name = "name1", Price = 10 },
            new Product() { Id = 2, Name = "name2", Price = 20 },
            new Product() { Id = 3, Name = "name3", Price = 30 }
        };

        _mockProductRepository.Setup(x => x.GetAllProducts()).Returns(products);

        var expected = products.OrderBy(x => x.Price).Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        });

        var expectedJson = JsonSerializer.Serialize(expected);
        //Act

        var result =
            _productService.GetProductsByItsInput(currentPage, numberOfItems, actionName, filterInput: string.Empty);
        var resultJson = JsonSerializer.Serialize(result);
        
        //Assert
        Assert.AreEqual(expectedJson,resultJson);
    }
    [Test]
    public void GetProductsByItsInput_ShouldReturnSearchedProducts_WhenActionFilterIsSearch()
    {
        //Arrange
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

        _mockProductRepository.Setup(x => x.GetAllProducts()).Returns(products);

        var expected = products.Where(x => x.Name.ToLower().Contains(filterInput)).Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        });

        var expectedJson = JsonSerializer.Serialize(expected);
        //Act

        var result =
            _productService.GetProductsByItsInput(currentPage, numberOfItems, actionName, filterInput);
        var resultJson = JsonSerializer.Serialize(result);
        
        //Assert
        Assert.AreEqual(expectedJson,resultJson);
    }
    [Test]
    public void GetProductsByItsInput_ShouldReturnMaleProducts_WhenActionFilterIsMale()
    {
        //Arrange
        var currentPage = 1;
        var numberOfItems = 6;
        var actionName = "Male";

        var products = new List<Product>()
        {
            new Product() { Id = 1, Name = "name1", Price = 10 },
            new Product() { Id = 2, Name = "name2", Price = 20 },
            new Product() { Id = 3, Name = "name3", Price = 30 }
        };

        _mockProductRepository.Setup(x => x.GetAllProducts()).Returns(products);

        var expected = products.Where(x => (int)x.CategoryEnum == 2).Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        });

        var expectedJson = JsonSerializer.Serialize(expected);
        //Act

        var result =
            _productService.GetProductsByItsInput(currentPage, numberOfItems, actionName, filterInput : string.Empty);
        var resultJson = JsonSerializer.Serialize(result);
        
        //Assert
        Assert.AreEqual(expectedJson,resultJson);
    }
    [Test]
    public void GetProductsByItsInput_ShouldReturnFemaleProducts_WhenActionFilterIsFemale()
    {
        //Arrange
        var currentPage = 1;
        var numberOfItems = 6;
        var actionName = "Female";

        var products = new List<Product>()
        {
            new Product() { Id = 1, Name = "name1", Price = 10 },
            new Product() { Id = 2, Name = "name2", Price = 20 },
            new Product() { Id = 3, Name = "name3", Price = 30 }
        };

        _mockProductRepository.Setup(x => x.GetAllProducts()).Returns(products);

        var expected = products.Where(x => (int)x.CategoryEnum == 1).Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        });

        var expectedJson = JsonSerializer.Serialize(expected);
        //Act

        var result =
            _productService.GetProductsByItsInput(currentPage, numberOfItems, actionName, filterInput : string.Empty);
        var resultJson = JsonSerializer.Serialize(result);
        
        //Assert
        Assert.AreEqual(expectedJson,resultJson);
    }
    [Test]
    public void GetProductsByItsInput_ShouldReturnProducts_WhenActionFilterIsEmpty()
    {
        //Arrange
        var currentPage = 1;
        var numberOfItems = 6;

        var products = new List<Product>()
        {
            new Product() { Id = 1, Name = "name1", Price = 10 },
            new Product() { Id = 2, Name = "name2", Price = 20 },
            new Product() { Id = 3, Name = "name3", Price = 30 }
        };

        _mockProductRepository.Setup(x => x.GetAllProducts()).Returns(products);

        var expected = products.Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        });

        var expectedJson = JsonSerializer.Serialize(expected);
        //Act

        var result =
            _productService.GetProductsByItsInput(currentPage, numberOfItems, actionName : string.Empty, filterInput : string.Empty);
        var resultJson = JsonSerializer.Serialize(result);
        
        //Assert
        Assert.AreEqual(expectedJson,resultJson);
    }
}
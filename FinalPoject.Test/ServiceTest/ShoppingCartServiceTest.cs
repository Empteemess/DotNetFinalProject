using Core.Services.Services;
using FinalProject.Configurations;
using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FinalPoject.Test.ServiceTest;

public class ShoppingCartServiceTest
{
    private ShoppingCartService _shoppingCartService;
    private Mock<DependencyConfiguration> _mockDependencyConfiguration;
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private Mock<IShoppingCartRepository> _mockRepository;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private DbContextOptions<AppDbContext> _options;
    private AppDbContext _context;

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(_options);
        _mockRepository = new Mock<IShoppingCartRepository>();

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _mockDependencyConfiguration = new Mock<DependencyConfiguration>();
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

        _shoppingCartService = new ShoppingCartService(_mockDependencyConfiguration.Object,
            _httpContextAccessorMock.Object, _mockRepository.Object, _context);
    }

    [Test]
    public async Task GetSelledProductPriceAsync_Whencalled_ReturnWholePrice()
    {
        //Arrange
        var wholePrice = 0.0;
        var user = new ApplicationUser
        {
            CartProducts = new List<CartProducts>
            {
                new CartProducts()
                {
                    SellQuantity = 2,
                    Price = 10
                },
                new CartProducts()
                {
                    SellQuantity = 1,
                    Price = 10
                }
            }
        };

        _mockRepository.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(user);

        //Act
        var result = await _shoppingCartService.GetSelledProductPriceAsync();

        //Assert
        Assert.AreEqual(30, result);
    }

    [Test]
    public async Task UpdateEditedItemAsync_WhenExactItemIsNull_ReturnsFalse()
    {
        //Arrange

        var model = new CartProducts
        {
            Id = 3,
            Name = "Test3",
            SellQuantity = 112
        };

        var user = new ApplicationUser()
        {
            CartProducts = new List<CartProducts>
            {
                new CartProducts
                {
                    Id = 1,
                    Name = "test1",
                    SellQuantity = 12,
                },
                new CartProducts
                {
                    Id = 2,
                    Name = "test2",
                    SellQuantity = 12,
                }
            }
        };

        _mockRepository.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(user);

        //Act
        var result = await _shoppingCartService.UpdateEditedItemAsync(model);

        //Assert
        Assert.IsFalse(result);
    }

    [Test]
    public async Task UpdateEditedItemAsync_WhenExactItemIsntNull_ReturnsTrue()
    {
        // Arrange
        var user = new ApplicationUser()
        {
            CartProducts = new List<CartProducts>
            {
                new CartProducts
                {
                    Id = 1,
                    ProductId = 11,
                    Name = "test1",
                    SellQuantity = 12,
                },
                new CartProducts
                {
                    Id = 2,
                    Name = "test2",
                    SellQuantity = 12,
                }
            }
        };

        var model = new CartProducts
        {
            Id = 1,
            Name = "Test3",
            SellQuantity = 112
        };

        var productToUpdate = new Product
            { Id = 11, Name = "Product 1", Description = "Description 1", Image = "image1.jpg" };

        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

        _mockRepository.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(user);
        _context.Products.Add(productToUpdate);
        await _context.SaveChangesAsync();

        // Act
        var result = await _shoppingCartService.UpdateEditedItemAsync(model);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public async Task DeleteItemAsync_WhenItemIsNull_ReturnsFalse()
    {
        //Arange
        var id = 12;
        
        var user = new ApplicationUser
        {
            CartProducts = new List<CartProducts>
            {
                new CartProducts
                {
                    Id = 1,
                    ProductId = 11,
                    Name = "test1",
                    SellQuantity = 12,
                },
                new CartProducts
                {
                    Id = 2,
                    Name = "test2",
                    SellQuantity = 12,
                }
            }
        };

        _mockRepository.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(user);
        var ss = _shoppingCartService.AddProductQuantity(It.IsAny<int>(), It.IsAny<int>());

        //Act
        var result = await _shoppingCartService.DeleteItemAsync(id);

        //Assert
        Assert.IsFalse(result);
    }
    [Test]
    public async Task DeleteItemAsync_WhenItemIsntNull_ReturnsTrue()
    {
        //Arange
        var id = 1;
        var user = new ApplicationUser
        {
            CartProducts = new List<CartProducts>
            {
                new CartProducts
                {
                    Id = 1,
                    ProductId = 11,
                    Name = "test1",
                    SellQuantity = 12,
                },
                new CartProducts
                {
                    Id = 2,
                    Name = "test2",
                    SellQuantity = 12,
                }
            }
        };
        
        _mockRepository.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(user);

        //Act
        var result = await _shoppingCartService.DeleteItemAsync(id);

        //Assert
        Assert.IsTrue(result);
    }
}
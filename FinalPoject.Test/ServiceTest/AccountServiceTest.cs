using System.Diagnostics.Tracing;
using FinalProject.Configurations;
using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using FinalProject.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FinalPoject.Test.ServiceTest;

public class AccountServiceTest
{
    private AccountService _accountService;
    private Mock<IShoppingCartService> _mockShoppingCartService;
    private Mock<DependencyConfiguration> _mockDependencyConfiguration;
    

    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;

    [SetUp]
    public void Setup()
    {
        _mockShoppingCartService = new Mock<IShoppingCartService>();

        _mockDependencyConfiguration = new Mock<DependencyConfiguration>();
        _accountService = new AccountService(_mockDependencyConfiguration.Object, _mockShoppingCartService.Object);

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
    public async Task RegisterAsync_WhenRegisterIsSuccessfull_ReturnTrue()
    {
        //Arrange
        var model = new RegisterViewModel()
        {
            Email = "bazadzegiorgi9@gmail.com",
            Password = "Giogio99@",
            ConfirmPassword = "Giogio99@",
        };

        _mockShoppingCartService.Setup(x => x.EmailSenderAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);


        //Act
        var result = await _accountService.RegisterAsync(model);

        //Assert
        Assert.IsTrue(result);
    }
    [Test]
    public async Task RegisterAsync_WhenRegisterIsntSuccessfull_ReturnFalse()
    {
        //Arrange
        var model = new RegisterViewModel()
        {
            Email = "bazadzegiorgi9@gmail.com",
            Password = "Giogio99@",
            ConfirmPassword = "Giogio99@",
        };

        _mockShoppingCartService.Setup(x => x.EmailSenderAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());

        //Act
        var result = await _accountService.RegisterAsync(model);

        //Assert
        Assert.IsFalse(result);
    }
    [Test]
    public async Task LoginAsync_WhenLoginIsSucceeded_ReturnTrue()
    {
        //Arrange
        var model = new LoginViewModel()
        {
            Email = "FakeEmail@gmail.com",
            Password = "FakePassword99@"
        };
        
        _signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false)).ReturnsAsync(SignInResult.Success);

        //Act
        var result = await _accountService.LoginAsync(model);

        //Assert
        Assert.IsTrue(result);
    }
    [Test]
    public async Task LoginAsync_WhenLoginIsntSucceeded_ReturnFalse()
    {
        //Arrange
        var model = new LoginViewModel()
        {
            Email = "FakeEmail@gmail.com",
            Password = "FakePassword99@"
        };
        
        _signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false)).ReturnsAsync(SignInResult.Failed);

        //Act
        var result = await _accountService.LoginAsync(model);

        //Assert
        Assert.IsFalse(result);
    }
    [Test]
    public async Task RemoveRoleAsync_WhenRoleDelete_ReturnSucceeded()
    {
        //Arrange
        var rolename = "Test";
        var identityRole = new IdentityRole() { Name = rolename };
        
        _roleManagerMock.Setup(x => x.FindByNameAsync(rolename)).ReturnsAsync(identityRole);
        _roleManagerMock.Setup(x => x.DeleteAsync(identityRole)).ReturnsAsync(IdentityResult.Success);

        //Act
        await _accountService.RemoveRoleAsync(new RoleViewModel() { Role = rolename });

        //Assert
        _roleManagerMock.Verify(x => x.FindByNameAsync(rolename),Times.Once);
        _roleManagerMock.Verify(x => x.DeleteAsync(identityRole),Times.Once);
    }
    [Test]
    public async Task RemoveUserAsync_WhenUserIsValid_RemoveUser()
    {
        //Arrange
        var userEmail = "TestEmail@gmail.com";
        var user = new ApplicationUser() { Email = userEmail };
        _userManagerMock.Setup(x => x.FindByEmailAsync(userEmail)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);
        

        //Act
        await _accountService.RemoveUserAsync(new RegisterViewModel{Email = userEmail});

        //Assert
        _userManagerMock.Verify(x => x.FindByEmailAsync(userEmail),Times.Once);
        _userManagerMock.Verify(x => x.DeleteAsync(user),Times.Once);
    }
    [Test]
    public async Task CheckUserAsync_WhenUsersEmailIsValid_ReturnsTrue()
    {
        //Arrange
        var email = "TestEmail@gmail.com";
        var user = new ApplicationUser() { Email = email };
        _userManagerMock.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);

        //Act
        var result = await _accountService.CheckUserAsync(email);

        //Assert
        Assert.IsTrue(result);
    }
    [Test]
    public async Task CheckUserAsync_WhenUsersEmailIsntValid_ReturnsFalse()
    {
        //Arrange
        var email = "TestEmail@gmail.com";
        var user = new ApplicationUser() { Email = email };
        _userManagerMock.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync((ApplicationUser)null);

        //Act
        var result = await _accountService.CheckUserAsync(email);

        //Assert
        Assert.IsFalse(result);
    }
    [Test]
    public async Task PasswordUpdate_WhenUserEmailIsInvalid_ReturnFalse()
    {
        //Arrange
        var email = "TestEmail@gmail.com";
        var password = "TestPassword9@";
        _userManagerMock.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync((ApplicationUser)null);
        
        //Act
        var result = await _accountService.PasswordUpdate(email, password);

        //Assert
        Assert.IsFalse(result);
    }
    [Test]
    public async Task PasswordUpdate_WhenUserEmailIsValid_ReturnTrue()
    {
        //Arrange
        var email = "TestEmail@gmail.com";
        var password = "TestPassword9@";
        var token = "TestToken";
        var user = new ApplicationUser { Email = email };
        
        _userManagerMock.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(token);
        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, token, password)).ReturnsAsync(IdentityResult.Success);
        
        //Act
        var result = await _accountService.PasswordUpdate(email, password);

        //Assert
        Assert.IsTrue(result);
        _userManagerMock.Verify(x => x.GeneratePasswordResetTokenAsync(user),Times.Once);
    }
}
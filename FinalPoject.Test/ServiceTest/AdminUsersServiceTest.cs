using Core.Services.Services;
using FinalProject.Configurations;
using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace FinalPoject.Test.ServiceTest;

public class AdminUsersServiceTest
{
    private AdminUsersService _AdminUserService;

    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private Mock<DependencyConfiguration> _mockDependencyConfiguration;

    [SetUp]
    public void Setup()
    {
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
       
        _AdminUserService = new AdminUsersService(_mockDependencyConfiguration.Object);
    }

    [Test]
    public async Task AddRole_WhenRolenameIsEmpty_ReturnsFalse()
    {
        //Arrange
        var rolename = string.Empty;

        //Act
        var result = await _AdminUserService.AddRole(rolename);

        //Assert
        Assert.IsFalse(result);
    }
    [Test]
    public async Task AddRole_WhenRolenameIsntEmptyAndRoleisntNull_ReturnsFalse()
    {
        //Arrange
        var roleName = "Admin";
        var role = new IdentityRole { Name = roleName };
        _roleManagerMock.Setup(x => x.FindByNameAsync(roleName)).ReturnsAsync(role);

        //Act
        var result = await _AdminUserService.AddRole(roleName);

        //Assert
        Assert.IsFalse(result);
    }
    [Test]
    public async Task AddRole_WhenRolenameIsntNullAndRoleIsNull_ReturnsTrue()
    {
        //Arrange
        var roleName = "test";
        _roleManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((IdentityRole)null);
        _roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>())).ReturnsAsync(IdentityResult.Success);

        //Act
        var result = await _AdminUserService.AddRole(roleName);

        //Assert
        Assert.IsTrue(result);
    }

    [Test]
    public async Task DeleteUserByEmail_WhenUserIsntNull_ReturnsTrue()
    {
        //Arrange
        var email = "Test@gmail.com";
        _userManagerMock.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(new ApplicationUser());

        //Act
        var result = await _AdminUserService.DeleteUserByEmail(email);

        //Assert
        Assert.IsTrue(result);
    }
    [Test]
    public async Task DeleteUserByEmail_WhenUserIsNull_ReturnsFalse()
    {
        //Arrange
        var email = "Test@gmail.com";
        _userManagerMock.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync((ApplicationUser)null);

        //Act
        var result = await _AdminUserService.DeleteUserByEmail(email);

        //Assert
        Assert.IsFalse(result);
    }

    [Test]
    public async Task FilterByInput_WhenFilterIsSearch_RetursnSearchedUser()
    {
        //Arrange
        var currenctPage = 1;
        var maxItem = 6;
        var email = "TestEmail@gmail.com";
        var filterName = "Search";

        var users = new List<UserViewModel>
        {
            new UserViewModel() { Email = "TestEmail@gmail.com", Role = "Guest" },
            new UserViewModel() { Email = "secondEmail@gmail.com", Role = "Guest" },
        };

        // _userManagerMock.Setup(x => x.Users).Returns(users);

        //Act

        //Assert
    }
}
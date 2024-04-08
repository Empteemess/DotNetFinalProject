using FinalProject.Configurations;
using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using FinalProject.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace FinalPoject.Test.ServiceTest;

public class AccountServiceTest
{
    // private AccountService _accountService;
    // private Mock<IShoppingCartService> _mockShoppingCartService;
    // private DependencyConfiguration _dependencyConfiguration;
    //
    // [SetUp]
    // public void Setup()
    // {
    //     _mockShoppingCartService = new Mock<IShoppingCartService>();
    //     _accountService = new AccountService(_dependencyConfiguration, _mockShoppingCartService.Object);
    // }
    //
    // [Test]
    // public async Task RegisterAsync_ShouldReturnTrue_WhenRegistrationSuccessfull()
    // {
    //     //Arrange
    //     var model = new RegisterViewModel()
    //     {
    //         Email = "bazadzegiorgi9@gmail.com",
    //         Password = "Giogio99!",
    //         ConfirmPassword = "Giogio99!"
    //     };
    //
    //     _mockShoppingCartService.Setup(x => x.EmailSenderAsync(model.Email, It.IsAny<string>())).ReturnsAsync(true);
    //     
    //     //Act
    //     var result = await _accountService.AddUserAsync(model);
    //
    //     //Assert
    //     Assert.IsTrue(result);
    // }
}
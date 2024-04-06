﻿using FakeItEasy;
using FinalProject.Controllers;
using FinalProject.Interfaces;
using FinalProject.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalPoject.Test.Controller;

public class ShoppingCartControllerTest
{
    private IShoppingCartService _cartService;
    private ShoppingCartController _controller;

    public ShoppingCartControllerTest()
    {
        _cartService = A.Fake<IShoppingCartService>();

        _controller = new ShoppingCartController(_cartService);
    }

    [Test]
    public async Task ShoppingCartController_AddItem_ReturnRedirectToAction()
    {
        //Arrange
        var currentPage = 1;
        var numberOfItems = 4;
        var returnType = A.Fake<IEnumerable<CartProducts>>();
       
        A.CallTo(() => _cartService.GetPersonCartItemsAsync()).Returns(returnType);
        A.CallTo(() => _cartService.GetSelledProductPriceAsync());
        //Act

        var result = await _controller.Index(currentPage, numberOfItems);

        //Assert
        result.Should().BeOfType<ViewResult>();
        result.Should().NotBeNull();
    }
    
    [Test]
    [TestCase("Home")]
    [TestCase("Products")]
    [TestCase("SingleProduct")]
    [TestCase("")]
    public async Task ShoppingCartController_AddItem_ReturnRedirectToAction(string actionName)
    {
        //Arrange
        var id = 64;
        var sellQuantity = 19;

        A.CallTo(() => _cartService.AddProductsInCartAsync(id, sellQuantity)).Returns(Task.CompletedTask);

        //Act
        var result = await _controller.AddItem(id, sellQuantity, actionName);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<RedirectToActionResult>();
        var redirectToAction = result as RedirectToActionResult;
        switch (actionName)
        {
            case "Home":
                redirectToAction.ActionName.Should().Be(nameof(HomeController.Index));
                break;
            case "Products":
            case "SingleProduct":
                redirectToAction.ActionName.Should().Be(nameof(ProductController.Index));
                break;
        }

        redirectToAction.ActionName.Should().Be(nameof(HomeController.Index));
    }

    [Test]
    public async Task ShoppingCartController_Edit_Get_ReturnViewResult()
    {
        //Arrange
        var id = 79;
        var returnType = A.Fake<CartProducts>();
        A.CallTo(() => _cartService.GetItemByIdAsync(id)).Returns(returnType);

        //Act
        var result = await _controller.Edit(id);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public async Task ShoppingCartController_Edit_Post_ReturnRedirectToAction()
    {
        //Arrange
        var cartProducts = new CartProducts();
        var fromResult = Task.FromResult(true);
        A.CallTo(() => _cartService.UpdateEditedItemAsync(cartProducts)).Returns(fromResult);

        //Act
        var result = await _controller.Edit(cartProducts);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<RedirectToActionResult>();
        var redirectToAction = result as RedirectToActionResult;
        redirectToAction.ActionName.Should().Be(nameof(ShoppingCartController.Index));
    }

    [Test]
    public async Task ShoppingCartController_Edit_Post_ReturnViewResult()
    {
        //Arrange
        CartProducts cartProducts = null;
        var returnType = Task.FromResult(false);
        A.CallTo(() => _cartService.UpdateEditedItemAsync(cartProducts)).Returns(returnType);

        //Act
        var result = await _controller.Edit(cartProducts) as ViewResult;

        //Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public async Task ShoppingCartController_Delete_ReturnSuccess()
    {
        //Arrange
        var id = 79;
        var serviceResult = Task.FromResult(true);
        A.CallTo(() => _cartService.DeleteItemAsync(id)).Returns(serviceResult);

        //Act

        var result = await _controller.Delete(id);

        //Assert

        result.Should().BeOfType<RedirectToActionResult>();
        var redirectToAction = result as RedirectToActionResult;
        redirectToAction.ActionName.Should().Be(nameof(ShoppingCartController.Index));
    }

    
}
using FakeItEasy;
using FinalProject.Controllers;
using FinalProject.Interfaces;
using FinalProject.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace FinalPoject.Test.Controller;

public class SingleProductControllerTest
{
    private ISingleProductService _service;
    private SingleProductController _controller;
    
    public SingleProductControllerTest()
    {
        _service = A.Fake<ISingleProductService>();
        _controller = new SingleProductController(_service);
    }

    [Test]
    public void SingleProductController_Index_ReturnSuccess()
    {
        // Arrange
        var id = 0;
        var currentPage = 1;
        var NumberOfItems = 4;
        var fakeProductViewModel = A.Fake<ProductViewModel>();

        A.CallTo(() => _service.ProductCount()).Returns(10);
        A.CallTo(() => _service.MapViewModelToDto(id, currentPage, NumberOfItems)).Should().BeNull();

        // Act
        var result = _controller.Index(id, currentPage, NumberOfItems) as ViewResult;

        // Assert
        result.Should().BeOfType<ViewResult>();

        // Verify the model passed to the view
        result.Model.Should().BeNull();
    }
}
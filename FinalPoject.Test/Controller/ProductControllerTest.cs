using FakeItEasy;
using FinalProject.Controllers;
using FinalProject.Interfaces;
using FinalProject.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace FinalPoject.Test.Controller;

public class ProductControllerTest
{
    private readonly IProductService _productService;
    private readonly ProductController _productController;

    public ProductControllerTest()
    {
        _productService = A.Fake<IProductService>();

        //Controller
        _productController = new ProductController(_productService);
    }

    [Test]
    public void ProductController_Index_Get_ReturView()
    {
        //Arrange
        var products = A.Fake<IEnumerable<ProductViewModel>>();

        var currentPage = 1;
        var NumberOfItems = 6;

        A.CallTo(() =>
                _productService.FilterProductsByItsInput(currentPage, NumberOfItems, string.Empty, string.Empty))
            .Returns(products);
        //Act
        var result = _productController.Index(currentPage, NumberOfItems);
        //Assert

        result.Should().NotBeNull();
        result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void ProductController_Index_Post_ReturnsSuccess()
    {
        //Arrange
        var products = A.Fake<IEnumerable<ProductViewModel>>();

        var currentPage = 1;
        var NumberOfItems = 6;
        var actionForFilter = "High to Low";
        var filterInput = "";

        A.CallTo(() =>
                _productService.FilterProductsByItsInput(currentPage, NumberOfItems, actionForFilter, filterInput))
            .Returns(products);

        //Act

        var result = _productController.Index(currentPage, NumberOfItems, actionForFilter, filterInput);

        //Assert - Object Check Actions
        result.Should().NotBeNull();
    }
}
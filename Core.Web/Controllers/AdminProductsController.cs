﻿using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

[Authorize(Roles = "Admin")]
public class AdminProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly IShoppingService _shoppingService;
    private readonly IAdminProductsService _productsService;

    public AdminProductsController(IProductService productService, IShoppingService shoppingService, IAdminProductsService productsService)
    {
        _productService = productService;
        _shoppingService = shoppingService;
        _productsService = productsService;
    }
    public IActionResult Products(int currentPage = 1, int NumberOfItems = 6)
    {
        var count = _productService.ProductCount();
        var filteredProducts =
            _productsService.FilterProductsByItsInput(currentPage, NumberOfItems, string.Empty, string.Empty);

        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)NumberOfItems);

        return View(filteredProducts);
    }

    [HttpPost]
    public IActionResult Products(int currentPage = 1, int NumberOfItems = 6, string actionForFilter = "",
        string filterInput = "")
    {
        var count = _productService.ProductCount();
        var filteredProducts =
            _productsService.FilterProductsByItsInput(currentPage, NumberOfItems, actionForFilter, filterInput);

        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)NumberOfItems);

        return View(filteredProducts);
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var item = _productsService.GetProductById(id);
        return View(item);
    }

    public IActionResult Update(Product model,IFormFile file)
    {
        _productsService.UpdateEditedItem(model,file);
        return RedirectToAction(nameof(Products));
    }

    public async Task<IActionResult> Delete(int id,int currentPage)
    {
        _productsService.DeleteItem(id);
        return RedirectToAction(nameof(Products),new {currentPage = currentPage});
    }

    [HttpGet]
    public IActionResult Add(Product model)
    {
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(Product model,IFormFile file)
    {
        if (file != null)
        {
            await _productsService.AddItem(model, file);
        }
        return RedirectToAction(nameof(Products));
    }
}
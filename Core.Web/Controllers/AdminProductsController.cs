using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

[Authorize(Roles = "Admin")]
public class AdminProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IAdminProductsService _productsService;

    public AdminProductsController(IProductService productService, IShoppingCartService shoppingCartService, IAdminProductsService productsService)
    {
        _productService = productService;
        _shoppingCartService = shoppingCartService;
        _productsService = productsService;
    }
    [HttpGet]
    public IActionResult Products(int currentPage = 1, int numberOfItems = 6)
    {
        var count = _productService.ProductCount();
        var filteredProducts =
            _productsService.FilterProductsByItsInput(currentPage, numberOfItems, string.Empty, string.Empty);

        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)numberOfItems);

        return View(filteredProducts);
    }

    [HttpPost]
    public IActionResult Products(int currentPage = 1, int numberOfItems = 6, string actionForFilter = "",
        string filterInput = "")
    {
        var count = _productService.ProductCount(actionForFilter,filterInput);
        var filteredProducts =
            _productsService.FilterProductsByItsInput(currentPage, numberOfItems, actionForFilter, filterInput);

        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)numberOfItems);

        ViewData["ActionFilter"] = actionForFilter;
        ViewData["FilterInput"] = filterInput;
        
        return View(filteredProducts);
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var item = _productsService.GetProductById(id);
        return View(item);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Product model,IFormFile file)
    {
        await _productsService.UpdateEditedItemAsync(model,file);
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
            await _productsService.AddItemAsync(model, file);
        }
        return RedirectToAction(nameof(Products));
    }
}
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

public class AdminController : Controller
{
    private readonly IProductService _productService;
    private readonly IShoppingService _shoppingService;
    private readonly IAdminService _service;

    public AdminController(IProductService productService, IShoppingService shoppingService, IAdminService service)
    {
        _productService = productService;
        _shoppingService = shoppingService;
        _service = service;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Products(int currentPage = 1, int NumberOfItems = 6)
    {
        var count = _productService.ProductCount();
        var filteredProducts =
            _service.FilterProductsByItsInput(currentPage, NumberOfItems, string.Empty, string.Empty);

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
            _service.FilterProductsByItsInput(currentPage, NumberOfItems, actionForFilter, filterInput);

        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)NumberOfItems);

        return View(filteredProducts);
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var item = _service.GetProductById(id);
        return View(item);
    }

    public IActionResult Update(Product model,IFormFile file)
    {
        _service.UpdateEditedItem(model,file);
        return RedirectToAction(nameof(Products));
    }

    public IActionResult Delete(int id,int currentPage)
    {
        _service.DeleteItem(id);
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
            await _service.AddItem(model, file);
        }
        return RedirectToAction(nameof(Products));
    }
}
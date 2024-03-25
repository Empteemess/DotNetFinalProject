using System.Collections;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Index(int currentPage = 1, int NumberOfItems = 6)
    {
        var count = _service.ProductCount();
        var filteredProducts = _service.FilterProductsByItsInput(currentPage, NumberOfItems,string.Empty,string.Empty);

        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)NumberOfItems);

        return View(filteredProducts);
    }

    [HttpPost]
    public IActionResult Index(int currentPage = 1, int NumberOfItems = 6, string actionForFilter = "",string filterInput = "")
    {
        var count = _service.ProductCount();
        var filteredProducts =
            _service.FilterProductsByItsInput(currentPage, NumberOfItems, actionForFilter, filterInput);

        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)NumberOfItems);

        return View(filteredProducts);
    }
}
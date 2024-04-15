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
    public IActionResult Index(int currentPage = 1, int numberOfItems = 6)
    {
        var check = _service.CheckPageNum(currentPage, numberOfItems);
        if (!check)
        {
            return RedirectToAction("Index", "Error");
        }

        var count = _service.ProductCount();
        var filteredProducts = _service.GetProductsByItsInput(currentPage, numberOfItems, string.Empty, string.Empty);

        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)numberOfItems);

        return View(filteredProducts);
    }

    [HttpPost]
    public IActionResult Index(int currentPage = 1, string actionForFilter = "", string filterInput = "")
    {
        var numberOfItems = 6;
        var count = _service.ProductCount(actionForFilter,filterInput);
    
        var filteredProducts = _service.GetProductsByItsInput(currentPage, numberOfItems, actionForFilter, filterInput);

        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)numberOfItems);

        ViewData["ActionFilter"] = actionForFilter;
        ViewData["FilterInput"] = filterInput;
        

        return View(filteredProducts);
    }
}
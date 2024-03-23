using System.Diagnostics;
using FinalProject.Data;
using FinalProject.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeService _service;

    public HomeController(ILogger<HomeController> logger,IHomeService service)
    {
        _logger = logger;
        _service = service;
    }

    public IActionResult Index(int currentPage = 1,int NumberOfItems = 4)
    {
        var count = _service.ProductCount();
        var ProductsByNumber = _service.MapViewModelDataToDto(currentPage,NumberOfItems);
        
        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)NumberOfItems);
        
        return View(ProductsByNumber);
    }
}
using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

public class SingleProductController : Controller
{
    private readonly ISingleProductService _service;

    public SingleProductController(ISingleProductService service)
    {
        _service = service;
    }
    public IActionResult Index(int id,int currentPage = 1,int NumberOfItems = 4)
    {
        var check = _service.CheckProduct(id);
        if (!check)
        {
            return RedirectToAction("Index", "Error");
        }
        var count = _service.ProductCount();
        
        var ProductsByNumber = _service.MapViewModelToDto(id,currentPage,NumberOfItems);
        
        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)NumberOfItems);
     
        return View(ProductsByNumber);
    }
}
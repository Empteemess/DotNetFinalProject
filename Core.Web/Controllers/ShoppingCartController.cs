using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

[Authorize(Roles = "User")]
public class ShoppingCartController : Controller
{
    private readonly IShoppingService _service;

    public ShoppingCartController(IShoppingService service)
    {
        _service = service;
    }

    // GET
    public async Task<IActionResult> Index(int currentPage = 1, int numberOfItems = 4)
    {
        var items = await _service.GetPersonCartItemsAsync();
        var count = items.Count();
        var exactProducts = items.Skip((currentPage - 1) * numberOfItems).Take(numberOfItems).ToList();

        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)numberOfItems);
        ViewBag.WholeSelledProductPrice = await _service.GetSelledProductPriceAsync();
        return View(exactProducts);
    }

    public async Task<IActionResult> AddItem(int id, int sellQuantity = 1, string actionName = "")
    {
        await _service.AddProductsInCartAsync(id, sellQuantity);
        switch (actionName)
        {
            case "Home":
                return RedirectToAction("Index", "Home");
                break;
            case "Products":
                return RedirectToAction("Index", "Product");
                break;
            case"SingleProduct":
                return RedirectToAction("Index", "Product");
                
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _service.GetItemByIdAsync(id);
        return View(item);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CartProducts model)
    {
        var item = await _service.UpdateEditedItemAsync(model);
        if (item)
        {
            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteItemAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Buy()
    {
        await _service.BuyItemAsync();
        return RedirectToAction("Index","Home");
    }
}
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

[Authorize(Roles = "User")]
public class ShoppingCartController : Controller
{
    private readonly IShoppingCartService _cartService;

    public ShoppingCartController(IShoppingCartService cartService)
    {
        _cartService = cartService;
    }


    [HttpGet]
    public async Task<IActionResult> Index(int currentPage = 1, int numberOfItems = 4, bool buyButtonCheck = false)
    {
        var items = await _cartService.GetPersonCartItemsAsync();
        var count = items.Count();
        var exactProducts = items.Skip((currentPage - 1) * numberOfItems).Take(numberOfItems).ToList();

        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)numberOfItems);
        ViewBag.WholeSelledProductPrice = await _cartService.GetSelledProductPriceAsync();
        ViewBag.BuyButtonCheck = buyButtonCheck;

        var returnValue = _cartService.MapCartProductsToCreditCadViewModel(exactProducts);

        return View(returnValue);
    }

    [HttpPost]
    public async Task<IActionResult> Index(CreditCartViewModel model)
    {
        if (model.CartProducts is null)
        {
            await _cartService.BuyItemAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> AddItem(int id, int sellQuantity = 1, string actionName = "")
    {
        await _cartService.AddProductsInCartAsync(id, sellQuantity);

        await _cartService.RemoveProductQuantity(id, sellQuantity);

        switch (actionName)
        {
            case "Home":
                return RedirectToAction("Index", "Home");
                break;
            case "Products":
                return RedirectToAction("Index", "Product");
                break;
            case "SingleProduct":
                return RedirectToAction("Index", "Product");
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item = await _cartService.GetItemByIdAsync(id);
        return View(item);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CartProducts model)
    {
        var item = await _cartService.UpdateEditedItemAsync(model);
        if (item)
        {
            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _cartService.DeleteItemAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
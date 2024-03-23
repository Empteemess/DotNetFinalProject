using System.Security.Claims;
using FinalProject.Configurations;
using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Services;

public class ShoppingCartService : IShoppingService
{
    private readonly DependencyConfiguration _dependencyConfiguration;

    public ShoppingCartService(DependencyConfiguration dependencyConfiguration)
    {
        _dependencyConfiguration = dependencyConfiguration;
    }

    private async Task<ApplicationUser> GetCurrentUser()
    {
        var personId = _dependencyConfiguration._httpContextAccessor.HttpContext.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var person = await _dependencyConfiguration._userManager.Users.FirstOrDefaultAsync(x => x.Id == personId);
        return person;
    }

    public async Task<IEnumerable<CartProducts>> GetPersonCartItemsAsync()
    {
        var person = await GetCurrentUser();
        if (person != null)
        {
            return person.CartProducts;
        }

        return null;
    }


    public async Task AddProductsInCartAsync(int id, int sellQuantity)
    {
        var exactItem = await _dependencyConfiguration._context.Products.FirstOrDefaultAsync(x => x.Id == id);

        var product = new CartProducts()
        {
            ProductId = exactItem.Id,
            Name = exactItem.Name,
            Description = exactItem.Description,
            Image = exactItem.Image,
            Price = exactItem.Price,
            Quantity = exactItem.Quantity,
            SellQuantity = sellQuantity,
        };

        var user = await GetCurrentUser();

        if (exactItem.Quantity < sellQuantity && user.CartProducts.Any(x => x.ProductId == exactItem.Id))
        {
            var existedProduxt = user.CartProducts.FirstOrDefault(x => x.ProductId == id);
            existedProduxt.SellQuantity += exactItem.Quantity;
            await _dependencyConfiguration._userManager.UpdateAsync(user);
        }
        else if (exactItem.Quantity < sellQuantity && !user.CartProducts.Any(x => x.ProductId == exactItem.Id))
        {
            product.SellQuantity = exactItem.Quantity;
            user.CartProducts.Add(product);
            await _dependencyConfiguration._userManager.UpdateAsync(user);
        }
        else if (!user.CartProducts.Any(x => x.ProductId == exactItem.Id))
        {
            product.SellQuantity = sellQuantity;
            user.CartProducts.Add(product);
            await _dependencyConfiguration._userManager.UpdateAsync(user);
        }
        else
        {
            var existedProduxt = user.CartProducts.FirstOrDefault(x => x.ProductId == id);
            existedProduxt.SellQuantity += sellQuantity;
            await _dependencyConfiguration._userManager.UpdateAsync(user);
        }
    }

    public async Task<double> GetSelledProductPrice()
    {
        var wholePrice = 0.0;
        var user = await GetCurrentUser();
        foreach (var item in user.CartProducts)
        {
            wholePrice += item.SellQuantity * item.Price;
        }

        return wholePrice;
    }


    public async Task<CartProducts> GetItemById(int id)
    {
        var user = await GetCurrentUser();

        var exactItem = user.CartProducts.FirstOrDefault(x => x.Id == id);

        return exactItem;
    }

    public async Task<bool> UpdateEditedItem(CartProducts model)
    {
        var user = await GetCurrentUser();
        var exactItem = user.CartProducts.FirstOrDefault(x => x.Id == model.Id);
        if (exactItem != null)
        {
            exactItem.SellQuantity = model.SellQuantity;
            await _dependencyConfiguration._userManager.UpdateAsync(user);
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteItem(int id)
    {
        var user = await GetCurrentUser();
        var item = user.CartProducts.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            user.CartProducts.Remove(item);
            await _dependencyConfiguration._userManager.UpdateAsync(user);
            return true;
        }

        return false;
    }
}
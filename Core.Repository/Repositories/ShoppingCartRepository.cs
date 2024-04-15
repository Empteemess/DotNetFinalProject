using System.Security.Claims;
using FinalProject.Configurations;
using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DependencyConfiguration _dependencyConfiguration;

    public ShoppingCartRepository(AppDbContext context,IHttpContextAccessor httpContextAccessor,DependencyConfiguration dependencyConfiguration)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _dependencyConfiguration = dependencyConfiguration;
    }
    public async Task RemoveProductQuantity(int id,int sellQuantity)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        product.Quantity -= sellQuantity;
        _context.SaveChanges();
    }
    
    public async Task AddProductQuantity(int id,int sellQuantity)
    {
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        product.Quantity += sellQuantity;
        _context.SaveChanges();
    }

    public async Task<ApplicationUser> GetCurrentUserAsync()
    {
        var personId = _httpContextAccessor.HttpContext.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var person = await _dependencyConfiguration._userManager.Users.FirstOrDefaultAsync(x => x.Id == personId);
        return person;
    }

    public async Task AddProductsInCartAsync(int id, int sellQuantity)
    {
        var exactItem = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

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

        var user = await GetCurrentUserAsync();

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
}
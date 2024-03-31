using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly AppDbContext _context;

    public ShoppingCartRepository(AppDbContext context)
    {
        _context = context;
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
}
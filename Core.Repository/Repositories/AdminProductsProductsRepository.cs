using FinalProject.Configurations;
using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositories;

public class AdminProductsProductsRepository : IAdminProductsRepository<Product>
{
    private readonly AppDbContext _context;

    public AdminProductsProductsRepository(AppDbContext context)
    {
        _context = context;
    }

    public Product GetProductById(int id)
    {
        return _context.Products.FirstOrDefault(x => x.Id == id);
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _context.Products;
    }
    public void Add(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void Delete(Product product)
    {
        _context.Products.Remove(product);
        _context.SaveChanges();
    }


    public void Update(Product product)
    {
        _context.Products.Update(product);
        _context.SaveChanges();
    }
}
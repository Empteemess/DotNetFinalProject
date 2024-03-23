using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;

namespace FinalProject.Repositories;

public class SingleProductRepository : ISingleProductRepository<Product>
{
    private readonly AppDbContext _context;

    public SingleProductRepository(AppDbContext context)
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
    }

    public void Delete(Product product)
    {
        _context.Remove(product);
    }

    public void Update(Product product)
    {
        _context.Update(product);
    }
}
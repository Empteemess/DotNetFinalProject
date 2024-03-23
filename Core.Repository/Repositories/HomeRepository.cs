using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;

namespace FinalProject.Repositories;

public class HomeRepository : IHomeRepository<Product>
{
    private readonly AppDbContext _context;

    public HomeRepository(AppDbContext context)
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
        try
        {
            _context.Remove(product);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void Update(Product product)
    {
        _context.Update(product);
    }
}
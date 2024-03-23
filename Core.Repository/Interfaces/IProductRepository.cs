using FinalProject.Models;

namespace FinalProject.Interfaces;

public interface IProductRepository<T> : IBaseRepository<T> where T : class
{
    IEnumerable<Product> GetProductByName(string productName);
}
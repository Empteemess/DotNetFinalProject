using FinalProject.Interfaces;
using FinalProject.Models;

namespace FinalProject.Services;

public class HomeService : IHomeService
{
    private readonly IHomeRepository<Product> _repository;

    public HomeService(IHomeRepository<Product> repository)
    {
        _repository = repository;
    }

    public IEnumerable<Product> DivideDataForPaging(int currentPage, int numberOfItems)
    {
        var products = _repository.GetAllProducts();
        var exactProducts = products.Skip((currentPage - 1) * numberOfItems).Take(numberOfItems).ToList();
        return exactProducts;
    }

    public bool CheckPageNum(int currentPage,int numberOfItems)
    {
        var count = ProductCount();
        var maxPageNum = (int)Math.Ceiling(count / (double)numberOfItems);
        if (currentPage > maxPageNum)
        {
            return false;
        }

        return true;
    }

    public Product ExactProduct(int id)
    {
        return _repository.GetProductById(id);
    }

    public int ProductCount()
    {
        return _repository.GetAllProducts().Count();
    }
}
using FinalProject.Configurations;
using FinalProject.Interfaces;
using FinalProject.Models;

namespace Core.Services.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository<Product> _repository;

    public ProductService(IProductRepository<Product> repository)
    {
        _repository = repository;
    }

    public IEnumerable<ProductViewModel> GetProductsByItsInput(int currentPage, int numberOfItems,
        string actionName, string filterInput)
    {
        var products = _repository.GetAllProducts().Select(x => new ProductViewModel()
        {
            Id = x.Id,
            Name = x.Name,
            Image = x.Image,
            Price = x.Price,
            Description = x.Description,
            Quantity = x.Quantity,
            CategoryEnum = x.CategoryEnum
        });

        switch (actionName)
        {
            case "High to Low":
                products = products.OrderByDescending(x => x.Price);
                break;
            case "Low to Hight":
                products = products.OrderBy(x => x.Price);
                break;
            case "Search":
                products = products.Where(x => x.Name.ToLower().Contains(filterInput));
                break;
            case "Male":
                products = products.Where(x => (int)x.CategoryEnum == 2);
                break;
            case "Female":
                products = products.Where(x => (int)x.CategoryEnum == 1);
                break;
            default:
                products = products;
                break;
        }

        var exactProducts = products.Skip((currentPage - 1) * numberOfItems).Take(numberOfItems);
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
    public int ProductCount()
    {
        return _repository.GetAllProducts().Count();
    }
}
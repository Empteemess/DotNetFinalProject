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

    public IEnumerable<ProductViewModel> FilterProductsByItsInput(int currentPage, int NumberOfItems,
        string actionForFilter, string filterInput)
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

        switch (actionForFilter)
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
            default:
                products = products;
                break;
        }

        var exactProducts = products.Skip((currentPage - 1) * NumberOfItems).Take(NumberOfItems);
        return exactProducts;
    }

    public int ProductCount()
    {
        return _repository.GetAllProducts().Count();
    }
}
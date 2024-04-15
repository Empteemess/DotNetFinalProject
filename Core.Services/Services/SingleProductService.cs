using FinalProject.Interfaces;
using FinalProject.Models;

namespace FinalProject.Services;

public class SingleProductService : ISingleProductService
{
    private readonly ISingleProductRepository<Product> _repository;

    public SingleProductService(ISingleProductRepository<Product> repository)
    {
        _repository = repository;
    }

    public ProductViewModel MapViewModelToDto(int id, int currentPage, int numberOfItems)
    {
        var exactProduct = _repository.GetProductById(id);

        var count = ProductCount();
        
        var products = _repository.GetAllProducts();
        var exactProducts = products.Skip((currentPage - 1) * numberOfItems).Take(numberOfItems).ToList();

        var productDto = new ProductViewModel()
        {
            Id = exactProduct.Id,
            Name = exactProduct.Name,
            Image = exactProduct.Image,
            Price = exactProduct.Price,
            Description = exactProduct.Description,
            Quantity = exactProduct.Quantity,
            CategoryEnum = exactProduct.CategoryEnum,
            Product = exactProducts,
        };

        return productDto;
    }

    public int ProductCount()
    {
        return _repository.GetAllProducts().Count();
    }

    public bool CheckProduct(int id)
    {
        var product = _repository.GetProductById(id);

        if (product != null)
        {
            return true;
        }

        return false;
    }
}
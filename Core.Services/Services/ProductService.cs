using FinalProject;
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
            case nameof(ActionNamesEnum.HightoLow):
                products = products.OrderByDescending(x => x.Price);
                break;
            case nameof(ActionNamesEnum.LowtoHight):
                products = products.OrderBy(x => x.Price);
                break;
            case nameof(ActionNamesEnum.Search):
                products = products.Where(x => x.Name.ToLower().Contains(filterInput));
                break;
            case nameof(ActionNamesEnum.Male):
                products = products.Where(x => (int)x.CategoryEnum == 2);
                break;
            case nameof(ActionNamesEnum.Female):
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
    public int ProductCount(string actionName,string filterName = "")
    {
        var count = 0;
        switch (actionName)
        {
            case nameof(ActionNamesEnum.HightoLow):
            case nameof(ActionNamesEnum.LowtoHight):
                count = _repository.GetAllProducts().Count();
                break;
            case nameof(ActionNamesEnum.Search):
                count = _repository.GetAllProducts().Where(x => x.Name.Contains(filterName)).Count();
                break;
            case nameof(ActionNamesEnum.Male):
                count = _repository.GetAllProducts().Where(x => x.CategoryEnum == CategoryEnum.Men).Count();
                break;
            case nameof(ActionNamesEnum.Female):
                count = _repository.GetAllProducts().Where(x => x.CategoryEnum == CategoryEnum.Women).Count();
                break;
            default:
                count = _repository.GetAllProducts().Count();
                break;
            
        }

        return count;
    }
    
}
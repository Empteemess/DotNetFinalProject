using System.Security.Claims;
using FinalProject.Configurations;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Services;

public class AdminProductsService : IAdminProductsService
{
    private readonly IAdminProductsRepository<Product> _productsRepository;
    private readonly IWebHostEnvironment _environment;
    private readonly DependencyConfiguration _dependencyConfiguration;

    public AdminProductsService(IAdminProductsRepository<Product> productsRepository, IWebHostEnvironment environment,
        DependencyConfiguration dependencyConfiguration)
    {
        _productsRepository = productsRepository;
        _environment = environment;
        _dependencyConfiguration = dependencyConfiguration;
    }

    public Product GetProductById(int id)
    {
        return _productsRepository.GetProductById(id);
    }
   
    public async Task UpdateEditedItemAsync(Product model, IFormFile file)
    {
        var item = _productsRepository.GetProductById(model.Id);

        item.Name = model.Name;
        item.Price = model.Price;
        item.Quantity = model.Quantity;
        item.Description = model.Description;
        item.CategoryEnum = model.CategoryEnum;

        if (file != null)
        {
            var imagePath = Path.Combine(_environment.WebRootPath, "uploadImages", Path.GetFileName(item.Image));
            File.Delete(imagePath);

            var filePath = await UploadFileAsync(file);

            item.Image = $"~/{filePath}";
            _productsRepository.Update(item);
        }
        else
        {
            _productsRepository.Update(item);
        }
    }


    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var uploadPath = Path.Combine("uploadImages");

        var filePath = Path.Combine(uploadPath, fileName);

        var absolutePath = Path.Combine(_environment.WebRootPath, filePath);

        using (var stream = new FileStream(absolutePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filePath;
    }

    public async Task AddItemAsync(Product model, IFormFile file)
    {
        var path = await UploadFileAsync(file);
        model.Image = $@"~/{path}";
        _productsRepository.Add(model);
    }


    public IEnumerable<Product> FilterProductsByItsInput(int currentPage, int NumberOfItems,
        string actionForFilter, string filterInput)
    {
        var products = _productsRepository.GetAllProducts();
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

    public bool DeleteItem(int id)
    {
        var item = _productsRepository.GetAllProducts().FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            var imagePath = Path.Combine(_environment.WebRootPath, "uploadImages", Path.GetFileName(item.Image));

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

            _productsRepository.Delete(item);
            return true;
        }

        return false;
    }
}
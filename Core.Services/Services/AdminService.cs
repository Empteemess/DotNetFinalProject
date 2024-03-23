using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Core.Services.Services;

public class AdminService : IAdminService
{
    private readonly IAdminRepository<Product> _repository;
    private readonly IWebHostEnvironment _environment;

    public AdminService(IAdminRepository<Product> repository, IWebHostEnvironment environment)
    {
        _repository = repository;
        _environment = environment;
    }

    public Product GetProductById(int id)
    {
        return _repository.GetProductById(id);
    }

    public async Task UpdateEditedItem(Product model, IFormFile file)
    {
        //Remove
        var item = _repository.GetProductById(model.Id);
        var imagePath = Path.Combine(_environment.WebRootPath, "uploadImages", Path.GetFileName(item.Image));
        File.Delete(imagePath);

        //Add Photo
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var uploadPath = Path.Combine("uploadImages");
        var filePath = Path.Combine(uploadPath, fileName);
        var absolutePath = Path.Combine(_environment.WebRootPath, filePath);
        
        using (var stream = new FileStream(absolutePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        item.Image = $"~/{filePath}";
        //Update
        _repository.Update(item);
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

    public async Task AddItem(Product model, IFormFile file)
    {
        var path = await UploadFileAsync(file);
        model.Image = $@"~/{path}";
        _repository.Add(model);
    }


    public IEnumerable<Product> FilterProductsByItsInput(int currentPage, int NumberOfItems,
        string actionForFilter, string filterInput)
    {
        var products = _repository.GetAllProducts();
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
        var item = _repository.GetAllProducts().FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            var imagePath = Path.Combine(_environment.WebRootPath, "uploadImages", Path.GetFileName(item.Image));

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

            _repository.Delete(item);
            return true;
        }

        return false;
    }
}
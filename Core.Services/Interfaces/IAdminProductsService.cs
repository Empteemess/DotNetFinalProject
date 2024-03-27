using FinalProject.Models;
using Microsoft.AspNetCore.Http;

namespace FinalProject.Interfaces;

public interface IAdminProductsService
{
    Product GetProductById(int id);
    Task UpdateEditedItemAsync(Product model, IFormFile file);
    Task<string> UploadFileAsync(IFormFile file);
    Task AddItemAsync(Product model, IFormFile file);
    bool DeleteItem(int id);
    IEnumerable<Product> FilterProductsByItsInput(int currentPage, int NumberOfItems,
        string actionForFilter, string filterInput);
}
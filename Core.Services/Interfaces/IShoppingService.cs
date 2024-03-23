using FinalProject.Data;
using FinalProject.Models;

namespace FinalProject.Interfaces;

public interface IShoppingService
{
    Task<IEnumerable<CartProducts>> GetPersonCartItemsAsync();
    Task AddProductsInCartAsync(int id, int sellQuantity);
    Task<double> GetSelledProductPrice();
    Task<CartProducts> GetItemById(int id);
    Task<bool> UpdateEditedItem(CartProducts model);
    Task<bool> DeleteItem(int id);
}
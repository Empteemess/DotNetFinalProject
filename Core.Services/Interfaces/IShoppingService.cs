using FinalProject.Data;
using FinalProject.Models;

namespace FinalProject.Interfaces;

public interface IShoppingService
{
    Task<IEnumerable<CartProducts>> GetPersonCartItemsAsync();
    Task AddProductsInCartAsync(int id, int sellQuantity);
    Task<double> GetSelledProductPriceAsync();
    Task<CartProducts> GetItemByIdAsync(int id);
    Task<bool> UpdateEditedItemAsync(CartProducts model);
    Task<bool> DeleteItemAsync(int id);
    Task BuyItemAsync();
    Task<ApplicationUser> GetCurrentUserAsync();
}
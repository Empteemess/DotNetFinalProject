using FinalProject.Data;
using FinalProject.Models;

namespace FinalProject.Interfaces;

public interface IShoppingCartService
{
    Task<IEnumerable<CartProducts>> GetPersonCartItemsAsync();
    Task<double> GetSelledProductPriceAsync();
    Task<CartProducts> MapItemsQuantities(int id);
    Task<bool> UpdateEditedItemAsync(CartProducts model);
    Task<bool> DeleteItemAsync(int id);
    Task BuyItemAsync();
    Task RemoveProductQuantity(int id,int sellQuantity);
    Task<bool> EmailSenderAsync();
    Task<bool> EmailSenderAsync(string userEmail, string userText);
    Task AddProductsInCartAsync(int id, int sellQuantity);
    CreditCartViewModel MapCartProductsToCreditCadViewModel(IEnumerable<CartProducts> cartProducts);
}
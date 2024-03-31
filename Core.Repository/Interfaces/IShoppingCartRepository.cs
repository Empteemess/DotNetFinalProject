using FinalProject.Models;

namespace FinalProject.Interfaces;

public interface IShoppingCartRepository
{
    Task RemoveProductQuantity(int id, int sellQuantity);
    Task AddProductQuantity(int id,int sellQuantity);
}
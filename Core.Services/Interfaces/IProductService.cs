using FinalProject.Configurations;
using FinalProject.Models;

namespace FinalProject.Interfaces;

public interface IProductService
{
    IEnumerable<ProductViewModel> GetProductsByItsInput(int currentPage, int NumberOfItems, string actionName,string filterInput);
    int ProductCount();
    bool CheckPageNum(int currentPage, int NumberOfItems);
}
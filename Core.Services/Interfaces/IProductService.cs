using FinalProject.Configurations;
using FinalProject.Models;

namespace FinalProject.Interfaces;

public interface IProductService
{
    IEnumerable<ProductViewModel> GetProductsByItsInput(int currentPage, int NumberOfItems, string actionName,string filterInput);
    int ProductCount();
    int ProductCount(string actionName, string filterName = "");
    bool CheckPageNum(int currentPage, int NumberOfItems);
}
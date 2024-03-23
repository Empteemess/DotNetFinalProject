using FinalProject.Configurations;
using FinalProject.Models;

namespace FinalProject.Interfaces;

public interface IProductService
{
    IEnumerable<ProductViewModel> FilterProductsByItsInput(int currentPage, int NumberOfItems, string actionForFilter,string filterInput);
    int ProductCount();
}
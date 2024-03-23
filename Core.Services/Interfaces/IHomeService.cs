using FinalProject.Models;

namespace FinalProject.Interfaces;

public interface IHomeService
{
    IEnumerable<Product> MapViewModelDataToDto(int currentPage,int NumberOfItems);
    Product ExactProduct(int id);
    int ProductCount();
    //list - ienumerable
}
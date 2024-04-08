using FinalProject.Models;

namespace FinalProject.Interfaces;

public interface IHomeService
{
    IEnumerable<Product> DivideDataForPaging(int currentPage,int NumberOfItems);
    Product ExactProduct(int id);
    int ProductCount();
    bool CheckPageNum(int currentPage, int NumberOfItems);
}
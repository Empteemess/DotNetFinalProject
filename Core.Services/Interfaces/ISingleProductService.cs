using FinalProject.Models;

namespace FinalProject.Interfaces;

public interface ISingleProductService
{
    ProductViewModel MapViewModelToDto(int id,int currentPage,int NumberOfItems);
    public int ProductCount();
    bool CheckProduct(int id);
}
using FinalProject.Models;

namespace FinalProject.Interfaces;

public interface IAdminProductsRepository<T> : IBaseRepository<T> where T : class
{
  
}
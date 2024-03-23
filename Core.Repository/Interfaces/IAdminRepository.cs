using FinalProject.Models;

namespace FinalProject.Interfaces;

public interface IAdminRepository<T> : IBaseRepository<T> where T : class
{
  
}
namespace FinalProject.Interfaces;

public interface IBaseRepository<T> where T : class
{
    T GetProductById(int id);
    IEnumerable<T> GetAllProducts();
    void Add(T product);
    void Delete(T product);
    void Update(T product);
}
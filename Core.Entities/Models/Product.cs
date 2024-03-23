
namespace FinalProject.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public CategoryEnum CategoryEnum { get; set; }
    
}
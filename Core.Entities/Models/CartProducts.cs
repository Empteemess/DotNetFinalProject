using FinalProject.Data;

namespace FinalProject.Models;

public class CartProducts
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public int SellQuantity { get; set; }
    public double WholeSelledProductPrice { get; set; }

    public string ApplicationUserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
}
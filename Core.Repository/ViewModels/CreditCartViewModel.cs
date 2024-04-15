using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FinalProject.Models;

public class CreditCartViewModel
{
    public string Name { get; set; }
    public int CardNumber { get; set; }
    public string DateTime { get; set; }
    public int SecurityCode { get; set; }
    public IEnumerable<CartProducts>? CartProducts { get; set; }
}
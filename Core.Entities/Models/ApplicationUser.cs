using FinalProject.Models;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Data;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<CartProducts> CartProducts { get; set; }
}
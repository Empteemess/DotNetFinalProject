using Microsoft.AspNetCore.Identity;

namespace FinalProject.Models;

public class UserViewModel
{
    public string? Email { get; set; }
    public string? Role { get; set; }
    public string AddRoleName { get; set; }
    public string UpdateRoleName { get; set; }
    public IEnumerable<IdentityRole>? IdentityRole { get; set; }
}
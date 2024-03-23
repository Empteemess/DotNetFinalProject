using FinalProject.Data;
using FinalProject.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Configurations;

public class DependencyConfiguration
{
    public readonly UserManager<ApplicationUser> _userManager;
    public readonly SignInManager<ApplicationUser> _signInManager;
    public readonly RoleManager<IdentityRole> _roleManager;
    public readonly IHttpContextAccessor _httpContextAccessor;
    public readonly AppDbContext _context;

    public DependencyConfiguration(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IHttpContextAccessor httpContextAccessor,
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }
}
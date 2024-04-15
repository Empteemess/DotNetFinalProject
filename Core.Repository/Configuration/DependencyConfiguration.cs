using FinalProject.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Configurations;

public class DependencyConfiguration
{
    public virtual UserManager<ApplicationUser> _userManager { get; set; }
    public virtual SignInManager<ApplicationUser> _signInManager { get; set; }
    public virtual RoleManager<IdentityRole> _roleManager { get; set; }

    public DependencyConfiguration()
    {
    }

    public DependencyConfiguration(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }
}
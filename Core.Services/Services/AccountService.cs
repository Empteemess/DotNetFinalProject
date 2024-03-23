using System.Net;
using System.Security.Claims;
using FinalProject.Configurations;
using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositories;

public class AccountService : IAccountService
{
    private readonly DependencyConfiguration _dependencyConfiguration;

    public AccountService(DependencyConfiguration dependencyConfiguration)
    {
        _dependencyConfiguration = dependencyConfiguration;
    }

    public async Task<bool> RegisterAsync(RegisterViewModel model)
    {
        var result = await AddUserAsync(model);
        return result;
    }

    public async Task<bool> LoginAsync(LoginViewModel model)
    {
        var result = await _dependencyConfiguration._signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        if (result.Succeeded)
        {
            return true;
        }

        return false;
    }

    public async Task LogOutAsync()
    {
        _dependencyConfiguration._signInManager.SignOutAsync();
    }
    
      public async Task CreateRoleAsync(string roleName)
    {
        await _dependencyConfiguration._roleManager.CreateAsync(new IdentityRole() { Name = roleName });
    }
      
    public async Task RemoveRoleAsync(RoleViewModel model)
    {
        var role = await _dependencyConfiguration._roleManager.FindByNameAsync(model.Role);
        if (role != null)
        {
            var result = await _dependencyConfiguration._roleManager.DeleteAsync(role);
        }
    }

    public async Task<bool> AddUserAsync(RegisterViewModel model)
    {
        var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
        var result = await _dependencyConfiguration._userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _dependencyConfiguration._userManager.AddToRoleAsync(user, "User");
            return true;
        }

        return false;
    }

    public async Task RemoveUserAsync(RegisterViewModel model)
    {
        var user = await _dependencyConfiguration._userManager.FindByEmailAsync(model.Email);
        await _dependencyConfiguration._userManager.DeleteAsync(user);
    }
}
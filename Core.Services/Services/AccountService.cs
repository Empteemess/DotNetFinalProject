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
    private readonly IShoppingCartService _shoppingCartService;

    public AccountService(DependencyConfiguration dependencyConfiguration,IShoppingCartService shoppingCartService)
    {
        _dependencyConfiguration = dependencyConfiguration;
        _shoppingCartService = shoppingCartService;
    }

    public async Task<bool> RegisterAsync(RegisterViewModel model)
    {
        var text = "Thank you for registering. Your account is now active.";
        await _shoppingCartService.EmailSenderAsync(model.Email,text);
        var result = await AddUserAsync(model);
        return result;
    }

    public async Task<bool> LoginAsync(LoginViewModel model)
    {
        var result =
            await _dependencyConfiguration._signInManager.PasswordSignInAsync(model.Email, model.Password, false,
                false);
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

    public async Task<ApplicationUser> GetCurrentUserByEmailAsync(string email)
    {
        var user = await _dependencyConfiguration._userManager.FindByEmailAsync(email);
        return user;
    }

    public async Task<bool> CheckUserAsync(string email)
    {
        var user = await GetCurrentUserByEmailAsync(email);
        if (user != null)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> PasswordUpdate(string email, string password)
    {
        var user = await GetCurrentUserByEmailAsync(email);
        var token = await _dependencyConfiguration._userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _dependencyConfiguration._userManager.ResetPasswordAsync(user, token, password);
        if (result.Succeeded)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> CheckEmail(string email)
    {
        var emailCheck = await _dependencyConfiguration._userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (emailCheck == null)
        {
            return false;
        }

        return true;
    }
}
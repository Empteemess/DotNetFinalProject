using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Interfaces;

public interface IAccountService
{
    Task<bool> RegisterAsync(RegisterViewModel model);
    Task<bool> LoginAsync(LoginViewModel model);
    Task LogOutAsync();
   
    Task CreateRoleAsync(string roleName);
    Task RemoveRoleAsync(RoleViewModel model);
    Task<bool>? CheckUserAsync(string email);
    Task<bool> PasswordUpdate(string email, string password);
    Task<bool> CheckEmail(string email);
}
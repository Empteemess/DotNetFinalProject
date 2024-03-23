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
    

}
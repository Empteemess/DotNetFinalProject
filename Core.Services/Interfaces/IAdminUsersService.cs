using FinalProject.Models;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Interfaces;

public interface IAdminUsersService
{
    Task<IEnumerable<UserViewModel>> GetUsers();
    Task<UserViewModel> GetUserByEmail(string userEmail);
    IEnumerable<IdentityRole> GetRoles();
    Task<bool> AddRole(string roleName);
    Task<bool> DeleteRole(string role);
    Task UpdateRole(string email, string newRole);
    Task<IEnumerable<UserViewModel>> FilterByInput(int currentPage, int maxItem, string email, string filterName);
    int Count();
}
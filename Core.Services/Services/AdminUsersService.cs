using System.ComponentModel.Design;
using System.Security.Claims;
using FinalProject.Configurations;
using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Services;

public class AdminUsersService : IAdminUsersService
{
    private readonly DependencyConfiguration _dependencyConfiguration;

    public AdminUsersService(DependencyConfiguration dependencyConfiguration)
    {
        _dependencyConfiguration = dependencyConfiguration;
    }

    public int Count()
    {
        var user = _dependencyConfiguration._userManager.Users.Count();
        return user;
    }

    public IEnumerable<IdentityRole> GetRoles()
    {
        var roles = _dependencyConfiguration._roleManager.Roles;
        return roles;
    }

    public async Task<bool> AddRole(string roleName)
    {
        if (!string.IsNullOrEmpty(roleName))
        {
            var checkRole = await _dependencyConfiguration._roleManager.FindByNameAsync(roleName);

            if (checkRole == null)
            {
                var addRole =
                    await _dependencyConfiguration._roleManager.CreateAsync(new IdentityRole() { Name = roleName });
                if (addRole.Succeeded)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public async Task<UserViewModel> GetUserByEmail(string userEmail)
    {
        var roles = _dependencyConfiguration._roleManager.Roles;
        var users = await GetUsers();

        var user = users.FirstOrDefault(x => x.Email == userEmail);

        user.IdentityRole = roles;
        return user;
    }

    public async Task<IEnumerable<UserViewModel>> GetUsers()
    {
        var users = _dependencyConfiguration._userManager.Users.ToList();
        var userViewModel = new List<UserViewModel>();

        foreach (var item in users)
        {
            var userRole = await _dependencyConfiguration._userManager.GetRolesAsync(item);
            var role = string.Join("", userRole);

            userViewModel.Add(new UserViewModel()
            {
                Email = item.Email,
                Role = role,
            });
        }

        return userViewModel;
    }

    public async Task<bool> DeleteRole(string role)
    {
        var currentRole = _dependencyConfiguration._roleManager.Roles.FirstOrDefault(x => x.Name == role);
        if (currentRole != null)
        {
            await _dependencyConfiguration._roleManager.DeleteAsync(currentRole);
            return true;
        }

        return false;
    }

    public async Task UpdateRole(string email, string newRole)
    {
        var user = await _dependencyConfiguration._userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

        var userRole = await _dependencyConfiguration._userManager.GetRolesAsync(user);
        var role = string.Join("", userRole);

        if (!string.IsNullOrEmpty(role))
        {
            await _dependencyConfiguration._userManager.RemoveFromRoleAsync(user, role);
        }

        await _dependencyConfiguration._userManager.AddToRoleAsync(user, newRole);
        _dependencyConfiguration._userManager.UpdateAsync(user);
    }

    public async Task<IEnumerable<UserViewModel>> FilterByInput(int currentPage, int maxItem, string email,
        string filterName)
    {
        IEnumerable<UserViewModel> user;

        if (!string.IsNullOrEmpty(email) && filterName == "Search")
        {
            var users = await GetUsers();
            user = users.Where(x => x.Email.Contains(email));
        }
        else
        {
            user = await GetUsers();
        }

        var result = user.Skip((currentPage - 1) * maxItem).Take(maxItem);

        return result;
    }

    public async Task<bool> DeleteUserByEmail(string email)
    {
        var user = await _dependencyConfiguration._userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var result = await _dependencyConfiguration._userManager.DeleteAsync(user);
            return true;
        }

        return false;
    }
}
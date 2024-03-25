using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

[Authorize(Roles = "Admin")]
public class AdminUsersController : Controller
{
    private readonly IAdminUsersService _service;

    public AdminUsersController(IAdminUsersService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(string email, string filterName, int currentPage = 1, int maxItem = 6)
    {
        var count = _service.Count();
        var users = await _service.FilterByInput(currentPage,maxItem,email, filterName);

        ViewBag.RoleList = _service.GetRoles();
        
        ViewBag.currentPage = currentPage;
        ViewBag.PageNum = (int)Math.Ceiling(count / (double)maxItem);
        return View(users);
    }

    public async Task<IActionResult> Update(string email)
    {
        var users = await _service.GetUserByEmail(email);
        return View(users);
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var roleList = _service.GetRoles();
        var roles = new UserViewModel()
        {
            IdentityRole = roleList
        };

        return View(roles);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserViewModel model)
    {
        var role = await _service.AddRole(model.AddRoleName);
        return RedirectToAction(nameof(Edit));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateRole(UserViewModel model, string email)
    {
        await _service.UpdateRole(email, model.UpdateRoleName);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(string role)
    {
        await _service.DeleteRole(role);
        return RedirectToAction(nameof(Edit));
    }
}
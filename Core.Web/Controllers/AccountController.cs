using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _service;

    public AccountController(IAccountService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.LoginAsync(model);
            if (result)
            {
                return RedirectToAction("Index","Home");
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var success = await _service.RegisterAsync(model);
            if (success)
            {
                return RedirectToAction("Index","Home");
            }
        }

        return View(model);
    }

    public async Task<IActionResult> LogOut()
    {
       _service.LogOutAsync();
       return RedirectToAction("Index", "Home");
    }
}
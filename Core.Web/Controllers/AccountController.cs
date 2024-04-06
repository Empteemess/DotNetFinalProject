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
                return RedirectToAction("Index", "Home");
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
        var emailCheck = await _service.CheckEmail(model.Email);
        if (ModelState.IsValid && !emailCheck)
        {
            var success = await _service.RegisterAsync(model);
            if (success)
            {
                return RedirectToAction(nameof(Login));
            }
        }

        return View(model);
    }

    public async Task<IActionResult> LogOut()
    {
        _service.LogOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> EmailCheck(string email)
    {
        var check = await _service.CheckUserAsync(email);
        return RedirectToAction(nameof(RecoverPassword), new { email = email, check = check });
    }

    [HttpGet]
    public IActionResult RecoverPassword(string email = "", bool check = false)
    {
        var model = new RecoveryPasswordViewModel();
        model.EmailCheck = check;
        model.Email = email;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> RecoverPassword(RecoveryPasswordViewModel model, bool check = false)
    {
        var update = await _service.PasswordUpdate(model.Email, model.Password);
        if (update)
        {
            model.EmailCheck = check;
            return RedirectToAction(nameof(Login));
        }

        return View(model);
    }
}
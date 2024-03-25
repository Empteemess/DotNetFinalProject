using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
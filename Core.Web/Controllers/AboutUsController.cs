using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

public class AboutUsController : Controller
{
    public AboutUsController()
    {
    }
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

}
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;

public class ContactController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
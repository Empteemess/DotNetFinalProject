using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalProject.Controllers;

public class ErrorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class ErrorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

using Microsoft.AspNetCore.Mvc;

namespace PhotoFolio.Controllers.About;

public class AboutController : Controller
{
    public IActionResult Index()
    {
        this.ViewData["ActivePage"] = "About";
        return View();
    }
}

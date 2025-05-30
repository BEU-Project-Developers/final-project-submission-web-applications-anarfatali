using Microsoft.AspNetCore.Mvc;

namespace PhotoFolio.Controllers.Services;

public class ServicesController : Controller
{
    public IActionResult Services()
    {
        this.ViewData["ActivePage"] = "Services";
        return View();
    }
}

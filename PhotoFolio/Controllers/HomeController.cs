using Microsoft.AspNetCore.Mvc;

namespace PhotoFolio.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["ActivePage"] = "Home";
        return View();
    }

    public IActionResult About()
    {
        ViewData["ActivePage"] = "About";
        return View();
    }
    public IActionResult Gallery()
    {
        ViewData["ActivePage"] = "Gallery";
        return View();
    }
    public IActionResult Services()
    {
        ViewData["ActivePage"] = "Services";
        return View();
    }
    public IActionResult Contact()
    {
        ViewData["ActivePage"] = "Contact";
        return View();
    }
    public IActionResult GallerySingle()
    {
        ViewData["ActivePage"] = "GallerySingle";
        return View();
    }
}

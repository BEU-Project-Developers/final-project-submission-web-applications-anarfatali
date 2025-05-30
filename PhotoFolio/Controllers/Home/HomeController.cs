using Microsoft.AspNetCore.Mvc;
using PhotoFolio.Services;

namespace PhotoFolio.Controllers;

public class HomeController : Controller
{
    // private readonly ILogger<HomeController> _logger;
    // public HomeController(ILogger<HomeController> logger)
    // {
    //     _logger = logger;
    // }
    
    private readonly IGalleryService _galleryService;

    public HomeController(IGalleryService galleryService)
    {
        _galleryService = galleryService;
    }


    public async Task<IActionResult> Index()
    {
        ViewData["ActivePage"] = "Home";
        var galleryItems = await _galleryService.GetGalleryItemsAsync();
        return View(galleryItems);
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
}

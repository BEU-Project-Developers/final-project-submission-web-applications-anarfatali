using Microsoft.AspNetCore.Mvc;
using PhotoFolio.Services;

namespace PhotoFolio.Controllers.Home;

public class HomeController : Controller
{
    private readonly IGalleryService _galleryService;

    public HomeController(IGalleryService galleryService)
    {
        _galleryService = galleryService;
    }

    public async Task<IActionResult> Index()
    {
        this.ViewData["ActivePage"] = "Home";
        var galleryItems = await _galleryService.GetGalleryItemsAsync();
        return View(galleryItems);
    }
}

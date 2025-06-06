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
        
        try
        {
            var galleryItems = await _galleryService.GetGalleryItemsAsync();
            return View(galleryItems);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HomeController.Index] Error: {ex.Message}");
            ViewBag.ErrorMessage = ex.Message;
            return View();
        }
    }
}

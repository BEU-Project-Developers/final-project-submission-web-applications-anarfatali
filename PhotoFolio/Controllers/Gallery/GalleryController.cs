using Microsoft.AspNetCore.Mvc;
using PhotoFolio.Services;
using PhotoFolio.ViewModels;

namespace PhotoFolio.Controllers.Gallery;

public class GalleryController : Controller
{
    private readonly IPhotoService _photoService;

    public GalleryController(IPhotoService photoService)
    {
        _photoService = photoService;
    }

    public async Task<IActionResult> Index(string? category)
    {
        this.ViewData["ActivePage"] = "Gallery";

        try
        {
            var photos = await _photoService.GetPhotosByCategoryAsync(category);

            var vm = new GalleryViewModel
            {
                SelectedCategory = category,
                Photos = photos
            };

            return View(vm);
        }
        catch (Exception ex)
        {
            // loglama üçün istəsən burda loglama servisinə ötürə bilərsən
            Console.WriteLine($"[GalleryController.Index] Error: {ex.Message}");
            ViewBag.ErrorMessage = ex.Message;
            return View("Index");
        }
    }
}

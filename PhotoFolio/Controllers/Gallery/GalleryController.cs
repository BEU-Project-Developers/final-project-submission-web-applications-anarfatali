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
        
        var photos = await _photoService.GetPhotosByCategoryAsync(category);

        var vm = new GalleryViewModel
        {
            SelectedCategory = category,
            Photos = photos
        };
        
        return View(vm);
    }
}

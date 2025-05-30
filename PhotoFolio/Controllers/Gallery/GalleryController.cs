using Microsoft.AspNetCore.Mvc;
using PhotoFolio.Services;

namespace PhotoFolio.Controllers.Gallery;

public class GalleryController : Controller
{
    private readonly IPhotoService _photoService;

    public GalleryController(IPhotoService photoService)
    {
        _photoService = photoService;
    }

    public async Task<IActionResult> Gallery()
    {
        this.ViewData["ActivePage"] = "Gallery";
        var photos = await _photoService.GetAllPhotosAsync();
        return View(photos);
    }
}

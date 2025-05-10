using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoFolio.Models;

namespace PhotoFolio.Controllers;

// [Authorize(Roles = "Photographer")]
public class PhotoController : Controller
{
    [HttpGet]
    public IActionResult Upload()
    {
        if (!User.IsInRole("Photographer"))
        {
            return RedirectToAction("Request", "Photo");
        }

        // 3) Otherwise user *is* Photographer or Admin:
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(PhotoUploadViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        // Şəkili yaddaşa yaz, DATA-yə url əlavə et
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Request()
    {
        return View();
    }

    
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoFolio.DATA;
using PhotoFolio.Models;
using PhotoFolio.ViewModels;

namespace PhotoFolio.Controllers;

[Authorize(Roles = "Photographer")]
public class PhotoController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ApplicationDbContext _context;

    public PhotoController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
    {
        _webHostEnvironment = webHostEnvironment;
        _context = context;
    }

    [HttpGet]
    public IActionResult Upload()
    {
        if (!User.IsInRole("Photographer"))
        {
            return RedirectToAction("Request", "Photo");
        }

        return View();
    }

    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Upload(PhotoUploadViewModel vm)
    // {
    //     if (!ModelState.IsValid) return View(vm);
    //
    //     // Şəkili yaddaşa yaz, DATA-yə url əlavə et
    //     return RedirectToAction("Index", "Home");
    // }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(PhotoUploadViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        if (vm.File.Length == 0)
        {
            ModelState.AddModelError("File", "Please select a file.");
            return View(vm);
        }

        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(vm.File.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await vm.File.CopyToAsync(fileStream);
        }

        var photo = new Photo
        {
            FileName = uniqueFileName,
            Url = "/uploads/" + uniqueFileName,
            Title = vm.Title,
            Description = vm.Description,
            UploadedAt = DateTime.Now,
        };

        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Request()
    {
        return View();
    }
}

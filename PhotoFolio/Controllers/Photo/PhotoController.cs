using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoFolio.DATA;
using PhotoFolio.Models;
using PhotoFolio.ViewModels;

namespace PhotoFolio.Controllers.Photo;

public class PhotoController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public PhotoController(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _db = db;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Upload()
    {
        // (a) If user is not signed in, force login:
        var user = await _userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return Challenge();
        }

        // (b) If user exists but is NOT a Photographer → show the “Request” form instead:
        if (!await _userManager.IsInRoleAsync(user, "Photographer"))
        {
            return RedirectToAction(nameof(ApplyPhotographer));
        }

        var photosForUser = await _db.Photos
            .Where(p => p.PhotographerId == user.Id)
            .OrderByDescending(p => p.UploadedAt)
            .ToListAsync();

        return View("Upload", photosForUser);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(IFormFile[]? files)
    {
        var user = await _userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return Challenge();
        }

        // If the user somehow lost the “Photographer” role in the meantime, bounce back to GET Upload
        if (!await _userManager.IsInRoleAsync(user, "Photographer"))
        {
            return RedirectToAction(nameof(Upload));
        }

        // If no files were posted:
        if (files == null || files.Length == 0)
        {
            ModelState.AddModelError("", "You must select at least one file.");

            var existing = await _db.Photos
                .Where(p => p.PhotographerId == user.Id)
                .OrderByDescending(p => p.UploadedAt)
                .ToListAsync();
            return View("Upload", existing);
        }

        // If too many files at once:
        if (files.Length > 10)
        {
            ModelState.AddModelError("", "You can upload up to 10 photos at once.");
            var existing = await _db.Photos
                .Where(p => p.PhotographerId == user.Id)
                .OrderByDescending(p => p.UploadedAt)
                .ToListAsync();
            return View("Upload", existing);
        }

        var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/jpg" };

        foreach (var file in files)
        {
            if (file.Length == 0)
            {
                ModelState.AddModelError("", $"File '{file.FileName}' is invalid or empty.");
                continue;
            }

            if (file.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("", $"\"{file.FileName}\" can't exceed 10MB.");
                continue;
            }

            if (!allowedContentTypes.Contains(file.ContentType))
            {
                ModelState.AddModelError("", $"\"{file.FileName}\" must be JPG, PNG, or GIF.");
                continue;
            }

            byte[] data;
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                data = ms.ToArray();
            }

            var uploadsRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsRoot))
            {
                Directory.CreateDirectory(uploadsRoot);
            }

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsRoot, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var random = new Random();
            var categoryId = random.Next(1, 7);

            var photo = new Models.Photo
            {
                CategoryId = categoryId,
                FileName = Path.GetFileName(file.FileName),
                ContentType = file.ContentType,
                Data = data,
                Url = $"/uploads/{uniqueFileName}",
                PhotographerId = user.Id,
                UploadedAt = DateTime.UtcNow
            };

            _db.Photos.Add(photo);
        }

        await _db.SaveChangesAsync();

        TempData["Success"] = "Photos uploaded successfully!";
        return RedirectToAction(nameof(Upload));
    }

    [HttpGet]
    public IActionResult ApplyPhotographer()
    {
        return View("Request", new PhotographerApplicationViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApplyPhotographer(PhotographerApplicationViewModel vm)
    {
        if (!this.ModelState.IsValid)
        {
            return View("Request", vm);
        }

        // Şu an aşırı basit: Kullanıcı başvuru formu doldurunca doğrudan rol veriyoruz.
        var user = await _userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return Challenge();
        }

        var request = new PhotographerRequest
        {
            UserId = user.Id,
            Name = vm.Name,
            Surname = vm.Surname,
            Age = vm.Age,
            Experience = vm.Experience,
            PortfolioUrl = vm.PortfolioUrl,
            RequestedAt = DateTime.UtcNow,
            Status = "Pending"
        };

        _db.PhotographerRequests.Add(request);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Your request has been submitted. Wait for admin approval.";
        return RedirectToAction(nameof(ApplyPhotographer));
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetImage(int id)
    {
        var photo = _db.Photos.Find(id);
        if (photo == null)
        {
            return NotFound();
        }

        return File(photo.Data, photo.ContentType);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userManager.GetUserAsync(this.User);
        if (user == null)
        {
            return Challenge();
        }

        var photo = await _db.Photos.FindAsync(id);
        if (photo == null)
        {
            return NotFound();
        }

        if (photo.PhotographerId != user.Id)
        {
            return Forbid();
        }

        _db.Photos.Remove(photo);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Photo deleted successfully.";
        return RedirectToAction(nameof(Upload));
    }
}

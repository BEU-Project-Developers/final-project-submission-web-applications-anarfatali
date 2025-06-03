using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoFolio.DATA;

namespace PhotoFolio.Controllers.About;

public class AboutController : Controller
{
    private readonly ApplicationDbContext _db;
    
    public AboutController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<IActionResult> Index()
    {
        this.ViewData["ActivePage"] = "About";
        try
        {
            var testimonials = await _db.Feedbacks
                .Where(f => f.Id >= 1 && f.Id <= 5)
                .Include(f => f.User)
                .OrderBy(f => f.Id)
                .ToListAsync();

            return View(testimonials);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[AboutController.Index] Error: {ex.Message}");

            return View("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PhotoFolio.DATA;

namespace PhotoFolio.Controllers.Services;

public class ServicesController : Controller
{
    private readonly ApplicationDbContext _db;
    
    public ServicesController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public IActionResult Index()
    {
        this.ViewData["ActivePage"] = "Services";
        try
        {
            var feedbacks = _db.Feedbacks
                .Where(f => f.Id >= 6 && f.Id <= 10)
                .ToList();

            return View(feedbacks);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ServicesController.Index] Error: {ex.Message}");
            ViewBag.ErrorMessage = ex.Message;
            return View();
        }
    }
}

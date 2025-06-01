using Microsoft.AspNetCore.Mvc;
using PhotoFolio.DATA;
using PhotoFolio.Models;

namespace PhotoFolio.Controllers.Contact;

public class ContactController : Controller
{
    private readonly ApplicationDbContext _db;
    public ContactController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        this.ViewData["ActivePage"] = "Contact";
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Send(ContactMessage model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { 
                success = false, 
                error = "Please fill in all required fields correctly." 
            });
        }

        try
        {
            var message = new ContactMessage
            {
                Name = model.Name,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message,
                SentAt = DateTime.UtcNow
            };

            _db.ContactMessages.Add(message);
            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }
        catch (Exception)
        {
            return Json(new { 
                success = false, 
                error = "Failed to send message. Please try again later." 
            });
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoFolio.DATA;
using PhotoFolio.Models;

namespace PhotoFolio.Controllers.Contact;

public class ContactController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public ContactController(ApplicationDbContext db,
        UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
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
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Json(new { 
                    success = false, 
                    error = "You must be logged in to send a message." 
                });
            }

            var message = new ContactMessage
            {
                Name = model.Name,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message,
                SentAt = DateTime.UtcNow,
                ApplicationUserId = currentUser.Id
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

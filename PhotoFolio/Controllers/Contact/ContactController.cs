using Microsoft.AspNetCore.Mvc;

namespace PhotoFolio.Controllers.Contact;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        this.ViewData["ActivePage"] = "Contact";
        return View();
    }
}

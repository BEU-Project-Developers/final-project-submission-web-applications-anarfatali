using Microsoft.AspNetCore.Mvc;

namespace PhotoFolio.Controllers.Contact;

public class ContactController : Controller
{
    public IActionResult Contact()
    {
        this.ViewData["ActivePage"] = "Contact";
        return View();
    }
}

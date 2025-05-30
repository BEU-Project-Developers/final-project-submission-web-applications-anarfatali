using Microsoft.AspNetCore.Mvc;
using PhotoFolio.ViewModels;

namespace PhotoFolio.Controllers.Account
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View(new AccountViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AccountViewModel vm)
        {
            if (vm.Username == "anar@gmail.com" && vm.Password == "1234")
                return RedirectToAction("Index", "Home");

            this.ModelState.AddModelError("error", "Wrong username or password.");

            return View("Login", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(AccountViewModel vm)
        {
            if (!this.ModelState.IsValid) return View("Login", vm);

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Profile(AccountViewModel vm)
        {
            if (this.ModelState.IsValid)
            {
                return View("Profile", vm);
            }

            return View("Login", vm);
        }
    }
}

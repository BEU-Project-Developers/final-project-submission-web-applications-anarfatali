using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoFolio.Models;
using PhotoFolio.ViewModels;

namespace PhotoFolio.Controllers.Account;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel vm, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid)
            return View(vm);

        // 1) İlk öncə, login dəyəri username kimi yoxla
        ApplicationUser user = await _userManager.FindByNameAsync(vm.Login);

        // 2) Əgər yuxarıdakı tapmadısa, onda email kimi yoxla
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(vm.Login);
        }

        // 3) Hələ də user null-dursa, deməli hem username, hem email bazada yoxdur
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Wrong username/email or password.");
            return View(vm);
        }

        // 4) User tapılıb, indi onun UserName sahəsini götürüb PasswordSignInAsync ilə yoxla
        var result = await _signInManager.PasswordSignInAsync(
            user.UserName, 
            vm.Password, 
            vm.RememberMe, 
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "This account has been locked out. Try again later.");
            return View(vm);
        }

        // Əgər şifrə səhvdisə
        ModelState.AddModelError(string.Empty, "Wrong username/email or password.");
        return View(vm);
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var user = new ApplicationUser
        {
            FullName = vm.Fullname,
            UserName = vm.Username,
            Email = vm.Email
        };

        var result = await _userManager.CreateAsync(user, vm.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhotoFolio.Models;
using PhotoFolio.ViewModels;

namespace PhotoFolio.Controllers.Profile;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ProfileController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        // Əgər istifadəçi oturum açmayıbsa, login səhifəsinə yönləndirək
        if (!this.User.Identity?.IsAuthenticated ?? false)
            return RedirectToAction("Login", "Account");

        // Məlumatları çəkmək üçün hazırkı istifadəçini götürək
        var user = await _userManager.GetUserAsync(this.User);
        if (user == null)
            return RedirectToAction("Login", "Account");

        // ViewModel-i dolduraq
        var model = new AccountSettingsViewModel
        {
            FullName = user.FullName ?? "",
            Email = user.Email ?? "",
            Username = user.UserName ?? ""
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(AccountSettingsViewModel vm)
    {
        if (!this.ModelState.IsValid)
            return View(vm);

        var user = await _userManager.GetUserAsync(this.User);
        if (user == null)
            return RedirectToAction("Login", "Account");

        // 1) Cari şifrəni yoxlayaq
        var checkPwd = await _userManager.CheckPasswordAsync(user, vm.CurrentPassword);
        if (!checkPwd)
        {
            this.ModelState.AddModelError(nameof(vm.CurrentPassword), "Current password is incorrect.");
            return View(vm);
        }

        // 2) Email-i dəyişmək lazımdırsa
        if (!string.Equals(user.Email, vm.Email, StringComparison.OrdinalIgnoreCase))
        {
            var setEmailResult = await _userManager.SetEmailAsync(user, vm.Email);
            if (!setEmailResult.Succeeded)
            {
                foreach (var err in setEmailResult.Errors)
                    this.ModelState.AddModelError(string.Empty, err.Description);
                return View(vm);
            }
        }

        // 3) FullName-i güncəlləyək
        user.FullName = vm.FullName;
        var updateUserResult = await _userManager.UpdateAsync(user);
        if (!updateUserResult.Succeeded)
        {
            foreach (var err in updateUserResult.Errors)
                this.ModelState.AddModelError(string.Empty, err.Description);
            return View(vm);
        }

        //username update
        user.UserName = vm.Username;
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            foreach (var err in updateResult.Errors)
                this.ModelState.AddModelError(string.Empty, err.Description);
            return View(vm);
        }

        // 4) Yeni şifrə verilibsə dəyişək
        if (!string.IsNullOrEmpty(vm.NewPassword))
        {
            var changePwdResult = await _userManager.ChangePasswordAsync(user, vm.CurrentPassword, vm.NewPassword);
            if (!changePwdResult.Succeeded)
            {
                foreach (var err in changePwdResult.Errors)
                    this.ModelState.AddModelError(nameof(vm.NewPassword), err.Description);
                return View(vm);
            }
        }

        // 5) Yenidən signin edək ki, security stamp güncəllənsin
        await _signInManager.RefreshSignInAsync(user);

        // 6) Uğurlu yeniləmə statusunu göstərmək üçün modelə bayraq qoyaq
        vm.UpdateSucceeded = true;
        vm.CurrentPassword = ""; // sabit təhlükəsizlik üçün boşaldaq
        vm.NewPassword = "";
        vm.ConfirmPassword = "";

        return View(vm);
    }
}

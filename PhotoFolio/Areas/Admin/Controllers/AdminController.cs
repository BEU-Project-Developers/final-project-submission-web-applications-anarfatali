using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoFolio.DATA;
using PhotoFolio.Models;
using PhotoFolio.ViewModels;

namespace PhotoFolio.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[AllowAnonymous]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> AdminPanel()
    {
        ViewData["ActivePage"] = "Dashboard";
        try
        {
            int totalUsers = await _db.Users.CountAsync();
            int activeUsers = await _db.Users.CountAsync(u => u.EmailConfirmed);

            var photographerRole = await _roleManager.FindByNameAsync("Photographer");
            int photographersCount = 0;
            if (photographerRole != null)
            {
                photographersCount = await _db.UserRoles
                    .CountAsync(ur => ur.RoleId == photographerRole.Id);
            }

            int pendingRequests = await _db.PhotographerRequests
                .CountAsync(r => r.Status == "Pending");

            int photosUploaded = await _db.Photos.CountAsync();

            var recentActiveUsers = await _db.Users
                .OrderByDescending(u => u.Id)
                .Select(u => new ActiveUserDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = "",
                    Status = u.EmailConfirmed ? "Active" : "Inactive",
                })
                .ToListAsync();

            foreach (var au in recentActiveUsers)
            {
                var user = await _userManager.FindByIdAsync(au.Id);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    au.Role = roles.FirstOrDefault() ?? "User";
                }
            }

            var pendingReqs = await _db.PhotographerRequests
                .OrderByDescending(r => r.RequestedAt)
                .ToListAsync();

            var photographerRequestsDto = new List<PhotographerRequestDto>();
            foreach (var req in pendingReqs)
            {
                var user = await _userManager.FindByIdAsync(req.UserId);
                photographerRequestsDto.Add(new PhotographerRequestDto
                {
                    Id = req.Id,
                    FullName = user?.FullName ?? "Unknown",
                    Email = user?.Email ?? "unknown@example.com",
                    Experience = req.Experience,
                    PortfolioLink = req.PortfolioUrl,
                    SubmittedDisplay = req.RequestedAt.ToString("MMM dd, yyyy HH:mm"),
                    Status = req.Status
                });
            }

            var vm = new AdminDashboardViewModel
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                PhotographersCount = photographersCount,
                PendingRequests = pendingRequests,
                PhotosUploaded = photosUploaded,
                ActiveUsersList = recentActiveUsers,
                PhotographerRequests = photographerRequestsDto
            };

            return View(vm);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AdminController.AdminPanel] Error: {ex.Message}");
            ViewBag.ErrorMessage = "Something went wrong!.";
            return View();
        }
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveRequest(int requestId)
    {
        var request = await _db.PhotographerRequests.FindAsync(requestId);
        if (request == null)
        {
            TempData["ErrorMessage"] = "Photographer request not found.";
            return RedirectToAction("AdminPanel");
        }

        request.Status = "Approved";
        _db.PhotographerRequests.Update(request);

        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user != null)
        {
            if (!await _roleManager.RoleExistsAsync("Photographer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Photographer"));
            }

            if (!await _userManager.IsInRoleAsync(user, "Photographer"))
            {
                await _userManager.AddToRoleAsync(user, "Photographer");
            }
        }

        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = "Request approved and now user has Photographer role.";
        return RedirectToAction("AdminPanel");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectRequest(int requestId)
    {
        var request = await _db.PhotographerRequests.FindAsync(requestId);
        if (request == null)
        {
            TempData["ErrorMessage"] = "Photographer request not found.";
            return RedirectToAction("AdminPanel");
        }

        request.Status = "Rejected";
        _db.PhotographerRequests.Update(request);
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = "Request rejected.";
        return RedirectToAction("AdminPanel");
    }


    public IActionResult PhotoGallery()
    {
        ViewData["ActivePage"] = "Photo Gallery";
        return View();
    }

    public async Task<IActionResult> Users()
    {
        ViewData["ActivePage"] = "Users";
        var usersDtoList = await _db.Users
            .OrderByDescending(u => u.Id)
            .Select(u => new ActiveUserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = "User",
                Status = u.EmailConfirmed ? "Active" : "Inactive",
            })
            .ToListAsync();

        var vm = new AdminDashboardViewModel
        {
            ActiveUsersList = usersDtoList,
        };

        return View(vm);
    }

    public async Task<IActionResult> Photographers()
    {
        ViewData["ActivePage"] = "Photographers";

        var model = new AdminDashboardViewModel();

        var photographersInRole = await _userManager.GetUsersInRoleAsync("Photographer");

        var photographerDtos = new List<PhotographerDto>();

        foreach (var user in photographersInRole)
        {
            var request = await _db.PhotographerRequests
                .Where(r => r.UserId == user.Id && r.Status == "Approved")
                .FirstOrDefaultAsync();

            string experience = request?.Experience ?? "0-1";

            int photoCount = await _db.Photos
                .CountAsync(p => p.PhotographerId == user.Id);

            string status = user.EmailConfirmed ? "Active" : "Inactive";

            photographerDtos.Add(new PhotographerDto
            {
                Id = user.Id,
                FullName = string.IsNullOrEmpty(user.FullName)
                    ? user.UserName
                    : user.FullName,
                Email = user.Email,
                Experience = experience,
                PhotoCount = photoCount,
                Status = status
            });
        }

        model.Photographers = photographerDtos;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var messages = _db.ContactMessages.Where(cm => cm.ApplicationUserId == user.Id);
            _db.ContactMessages.RemoveRange(messages);

            var feedbacks = _db.Feedbacks.Where(fb => fb.UserId == user.Id);
            _db.Feedbacks.RemoveRange(feedbacks);

            var galleries = _db.Galleries.Where(g => g.UserId == user.Id);
            _db.Galleries.RemoveRange(galleries);

            var requests = _db.PhotographerRequests.Where(r => r.UserId == user.Id);
            _db.PhotographerRequests.RemoveRange(requests);

            await _db.SaveChangesAsync();

            var identityResult = await _userManager.DeleteAsync(user);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                TempData["ErrorMessage"] = "Can't delete user.";
                return RedirectToAction("Users");
            }

            TempData["SuccessMessage"] = "User deleted successfully.";
            return RedirectToAction("AdminPanel");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AdminController.DeleteUser] Error: {ex.Message}");
            TempData["ErrorMessage"] = "Something went wrong. Please try again.";
            return RedirectToAction("AdminPanel");
        }
    }
}

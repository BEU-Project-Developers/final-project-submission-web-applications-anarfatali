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
            .OrderByDescending(u => u.Id) // əgər Id artan ardıcıllıqdadırsa, yoxsa son login tarixinə görə OrderByDescending
            .Select(u => new ActiveUserDto
            {
                Id = u.Id,
                FullName = u.FullName, // Əgər ApplicationUser-də FullName sahəsi varsa
                Email = u.Email,
                Role = "", // Rolu sonra dolduraq
                Status = u.EmailConfirmed ? "Active" : "Inactive",
            })
            .ToListAsync();

        // İndi hər bir ActiveUserDto üçün rolu da alaq:
        foreach (var au in recentActiveUsers)
        {
            var user = await _userManager.FindByIdAsync(au.Id);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                au.Role = roles.FirstOrDefault() ?? "User";

                // Məsələn, LastActiveDisplay üçün əgər ApplicationUser-də LastLoginAt tərzi bir datetime sahəniz varsa,
                // onu aşağıdakı kimi göstərə bilərsiniz:
                // au.LastActiveDisplay = user.LastLoginAt?.ToString("g") ?? "N/A";
            }
        }

        var pendingReqs = await _db.PhotographerRequests
            .OrderByDescending(r => r.RequestedAt) // əgər SubmittedAt tarixi varsa
            .ToListAsync();

        var photographerRequestsDto = new List<PhotographerRequestDto>();
        foreach (var req in pendingReqs)
        {
            // req.UserId -> applicationUser Id-si
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

        // 2) Load all users who have the "Photographer" role
        var photographersInRole = await _userManager.GetUsersInRoleAsync("Photographer");

        var photographerDtos = new List<PhotographerDto>();

        foreach (var user in photographersInRole)
        {
            // 3) Find the approved PhotographerRequest (if any) to get Experience
            var request = await _db.PhotographerRequests
                .Where(r => r.UserId == user.Id && r.Status == "Approved")
                .FirstOrDefaultAsync();

            string experience = request?.Experience ?? "0-1";

            // 4) Count how many photos this user has uploaded
            int photoCount = await _db.Photos
                .CountAsync(p => p.PhotographerId == user.Id);

            // 5) Determine status (Active if email is confirmed)
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

        // 6) Assign the list into our AdminDashboardViewModel
        model.Photographers = photographerDtos;

        // (If you also want to fill other properties—like ActiveUsers, TotalUsers, etc.—do so here.)

        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        if (string.IsNullOrEmpty(id))
            return NotFound();

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
            // You could add ModelState errors or show a flash message here.
            // For now, just redirect back with an error
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return RedirectToAction("Users");
        }

        return RedirectToAction("AdminPanel");  
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoFolio.Models;
using PhotoFolio.ViewModels;

namespace PhotoFolio.Areas.Admin.Controllers;

[Area("Admin")]
// [Authorize(Roles = "Admin")]
[AllowAnonymous]
public class AdminController : Controller
{
    public IActionResult AdminPanel()
    {
        //view home
        var vm = new AdminDashboardViewModel
        {
            // Statistik kartlar
            TotalUsers = 2390,
            ActiveUsers = 924,
            PhotographersCount = 214,
            PendingRequests = 12,
            PhotosUploaded = 15742,

            // Active Users cədvəli üçün nümunə siyahı
            ActiveUsersList = new List<ActiveUserDto>
            {
                new ActiveUserDto
                {
                    Id = 12345, FullName = "John Doe", Email = "john.doe@example.com", Role = "User", Status = "Active",
                    LastActiveDisplay = "Just now"
                },
                new ActiveUserDto
                {
                    Id = 12346, FullName = "Jane Smith", Email = "jane.smith@example.com", Role = "Photographer", Status = "Active",
                    LastActiveDisplay = "5 minutes ago"
                },
                new ActiveUserDto
                {
                    Id = 12347, FullName = "Robert Johnson", Email = "robert.johnson@example.com", Role = "User", Status = "Active",
                    LastActiveDisplay = "15 minutes ago"
                },
                new ActiveUserDto
                {
                    Id = 12348, FullName = "Emily Davis", Email = "emily.davis@example.com", Role = "User", Status = "Idle",
                    LastActiveDisplay = "1 hour ago"
                },
                new ActiveUserDto
                {
                    Id = 12349, FullName = "Michael Wilson", Email = "michael.wilson@example.com", Role = "Photographer",
                    Status = "Active", LastActiveDisplay = "2 hours ago"
                },
            },

            // Photographer Approval Requests üçün nümunə siyahı
            PhotographerRequests = new List<PhotographerRequestDto>
            {
                new PhotographerRequestDto
                {
                    Id = 23456, FullName = "Sarah Thompson", Email = "sarah.thompson@example.com", Experience = "3 years",
                    PortfolioLink = "#", SubmittedDisplay = "Today, 9:41 AM", Status = "Pending"
                },
                new PhotographerRequestDto
                {
                    Id = 23457, FullName = "David Brown", Email = "david.brown@example.com", Experience = "5 years", PortfolioLink = "#",
                    SubmittedDisplay = "Yesterday, 3:22 PM", Status = "Pending"
                },
                new PhotographerRequestDto
                {
                    Id = 23458, FullName = "Jessica Miller", Email = "jessica.miller@example.com", Experience = "2 years",
                    PortfolioLink = "#", SubmittedDisplay = "Yesterday, 11:15 AM", Status = "Pending"
                },
                new PhotographerRequestDto
                {
                    Id = 23459, FullName = "Thomas Anderson", Email = "thomas.anderson@example.com", Experience = "7 years",
                    PortfolioLink = "#", SubmittedDisplay = "May 12, 2023", Status = "Approved"
                },
                new PhotographerRequestDto
                {
                    Id = 23460, FullName = "Lisa Taylor", Email = "lisa.taylor@example.com", Experience = "1 year", PortfolioLink = "#",
                    SubmittedDisplay = "May 10, 2023", Status = "Rejected"
                },
            }
        };

        ViewData["ActivePage"] = "Dashboard";
        return View(vm);
    }
    
    public IActionResult PhotoGallery()
    {
        ViewData["ActivePage"] = "Photo Gallery";
        return View();
    }

    public IActionResult Users()
    {
        ViewData["ActivePage"] = "Users";
        return View();
    }

    public IActionResult Photographers()
    {
        ViewData["ActivePage"] = "Photographers";
        return View();
    }
}

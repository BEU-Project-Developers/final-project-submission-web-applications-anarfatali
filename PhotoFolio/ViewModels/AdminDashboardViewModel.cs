namespace PhotoFolio.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int PhotographersCount { get; set; }
    public int PendingRequests { get; set; }
    public int PhotosUploaded { get; set; }

    public List<ActiveUserDto> ActiveUsersList { get; set; } = new();

    public List<PhotographerRequestDto> PhotographerRequests { get; set; } = new();
}

public class ActiveUserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
    public string Status { get; set; } = ""; // e.g. "Active", "Idle"
}

public class PhotographerRequestDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Experience { get; set; } = ""; // e.g. "3 years"
    public string PortfolioLink { get; set; } = ""; // URL
    public string SubmittedDisplay { get; set; } = ""; // e.g. "Today, 9:41 AM"
    public string Status { get; set; } = ""; // e.g. "Pending", "Approved"
}

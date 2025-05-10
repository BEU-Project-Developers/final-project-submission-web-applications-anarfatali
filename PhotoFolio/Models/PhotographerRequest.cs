namespace PhotoFolio.Models;

public class PhotographerRequest
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public ApplicationUser? User { get; set; }

    public string Experience { get; set; } = "";
    public string PortfolioUrl { get; set; } = "";
    public DateTime SubmittedAt { get; set; }
    public string Status { get; set; } = "Pending";
}

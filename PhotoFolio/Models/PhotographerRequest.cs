using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoFolio.Models;

public class PhotographerRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }

    // We store the user’s answers from the form:
    [Required]
    public string Name { get; set; }

    [Required]
    public string Surname { get; set; }

    [Required]
    public int Age { get; set; }

    [Required]
    public string Experience { get; set; }

    [Required]
    [Url]
    public string PortfolioUrl { get; set; }


    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public bool IsApproved { get; set; } = false;
    public DateTime? ApprovedAt { get; set; }
    public string? ApprovedByUserId { get; set; }
}

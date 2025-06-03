using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoFolio.Models;

public class PhotographerRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(50)]
    public string Surname { get; set; } = "";

    [Required]
    [Range(18, 99)]
    public int Age { get; set; }

    [Required]
    [StringLength(100)]
    public string Experience { get; set; } = "";

    [Required]
    [Url]
    [StringLength(200)]
    public string PortfolioUrl { get; set; } = "";

    [Required]
    public string UserId { get; set; } = "";
    
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Pending";
}

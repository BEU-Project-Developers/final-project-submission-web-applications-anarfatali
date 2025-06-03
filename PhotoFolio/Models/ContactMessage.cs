using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoFolio.Models;

public class ContactMessage
{
    [Key]
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, StringLength(200)]
    public string Subject { get; set; } = "";

    [Required]
    public string Message { get; set; } = "";

    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    // <-- we want this to be non-nullable (every message must reference an existing user) -->
    [Required]
    public string ApplicationUserId { get; set; } = "";

    [ForeignKey(nameof(ApplicationUserId))]
    public ApplicationUser User { get; set; }
}

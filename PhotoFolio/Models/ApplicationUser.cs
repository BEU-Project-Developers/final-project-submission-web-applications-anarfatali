using Microsoft.AspNetCore.Identity;

namespace PhotoFolio.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? ProfilePictureUrl { get; set; }

    public ICollection<PhotographerRequest>? PhotographerRequests { get; set; }
    public ICollection<Photo>? Photos { get; set; }
    public ICollection<Gallery>? Galleries { get; set; }
    public ICollection<Feedback>? Feedbacks { get; set; }
    public ICollection<ContactMessage>? ContactMessages { get; set; }
}

namespace PhotoFolio.Models;

public class Photo
{
    public int Id { get; set; }
    public string UploaderId { get; set; } = "";
    public ApplicationUser? Uploader { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public string Url { get; set; } = "";
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public DateTime UploadedAt { get; set; }

    public ICollection<Feedback>? Feedbacks { get; set; }
    public ICollection<PhotoTag>? PhotoTags { get; set; }
    public ICollection<Favorite>? Favorites { get; set; }
    public ICollection<PhotoAlbumItem>? PhotoAlbumItems { get; set; }
}

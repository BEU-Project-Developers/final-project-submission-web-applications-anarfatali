namespace PhotoFolio.Models;

public class Photo
{
    public int Id { get; set; }
    public string FileName { get; set; }

    public string PhotographerId { get; set; } = "";
    public ApplicationUser? Photographer { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public string Url { get; set; } = "";

    public string Title { get; set; } = "";
    public string? Description { get; set; }

    public DateTime UploadedAt { get; set; }

    public ICollection<PhotoAlbumItem>? PhotoAlbumItems { get; set; }
}

namespace PhotoFolio.Models;

public class Gallery
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public ApplicationUser? User { get; set; }

    public string Name { get; set; } = "";
    public DateTime CreatedAt { get; set; }

    public ICollection<PhotoAlbumItem>? PhotoAlbumItems { get; set; }
}

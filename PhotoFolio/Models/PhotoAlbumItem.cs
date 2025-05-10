namespace PhotoFolio.Models;

public class PhotoAlbumItem
{
    public int GalleryId { get; set; }
    public Gallery? Gallery { get; set; }

    public int PhotoId { get; set; }
    public Photo? Photo { get; set; }
}

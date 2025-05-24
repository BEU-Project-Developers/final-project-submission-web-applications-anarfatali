namespace PhotoFolio.Models;

public class Photo
{
    public int Id { get; set; }
    public string FileName { get; set; }
    
    // Yükleyen istifadəçi
    public string UploaderId { get; set; } = "";
    public ApplicationUser? Uploader { get; set; }

    // Kateqoriya (əgər varsa)
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    // Fiziki faylın URL-i (məsələn: "/uploads/1234abcd_photo.jpg")
    public string Url { get; set; } = "";

    // Şəklin başlığı və ixtiyari açıqlaması
    public string Title { get; set; } = "";
    public string? Description { get; set; }

    // Yükləmə tarixi
    public DateTime UploadedAt { get; set; }

    // Əks əlaqələr (Feedback, Tag, və s.)
    public ICollection<Feedback>? Feedbacks { get; set; }
    public ICollection<PhotoAlbumItem>? PhotoAlbumItems { get; set; }
}

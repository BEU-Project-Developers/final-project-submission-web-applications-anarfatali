using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoFolio.Models;

public class Photo
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    public string FileName { get; set; }

    public string PhotographerId { get; set; }
    public ApplicationUser? Photographer { get; set; }
    
    [Required]
    public string ContentType { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public string Url { get; set; } = "";

    public string Title { get; set; } = "";
    public string? Description { get; set; }

    public DateTime UploadedAt { get; set; }
    
    [Required]
    public byte[] Data { get; set; }
    
    public ICollection<PhotoAlbumItem>? PhotoAlbumItems { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace PhotoFolio.ViewModels;

public class PhotoUploadViewModel
{
    [Required(ErrorMessage = "Image is required.")]
    [Display(Name = "Image")]
    public IFormFile File { get; set; }

    [Required(ErrorMessage = "The title cannot be empty.")]
    [StringLength(100, ErrorMessage = "The title cannot be longer than 100 characters.")]
    [Display(Name = "The title")]
    public string Title { get; set; }

    [StringLength(500, ErrorMessage = "The description cannot be longer than 500 characters.")]
    [Display(Name = "The description")]
    public string Description { get; set; }
}

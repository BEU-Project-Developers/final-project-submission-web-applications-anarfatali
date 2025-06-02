using System.ComponentModel.DataAnnotations;

namespace PhotoFolio.ViewModels;

public class PhotographerApplicationViewModel
{
    [Required]
    [Display(Name = "Name")]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Surname")]
    public string Surname { get; set; }

    [Required]
    [Range(18, 100)]
    [Display(Name = "Age")]
    public int Age { get; set; }

    [Required]
    [Display(Name = "Experience (years)")]
    public string Experience { get; set; }

    [Required]
    [Url]
    [Display(Name = "Portfolio URL")]
    public string PortfolioUrl { get; set; }
}

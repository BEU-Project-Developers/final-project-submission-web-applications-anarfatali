using System.ComponentModel.DataAnnotations;

namespace PhotoFolio.ViewModels;

public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
    
    public string Username { get; set; } = "";
    
    public string Fullname { get; set; } = "";

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} chars long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = "";
}

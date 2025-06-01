using System.ComponentModel.DataAnnotations;

namespace PhotoFolio.ViewModels;

public class AccountSettingsViewModel
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = "";
    
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100, ErrorMessage = "Username must be between 2 and 100 characters.")]
    [Display(Name = "Username")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Current password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Current Password")]
    public string CurrentPassword { get; set; } = "";

    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "New password must be at least 6 characters.")]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string? ConfirmPassword { get; set; }

    public bool UpdateSucceeded { get; set; } = false;
}

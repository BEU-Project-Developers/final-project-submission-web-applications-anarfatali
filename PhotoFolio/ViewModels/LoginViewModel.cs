using System.ComponentModel.DataAnnotations;

namespace PhotoFolio.ViewModels;

public class LoginViewModel
{       
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
    
    public string Username { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}

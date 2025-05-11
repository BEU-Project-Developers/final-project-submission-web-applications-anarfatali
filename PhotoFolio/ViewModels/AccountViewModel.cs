using System.ComponentModel.DataAnnotations;

namespace PhotoFolio.ViewModels;

public class AccountViewModel
{
    [Required, EmailAddress] public string Email { get; set; }

    [Required] public string Username { get; set; }

    [Required, DataType(DataType.Password)]
    public string Password { get; set; }

    [Compare("Password"), DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}

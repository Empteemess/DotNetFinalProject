using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models;

public class RecoveryPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [Compare(nameof(Password),ErrorMessage = "Password Doesn't Match")]
    public string ConfirmPassword { get; set; }
    public bool EmailCheck { get; set; }

}
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.DTOs.UserAuth;

public class LoginRequestDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please provide a valid email")]
    public required string Email { get; set; }
    public required string Password { get; set; }
}

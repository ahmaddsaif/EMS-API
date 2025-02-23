using EventManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.DTOs.UserAuth;

public class RegistrationRequestDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
    public string Name { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please provide a valid email")]
    public string Email { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    public string Password { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please provide role")]
    public Role Role { get; set; }
}

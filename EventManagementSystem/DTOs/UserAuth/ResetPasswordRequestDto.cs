using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.DTOs.UserAuth;

public class ResetPasswordRequestDto
{
    [Required]
    public string OldPassword { get; set; }
    [Required]
    public string NewPassword { get; set; }
}

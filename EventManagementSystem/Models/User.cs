using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string Role { get; set; } // Organizer, Participant, Admin
}

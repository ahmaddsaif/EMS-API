using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace EventManagementSystem.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Column(TypeName = "int")]
    public Role Role { get; set; } // Organizer, Participant, Admin
    public bool MustChangePassword { get; set; } = true;

    public ICollection<Event> OrganizedEvents { get; set; }
    public ICollection<RSVP> RSVPs { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public ICollection<AuditLog> AuditLogs { get; set; }
}

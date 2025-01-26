using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models;

public class Notification
{
    [Key]
    public int NotificationId { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int? EventId { get; set; }
    public Event Event { get; set; }
}

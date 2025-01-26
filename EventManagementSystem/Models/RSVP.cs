using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models;

public class RSVP
{
    [Key]
    public int RSVPId { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public string Status { get; set; } // Pending, Confirmed, Declined
    public DateTime RSVPDateTime { get; set; }
}

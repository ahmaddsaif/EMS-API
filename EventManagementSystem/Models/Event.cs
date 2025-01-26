using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementSystem.Models;

public class Event
{
    [Key]
    public int EventId { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    [Required]
    public DateTime Date { get; set; }
    public TimeOnly Time { get; set; }
    public string Location { get; set; }

    [ForeignKey("Organizer")]
    public int OrganizerId { get; set; }
    public User Organizer { get; set; }

    public ICollection<EventCategory> EventCategories { get; set; }
    public ICollection<RSVP> RSVPs { get; set; }
    public ICollection<Notification> Notifications { get; set; }
}

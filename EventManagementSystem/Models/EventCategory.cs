using System.ComponentModel.DataAnnotations.Schema;

namespace EventManagementSystem.Models;

public class EventCategory
{
    [ForeignKey("Event")]
    public int EventId { get; set; }
    public Event Event { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}



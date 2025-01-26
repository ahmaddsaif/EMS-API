using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models;

public class AuditLog
{
    [Key]
    public int LogId { get; set; }
    public int AdminId { get; set; }
    public User Admin { get; set; }
    public string ActionType { get; set; }
    public string ActionDetails { get; set; }
    public DateTime TimeStamp { get; set; }
}

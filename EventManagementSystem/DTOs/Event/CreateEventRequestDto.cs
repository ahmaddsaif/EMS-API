using EventManagementSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace EventManagementSystem.DTOs.Event;

public class CreateEventRequestDto : IValidatableObject
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
    public string Title { get; set; }
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }
    [Required(ErrorMessage = "Date is required.")]
    public string Date { get; set; }
    [Required(ErrorMessage = "Time is required.")]
    public string Time { get; set; }
    [Required(ErrorMessage = "Location is required.")]
    [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
    public string Location { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!DateTime.TryParseExact(Date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            yield return new ValidationResult("Date must be in the format yyyyMMdd.", new[] { nameof(Date) });
        }

        if (!DateTime.TryParseExact(Time, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            yield return new ValidationResult("Time must be in the format HH:mm:ss.", new[] { nameof(Time) });
        }
    }
}

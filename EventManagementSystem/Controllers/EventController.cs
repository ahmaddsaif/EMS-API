using Asp.Versioning;
using EventManagementSystem.Data;
using EventManagementSystem.DTOs.Event;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using Serilog;
using System.Globalization;
using System.Security.Claims;

namespace EventManagementSystem.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{v:apiVersion}/event")]
public class EventController(AppDbContext context, ILogger<EventController> logger, IEmailService emailService) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<EventController> _logger = logger;
    private readonly IEmailService _emailService = emailService;

    [HttpPost("createEvent")]
    [Authorize(Roles = "Organizer")]
    public IActionResult CreateEvent([FromBody] CreateEventRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var organizerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        DateTime parsedDate = DateTime.ParseExact(request.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
        TimeOnly parsedTime = TimeOnly.ParseExact(request.Time, "HH:mm:ss", CultureInfo.InvariantCulture);

        var newEvent = new Event
        {
            Title = request.Title,
            Description = request.Description,
            Date = parsedDate,
            Time = parsedTime,
            Location = request.Location,
            OrganizerId = organizerId
        };

        _context.Events.Add(newEvent);
        _context.SaveChanges();

        Log.Information("Organizer {OrganizerId} created an event: {Title} on {Date} at {Time}", organizerId, newEvent.Title, newEvent.Date, newEvent.Time);
        return CreatedAtAction(nameof(GetEventById), new { eventId = newEvent.EventId }, new { message = "Event created successfully", data = newEvent });
    }

    [HttpGet("{eventId}")]
    [Authorize]
    public IActionResult GetEventById(int eventId)
    {
        if(eventId < 0)
        {
            return BadRequest(new {message = "Please provide a valid event id"});
        }

        Event? eventItem = _context.Events.Include(e => e.Organizer).FirstOrDefault(e => e.EventId == eventId);

        if (eventItem == null)
        {
            return BadRequest(new { message = $"No even found for id {eventId}" });
        }

        var response = new
        {
            eventItem.EventId,
            eventItem.Title,
            eventItem.Description,
            Date = eventItem.Date.ToString("yyyy-MM-dd"),
            Time = eventItem.Time.ToString("HH:mm:ss"),
            eventItem.Location,
            Organizer = eventItem.Organizer.Name
        };

        Log.Information("Retrieved event: {Title} (ID: {EventId})", eventItem.Title, eventId);
        return Ok(new { message = "Event retrieved successfully", data = response });
    }

    [HttpGet("sendTestEmail")]
    public async Task<IActionResult> SendTestEmail()
    {
        RestResponse res = await _emailService.SendEmail("joeydoeyyy@gmail.com");

        Console.WriteLine($"Twilio response: {res.Content}");

        return Ok(res.Content);
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetAllEvents()
    {
        return Ok(new { message = "List of events" });
    }
}

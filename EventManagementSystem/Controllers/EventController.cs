using EventManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EventManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    [HttpPost]
    //[Authorize(Roles = "Organizer")]
    public IActionResult CreateEvent()//[FromBody] Event newEvent)
    {
        try
        {
            Log.Information("Starting Event Management System...");
            throw new Exception("test exc");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "The application failed to start.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
        return Ok(new { message = "Event created successfully" });
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetAllEvents()
    {
        return Ok(new { message = "List of events" });
    }
}

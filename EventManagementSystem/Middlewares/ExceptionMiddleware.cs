using EventManagementSystem.DTOs;
using Newtonsoft.Json;

namespace EventManagementSystem.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ex switch
        {
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        var traceId = System.Diagnostics.Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;

        var response = new ResponseDto<string>
        {
            Status = new Status
            {
                Code = 0,
                Message = context.Response.StatusCode == StatusCodes.Status500InternalServerError ? "An error occurred at server" : ex.Message
            },
            Data = traceId
        };

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}

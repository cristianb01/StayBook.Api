using System.Net;
using System.Text.Json;
using StayBook.Application.Exceptions;

namespace StayBook.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            BookingOverlayException => (HttpStatusCode.Conflict, exception.Message),
            BookingNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            InvalidBookingStatusException => (HttpStatusCode.BadRequest, exception.Message),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            PaymentFailedException => (HttpStatusCode.PaymentRequired, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };
        
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)statusCode;
        
        var body = JsonSerializer.Serialize(new { error = message });
        return httpContext.Response.WriteAsync(body);
    }
}
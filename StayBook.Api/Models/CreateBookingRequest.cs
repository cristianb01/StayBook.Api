namespace StayBook.Api.Models;

public record CreateBookingRequest(
    int PropertyId,
    DateTime StartDate,
    DateTime EndDate);


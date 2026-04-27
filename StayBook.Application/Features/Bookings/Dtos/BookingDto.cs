using StayBook.Domain.Enums;

namespace StayBook.Application.Features.Bookings.DTOs;

public record BookingDto(
    int Id,
    int UserId,
    int PropertyId,
    BookingStatus Status,
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalPrice,
    DateTime CreatedAt);
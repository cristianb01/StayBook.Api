using StayBook.Domain.Enums;

namespace StayBook.Application.Exceptions;

public class InvalidBookingStatusException(int bookingId, BookingStatus currentStatus, BookingStatus expectedStatus)
    : Exception($"Booking {bookingId} cannot be processed: expected status '{expectedStatus}', but was '{currentStatus}'.");


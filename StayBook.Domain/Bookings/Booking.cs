using StayBook.Domain.Bookings.Events;
using StayBook.Domain.Common;
using StayBook.Domain.Enums;
using StayBook.Domain.ValueObjects;

namespace StayBook.Domain.Bookings;

public class Booking : AggregateRoot
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public int PropertyId { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateRange DateRange { get; private set; }
    public decimal TotalPrice { get; private set; }
    public string? PaymentReferenceId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    // TODO: Initialize ExpiresAt when a booking is created.
    public DateTime ExpiresAt { get; private set; }

    // Required by EF Core
    private Booking() { }

    public Booking(int userId, int propertyId, DateRange dateRange, decimal totalPrice)
    {
        UserId = userId;
        PropertyId = propertyId;
        DateRange = dateRange;
        Status = BookingStatus.Pending;
        TotalPrice = totalPrice;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = CreatedAt.AddMinutes(10);
    }

    public void Confirm(string paymentReferenceId)
    {
        if (Status != BookingStatus.Pending)
        {
            throw new InvalidOperationException("Only pending bookings can be confirmed.");
        }
        
        Status = BookingStatus.Confirmed;
        PaymentReferenceId = paymentReferenceId;
        
        AddDomainEvent(new BookingConfirmedEvent(Id));
    }

    public void Cancel()
    {
        if (Status == BookingStatus.Cancelled)
        {
            throw new InvalidOperationException("Booking is already cancelled.");
        }
        
        Status = BookingStatus.Cancelled;
    }

    public void Expire()
    {
        if (Status != BookingStatus.Pending)
        {
            throw new InvalidOperationException("Only pending bookings can expire.");
        }
        
        Status = BookingStatus.Expired;
    }
    
    public bool IsActive() => Status == BookingStatus.Pending || Status == BookingStatus.Confirmed;
}
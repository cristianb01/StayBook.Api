using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Bookings.Commands;
using StayBook.Application.Interfaces;
using StayBook.Domain.Bookings;
using StayBook.Domain.ValueObjects;

namespace StayBook.Application.Features.Bookings.Handlers;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, int>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMySqlPropertyLock _propertyLock;

    public CreateBookingCommandHandler(IBookingRepository bookingRepository, IMySqlPropertyLock propertyLock, IPropertyRepository propertyRepository)
    {
        _bookingRepository = bookingRepository;
        _propertyLock = propertyLock;
        _propertyRepository = propertyRepository;
    }
    
    public async Task<int> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdAsync(request.PropertyId, cancellationToken);
        
        if (property is null)
            throw new PropertyNotFoundException("Specified property not found");
        
        await _propertyLock.ExecuteAsync(request.PropertyId, cancellationToken);

        var hasOverlap = await _bookingRepository.HasOverlapAsync(request.PropertyId, request.StartDate,
            request.EndDate, cancellationToken);
        if (hasOverlap)
        { 
            throw new BookingOverlayException();
        }

        var booking = new Booking(request.GuestId, property.HostId, request.PropertyId,
            new DateRange(request.StartDate, request.EndDate), 0);

        await _bookingRepository.AddAsync(booking, cancellationToken);

        return booking.Id;
    }
}
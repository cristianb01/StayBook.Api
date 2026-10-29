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

    public CreateBookingCommandHandler(IBookingRepository bookingRepository, IPropertyRepository propertyRepository)
    {
        _bookingRepository = bookingRepository;
        _propertyRepository = propertyRepository;
    }
    
    public async Task<int> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdForUpdateAsync(request.PropertyId, cancellationToken);
        
        if (property is null)
            throw new ResourceNotFoundException("Specified property not found");

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
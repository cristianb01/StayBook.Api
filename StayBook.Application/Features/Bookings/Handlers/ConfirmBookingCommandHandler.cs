using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Bookings.Commands;
using StayBook.Application.Interfaces;
using StayBook.Application.Models;
using StayBook.Domain.Enums;

namespace StayBook.Application.Features.Bookings.Handlers;

public class ConfirmBookingCommandHandler : IRequestHandler<ConfirmBookingCommand, Unit>
{
    private readonly IBookingRepository  _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public ConfirmBookingCommandHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Unit> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var existingBooking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

        if (existingBooking == null)
        {
            throw new BookingNotFoundException(request.BookingId);
        }

        if (existingBooking.Status != BookingStatus.Pending)
        {
            throw new InvalidBookingStatusException(request.BookingId, existingBooking.Status, BookingStatus.Pending);
        }

        existingBooking.Confirm();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
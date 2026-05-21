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
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWork _unitOfWork;
    
    public ConfirmBookingCommandHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork, IPaymentService paymentService)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
    }
    
    public async Task<Unit> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var existingBooking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

        if (existingBooking == null)
        {
            throw new BookingNotFoundException(request.BookingId);
        }

        if (existingBooking.Status == BookingStatus.Confirmed)
        {
            return Unit.Value;
        }
        
        if (existingBooking.Status != BookingStatus.Pending)
        {
            throw new InvalidBookingStatusException(request.BookingId, existingBooking.Status, BookingStatus.Pending);
        }
        
        var paymentResult = await _paymentService.VerifyAsync(request.PaymentReferenceId, cancellationToken);
        
        if (!paymentResult.IsSuccess) throw new PaymentFailedException();

        existingBooking.Confirm();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
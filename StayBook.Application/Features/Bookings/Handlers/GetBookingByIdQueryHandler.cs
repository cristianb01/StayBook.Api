using AutoMapper;
using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Bookings.DTOs;
using StayBook.Application.Features.Bookings.Queries;
using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Bookings.Handlers;

public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingDto>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public GetBookingByIdQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    public async Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if  (booking == null) throw new ResourceNotFoundException("Booking", request.Id);
        
        return _mapper.Map<BookingDto>(booking);
    }
}
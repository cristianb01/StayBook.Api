using AutoMapper;
using MediatR;
using StayBook.Application.Features.Bookings.DTOs;
using StayBook.Application.Features.Bookings.Queries;
using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Bookings.Handlers;

public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, List<BookingDto>>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public GetAllBookingsQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    public async Task<List<BookingDto>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetAllAsync(request.Filters, cancellationToken);
        return _mapper.Map<List<BookingDto>>(bookings);
    }
}
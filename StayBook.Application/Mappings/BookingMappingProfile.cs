using AutoMapper;
using StayBook.Application.Features.Bookings.DTOs;
using StayBook.Domain.Bookings;

namespace StayBook.Application.Mappings;

public class BookingMappingProfile : Profile
{
    public BookingMappingProfile()
    {
        CreateMap<Booking, BookingDto>()
            .ConstructUsing(src => new BookingDto(
                src.Id,
                src.GuestId,
                src.PropertyId,
                src.Status,
                src.DateRange.StartDate,
                src.DateRange.EndDate,
                src.TotalPrice,
                src.CreatedAt,
                src.ExpiresAt));
    }
}
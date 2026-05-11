namespace StayBook.Application.Features.Properties.Dtos;

public record PropertyDto(
    int Id,
    string Name,
    string Description,
    int HostId);
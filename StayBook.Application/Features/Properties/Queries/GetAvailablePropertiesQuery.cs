using MediatR;
using StayBook.Application.Features.Properties.Dtos;

namespace StayBook.Application.Features.Properties.Queries;

public record GetAvailablePropertiesQuery(DateTime StartDate, DateTime EndDate) : IRequest<List<PropertyDto>>;
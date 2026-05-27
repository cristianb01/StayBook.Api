using MediatR;
using StayBook.Application.Features.Properties.Dtos;

namespace StayBook.Application.Features.Properties.Queries;

public record GetPropertyByIdQuery(int PropertyId) : IRequest<PropertyDto>;
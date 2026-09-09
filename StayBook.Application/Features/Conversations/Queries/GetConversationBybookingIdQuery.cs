using MediatR;
using StayBook.Application.Features.Bookings.DTOs;
using StayBook.Application.Features.Conversations.Dtos;

namespace StayBook.Application.Features.Conversations.Queries;

public record GetConversationBybookingIdQuery(int BookingId, int UserId) : IRequest<ConversationDto>;
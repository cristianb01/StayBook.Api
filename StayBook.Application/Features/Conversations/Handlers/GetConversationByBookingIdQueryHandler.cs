using AutoMapper;
using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Bookings.DTOs;
using StayBook.Application.Features.Conversations.Queries;
using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Conversations.Handlers;

public class GetConversationByBookingIdQueryHandler : IRequestHandler<GetConversationBybookingIdQuery, ConversationDto>
{
    private readonly IConversationRepository _repository;
    private readonly IMapper _mapper;

    public GetConversationByBookingIdQueryHandler(IConversationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ConversationDto> Handle(GetConversationBybookingIdQuery request, CancellationToken cancellationToken)
    {
        var conversation = await _repository.GetByBookingIdAsync(request.Id, cancellationToken);
        
        if (conversation is null)
            throw new ResourceNotFoundException("Conversation", request.Id);
        
        return _mapper.Map<ConversationDto>(conversation);
    }
}
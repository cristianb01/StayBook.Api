using AutoMapper;
using MediatR;
using StayBook.Application.Exceptions;
using StayBook.Application.Features.Bookings.DTOs;
using StayBook.Application.Features.Conversations.Queries;
using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Conversations.Handlers;

public class GetConversationByIdQueryHandler : IRequestHandler<GetConversationByIdQuery, ConversationDto>
{
    private readonly IConversationRepository _repository;
    private readonly IMapper _mapper;

    public GetConversationByIdQueryHandler(IConversationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ConversationDto> Handle(GetConversationByIdQuery request, CancellationToken cancellationToken)
    {
        var conversation = await _repository.GetByIdAsync(request.Id, cancellationToken);
        
        if (conversation is null)
            throw new ResourceNotFoundException("Conversation", request.Id);
        
        return _mapper.Map<ConversationDto>(conversation);
    }
}
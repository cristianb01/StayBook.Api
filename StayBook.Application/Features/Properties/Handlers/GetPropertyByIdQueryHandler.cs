using AutoMapper;
using MediatR;
using StayBook.Application.Features.Properties.Dtos;
using StayBook.Application.Features.Properties.Queries;
using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Properties.Handlers;

public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDto>
{
    private readonly IPropertyRepository _repository;
    private readonly IMapper _mapper;

    public GetPropertyByIdQueryHandler(IPropertyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PropertyDto> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
    {
        var existingProperty = await _repository.GetByIdAsync(request.PropertyId, cancellationToken);

        if (existingProperty is null)
        {
            throw new KeyNotFoundException($"Property with id: {request.PropertyId} not found");
        }
        
        return  _mapper.Map<PropertyDto>(existingProperty);
    }
}
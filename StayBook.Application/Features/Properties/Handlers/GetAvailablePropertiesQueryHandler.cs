using AutoMapper;
using MediatR;
using StayBook.Application.Features.Properties.Dtos;
using StayBook.Application.Features.Properties.Queries;
using StayBook.Application.Interfaces;

namespace StayBook.Application.Features.Properties.Handlers;

public class GetAvailablePropertiesQueryHandler : IRequestHandler<GetAvailablePropertiesQuery, List<PropertyDto>>
{
    private readonly IPropertyRepository _repository;
    private readonly IMapper _mapper;

    public GetAvailablePropertiesQueryHandler(IPropertyRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<PropertyDto>> Handle(GetAvailablePropertiesQuery request, CancellationToken cancellationToken)
    {
        var availableProperties =
            await _repository.GetAvailableProperties(request.StartDate, request.EndDate, cancellationToken);
        
        return _mapper.Map<List<PropertyDto>>(availableProperties);
    }
}
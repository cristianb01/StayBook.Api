using AutoMapper;
using MediatR;
using StayBook.Application.Features.Properties.Dtos;
using StayBook.Application.Features.Properties.Queries;
using StayBook.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace StayBook.Application.Features.Properties.Handlers;

public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDto>
{
    private readonly IPropertyRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    public GetPropertyByIdQueryHandler(IPropertyRepository repository, IMapper mapper, IMemoryCache cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<PropertyDto> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(request.PropertyId, out PropertyDto? cachedProperty)) return cachedProperty!;
        
        var existingProperty = await _repository.GetByIdAsync(request.PropertyId, cancellationToken);

        if (existingProperty is null)
        {
            throw new KeyNotFoundException($"Property with id: {request.PropertyId} not found");
        }
        
        var dto = _mapper.Map<PropertyDto>(existingProperty);
        
        _cache.Set(
            request.PropertyId,
            dto,
            new MemoryCacheEntryOptions {AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)}
            );
        
        return dto;
    }
}
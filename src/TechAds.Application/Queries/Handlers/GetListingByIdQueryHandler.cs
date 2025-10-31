using MediatR;
using TechAds.Domain.Interfaces;
using TechAds.Application.DTOs;

namespace TechAds.Application.Queries.Handlers;

public class GetListingByIdQueryHandler : IRequestHandler<GetListingByIdQuery, ListingDto>
{
    private readonly IProjectListingRepository _repository;

    public GetListingByIdQueryHandler(IProjectListingRepository repository)
    {
        _repository = repository;
    }

    public async Task<ListingDto> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
    {
        var listing = await _repository.GetByIdAsync(request.Id);
        if (listing == null) return null;
        return new ListingDto
        {
            Id = listing.Id,
            Title = listing.Title,
            ShortDescription = listing.ShortDescription,
            Requirements = listing.Requirements,
            Tags = listing.Tags.Select(t => t.Value).ToList(),
            CreatedByUserId = listing.CreatedByUserId,
            Status = listing.Status,
            CreatedAt = listing.CreatedAt
        };
    }
}
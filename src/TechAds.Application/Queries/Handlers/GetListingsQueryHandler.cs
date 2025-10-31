using MediatR;
using TechAds.Domain.Interfaces;
using TechAds.Application.DTOs;

namespace TechAds.Application.Queries.Handlers;

public class GetListingsQueryHandler : IRequestHandler<GetListingsQuery, List<ListingDto>>
{
    private readonly IProjectListingRepository _repository;

    public GetListingsQueryHandler(IProjectListingRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ListingDto>> Handle(GetListingsQuery request, CancellationToken cancellationToken)
    {
        var listings = await _repository.GetAllAsync();
        return listings.Select(l => new ListingDto
        {
            Id = l.Id,
            Title = l.Title,
            ShortDescription = l.ShortDescription,
            Requirements = l.Requirements,
            Tags = l.Tags.Select(t => t.Value).ToList(),
            CreatedByUserId = l.CreatedByUserId,
            Status = l.Status,
            CreatedAt = l.CreatedAt
        }).ToList();
    }
}
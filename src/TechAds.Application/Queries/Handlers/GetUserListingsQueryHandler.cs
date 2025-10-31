using MediatR;
using TechAds.Domain.Interfaces;
using TechAds.Application.DTOs;

namespace TechAds.Application.Queries.Handlers;

public class GetUserListingsQueryHandler : IRequestHandler<GetUserListingsQuery, List<ListingDto>>
{
    private readonly IProjectListingRepository _repository;

    public GetUserListingsQueryHandler(IProjectListingRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ListingDto>> Handle(GetUserListingsQuery request, CancellationToken cancellationToken)
    {
        var listings = await _repository.GetAllAsync();
        var userListings = listings.Where(l => l.CreatedByUserId == request.UserId);
        return userListings.Select(l => new ListingDto
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
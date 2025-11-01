using MediatR;
using TechAds.Application.DTOs;

namespace TechAds.Application.Queries;

public class GetListingByIdQuery : IRequest<ListingDto>
{
    public Guid Id { get; }

    public GetListingByIdQuery(Guid id)
    {
        Id = id;
    }
}
using MediatR;
using TechAds.Application.DTOs;

namespace TechAds.Application.Queries;

public class GetUserListingsQuery : IRequest<List<ListingDto>>
{
    public Guid UserId { get; }

    public GetUserListingsQuery(Guid userId)
    {
        UserId = userId;
    }
}
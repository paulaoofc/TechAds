using MediatR;
using TechAds.Application.DTOs;

namespace TechAds.Application.Queries;

public class GetListingsQuery : IRequest<List<ListingDto>>
{
}
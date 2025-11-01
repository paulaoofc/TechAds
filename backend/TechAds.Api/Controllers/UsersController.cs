using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using TechAds.Application.Queries;

namespace TechAds.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id}/listings")]
    [Authorize]
    public async Task<IActionResult> GetUserListings(Guid id)
    {
        var query = new GetUserListingsQuery(id);
        var listings = await _mediator.Send(query);
        return Ok(listings);
    }
}
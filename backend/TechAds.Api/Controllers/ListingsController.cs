using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using TechAds.Application.Queries;
using TechAds.Application.Commands;
using TechAds.Api.DTOs;
using System.Security.Claims;
using TechAds.Application.Common;

namespace TechAds.Api.Controllers;

[ApiController]
[Route("api/listings")]
public class ListingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ListingsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateListingRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var command = new CreateListingCommand(request.Title, request.ShortDescription, request.Requirements, request.Tags, userId);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        // Get the created listing
        var query = new GetListingByIdQuery(result.Value);
        var listing = await _mediator.Send(query);

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, listing);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetListingsQuery();
        var listings = await _mediator.Send(query);
        return Ok(listings);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetListingByIdQuery(id);
        var listing = await _mediator.Send(query);
        if (listing == null) return NotFound();
        return Ok(listing);
    }

    [HttpPost("{id}/applications")]
    [Authorize]
    public async Task<IActionResult> SubmitApplication(Guid id, [FromBody] SubmitApplicationRequest request)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var command = new SubmitApplicationCommand(id, userId, request.Message);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Created("", new { Id = result.Value });
    }
}
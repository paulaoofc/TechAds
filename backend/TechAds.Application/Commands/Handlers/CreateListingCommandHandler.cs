using MediatR;
using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using TechAds.Domain.ValueObjects;
using TechAds.Domain.Enums;

namespace TechAds.Application.Commands.Handlers;

public class CreateListingCommandHandler : IRequestHandler<CreateListingCommand, Guid>
{
    private readonly IProjectListingRepository _repository;

    public CreateListingCommandHandler(IProjectListingRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var tags = request.Tags.Select(t => new Tag(t)).ToList();
        var listing = new ProjectListing(id, request.Title, request.ShortDescription, request.Requirements, tags, request.CreatedByUserId, ListingStatus.Draft, DateTime.UtcNow);
        await _repository.AddAsync(listing);
        return id;
    }
}
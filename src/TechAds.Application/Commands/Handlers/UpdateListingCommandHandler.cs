using MediatR;
using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using TechAds.Domain.ValueObjects;

namespace TechAds.Application.Commands.Handlers;

public class UpdateListingCommandHandler : IRequestHandler<UpdateListingCommand, Unit>
{
    private readonly IProjectListingRepository _repository;

    public UpdateListingCommandHandler(IProjectListingRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateListingCommand request, CancellationToken cancellationToken)
    {
        var listing = await _repository.GetByIdAsync(request.Id);
        if (listing == null) throw new Exception("Not found");
        listing.Title = request.Title;
        listing.ShortDescription = request.ShortDescription;
        listing.Requirements = request.Requirements;
        listing.Tags = request.Tags.Select(t => new Tag(t)).ToList();
        await _repository.UpdateAsync(listing);
        return Unit.Value;
    }
}
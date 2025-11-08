using MediatR;
using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using TechAds.Domain.ValueObjects;
using TechAds.Domain.Enums;
using TechAds.Application.Common;
using FluentValidation;

namespace TechAds.Application.Commands.Handlers;

public class CreateListingCommandHandler : IRequestHandler<CreateListingCommand, Result<Guid>>
{
    private readonly IProjectListingRepository _repository;
    private readonly IValidator<CreateListingCommand> _validator;

    public CreateListingCommandHandler(IProjectListingRepository repository, IValidator<CreateListingCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid>> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate using FluentValidation
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<Guid>.Failure(errorMessage);
            }

            var id = Guid.NewGuid();
            var tags = request.Tags.Select(t => new Tag(t)).ToList();
            var listing = new ProjectListing(id, request.Title, request.ShortDescription, request.Requirements, tags, request.CreatedByUserId, ListingStatus.Draft, DateTime.UtcNow);

            await _repository.AddAsync(listing);
            return Result<Guid>.Success(id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(ex);
        }
    }
}
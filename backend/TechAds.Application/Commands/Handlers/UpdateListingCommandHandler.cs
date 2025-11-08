using MediatR;
using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using TechAds.Domain.ValueObjects;
using TechAds.Application.Common;
using FluentValidation;

namespace TechAds.Application.Commands.Handlers;

public class UpdateListingCommandHandler : IRequestHandler<UpdateListingCommand, Result>
{
    private readonly IProjectListingRepository _repository;
    private readonly IValidator<UpdateListingCommand> _validator;

    public UpdateListingCommandHandler(IProjectListingRepository repository, IValidator<UpdateListingCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result> Handle(UpdateListingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate using FluentValidation
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Failure(errorMessage);
            }

            var listing = await _repository.GetByIdAsync(request.Id);
            if (listing == null)
                return Result.Failure("Listing not found");

            listing.Title = request.Title;
            listing.ShortDescription = request.ShortDescription;
            listing.Requirements = request.Requirements;
            listing.Tags = request.Tags.Select(t => new Tag(t)).ToList();
            await _repository.UpdateAsync(listing);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex);
        }
    }
}
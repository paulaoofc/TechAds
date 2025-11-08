using MediatR;
using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using TechAds.Domain.Enums;
using TechAds.Application.Common;
using FluentValidation;

namespace TechAds.Application.Commands.Handlers;

public class SubmitApplicationCommandHandler : IRequestHandler<SubmitApplicationCommand, Result<Guid>>
{
    private readonly IApplicationRepository _repository;
    private readonly IValidator<SubmitApplicationCommand> _validator;

    public SubmitApplicationCommandHandler(IApplicationRepository repository, IValidator<SubmitApplicationCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid>> Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
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
            var application = new TechAds.Domain.Entities.Application(id, request.ProjectListingId, request.CandidateUserId, request.Message, ApplicationStatus.Pending, DateTime.UtcNow);
            await _repository.AddAsync(application);
            return Result<Guid>.Success(id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(ex);
        }
    }
}
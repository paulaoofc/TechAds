using FluentValidation;
using TechAds.Application.Commands;

namespace TechAds.Application.Validators;

public class SubmitApplicationCommandValidator : AbstractValidator<SubmitApplicationCommand>
{
    public SubmitApplicationCommandValidator()
    {
        RuleFor(x => x.ProjectListingId)
            .NotEmpty().WithMessage("Project listing ID is required");

        RuleFor(x => x.CandidateUserId)
            .NotEmpty().WithMessage("Candidate user ID is required");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required")
            .MinimumLength(10).WithMessage("Message must be at least 10 characters long")
            .MaximumLength(1000).WithMessage("Message must not exceed 1000 characters");
    }
}
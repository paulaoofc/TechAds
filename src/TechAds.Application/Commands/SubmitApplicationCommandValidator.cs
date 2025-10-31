using FluentValidation;

namespace TechAds.Application.Commands;

public class SubmitApplicationCommandValidator : AbstractValidator<SubmitApplicationCommand>
{
    public SubmitApplicationCommandValidator()
    {
        RuleFor(x => x.ProjectListingId).NotEmpty();
        RuleFor(x => x.CandidateUserId).NotEmpty();
        RuleFor(x => x.Message).NotEmpty().MaximumLength(1000);
    }
}
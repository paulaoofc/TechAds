using FluentValidation;

namespace TechAds.Application.Commands;

public class CreateListingCommandValidator : AbstractValidator<CreateListingCommand>
{
    public CreateListingCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ShortDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Requirements).NotEmpty();
        RuleFor(x => x.Tags).NotNull();
        RuleFor(x => x.CreatedByUserId).NotEmpty();
    }
}
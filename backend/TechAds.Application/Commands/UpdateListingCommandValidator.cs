using FluentValidation;

namespace TechAds.Application.Commands;

public class UpdateListingCommandValidator : AbstractValidator<UpdateListingCommand>
{
    public UpdateListingCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ShortDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Requirements).NotEmpty();
        RuleFor(x => x.Tags).NotNull();
        RuleFor(x => x.UpdatedByUserId).NotEmpty();
    }
}
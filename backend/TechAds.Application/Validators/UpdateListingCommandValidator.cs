using FluentValidation;
using TechAds.Application.Commands;

namespace TechAds.Application.Validators;

public class UpdateListingCommandValidator : AbstractValidator<UpdateListingCommand>
{
    public UpdateListingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Listing ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.ShortDescription)
            .NotEmpty().WithMessage("Short description is required")
            .MaximumLength(500).WithMessage("Short description must not exceed 500 characters");

        RuleFor(x => x.Requirements)
            .NotEmpty().WithMessage("Requirements are required")
            .MaximumLength(2000).WithMessage("Requirements must not exceed 2000 characters");

        RuleFor(x => x.Tags)
            .NotEmpty().WithMessage("At least one tag is required")
            .Must(tags => tags.Count <= 10).WithMessage("Maximum 10 tags allowed")
            .ForEach(tag => tag.Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Tags can only contain letters, numbers, underscores and hyphens"));

        RuleFor(x => x.UpdatedByUserId)
            .NotEmpty().WithMessage("Updated by user ID is required");
    }
}
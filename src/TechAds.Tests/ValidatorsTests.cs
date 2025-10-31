using Xunit;
using FluentAssertions;
using TechAds.Application.Commands;
using FluentValidation.TestHelper;

public class ValidatorsTests
{
    [Fact]
    public void CreateListingCommandValidator_ValidCommand_ShouldNotHaveValidationErrors()
    {
        var validator = new CreateListingCommandValidator();
        var command = new CreateListingCommand("Title", "Short desc", "Req", new List<string> { "tag" }, Guid.NewGuid());
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateListingCommandValidator_InvalidCommand_ShouldHaveValidationErrors()
    {
        var validator = new CreateListingCommandValidator();
        var command = new CreateListingCommand("", "", "", null, Guid.Empty);
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.ShortDescription);
        result.ShouldHaveValidationErrorFor(x => x.Requirements);
        result.ShouldHaveValidationErrorFor(x => x.Tags);
        result.ShouldHaveValidationErrorFor(x => x.CreatedByUserId);
    }
}
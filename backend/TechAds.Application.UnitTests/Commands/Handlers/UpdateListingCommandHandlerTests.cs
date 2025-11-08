using Xunit;
using Moq;
using FluentAssertions;
using TechAds.Application.Commands.Handlers;
using TechAds.Application.Commands;
using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using TechAds.Domain.ValueObjects;
using TechAds.Domain.Enums;
using TechAds.Application.Common;
using FluentValidation;
using FluentValidation.Results;

namespace TechAds.Application.UnitTests.Commands.Handlers;

public class UpdateListingCommandHandlerTests
{
    private readonly Mock<IProjectListingRepository> _repositoryMock;
    private readonly Mock<IValidator<UpdateListingCommand>> _validatorMock;
    private readonly UpdateListingCommandHandler _handler;

    public UpdateListingCommandHandlerTests()
    {
        _repositoryMock = new Mock<IProjectListingRepository>();
        _validatorMock = new Mock<IValidator<UpdateListingCommand>>();
        _handler = new UpdateListingCommandHandler(_repositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var existingListing = new ProjectListing(
            listingId,
            "Original Title",
            "Original Description",
            "Original Requirements",
            new List<Tag> { new Tag("original") },
            Guid.NewGuid(),
            ListingStatus.Published,
            DateTime.UtcNow
        );

        var command = new UpdateListingCommand(
            listingId,
            "Updated Title",
            "Updated Description",
            "Updated Requirements",
            new List<string> { "csharp", "aspnet" },
            Guid.NewGuid()
        );

        _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(x => x.GetByIdAsync(listingId))
            .ReturnsAsync(existingListing);
        _repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<ProjectListing>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(x => x.GetByIdAsync(listingId), Times.Once);
        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProjectListing>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ListingNotFound_ReturnsFailureResult()
    {
        // Arrange
        var command = new UpdateListingCommand(
            Guid.NewGuid(),
            "Updated Title",
            "Updated Description",
            "Updated Requirements",
            new List<string> { "csharp", "aspnet" },
            Guid.NewGuid()
        );

        _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(x => x.GetByIdAsync(command.Id))
            .ReturnsAsync((ProjectListing?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Listing not found");
        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProjectListing>()), Times.Never);
    }

    [Fact]
    public async Task Handle_EmptyTitle_ReturnsFailureResult()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var existingListing = new ProjectListing(
            listingId,
            "Original Title",
            "Original Description",
            "Original Requirements",
            new List<Tag> { new Tag("original") },
            Guid.NewGuid(),
            ListingStatus.Published,
            DateTime.UtcNow
        );

        var command = new UpdateListingCommand(
            listingId,
            "",
            "Updated Description",
            "Updated Requirements",
            new List<string> { "csharp", "aspnet" },
            Guid.NewGuid()
        );

        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Title", "Title is required")
        };
        _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Title is required");
        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProjectListing>()), Times.Never);
    }

    [Fact]
    public async Task Handle_EmptyShortDescription_ReturnsFailureResult()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var existingListing = new ProjectListing(
            listingId,
            "Original Title",
            "Original Description",
            "Original Requirements",
            new List<Tag> { new Tag("original") },
            Guid.NewGuid(),
            ListingStatus.Published,
            DateTime.UtcNow
        );

        var command = new UpdateListingCommand(
            listingId,
            "Updated Title",
            "",
            "Updated Requirements",
            new List<string> { "csharp", "aspnet" },
            Guid.NewGuid()
        );

        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("ShortDescription", "Short description is required")
        };
        _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Short description is required");
        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProjectListing>()), Times.Never);
    }

    [Fact]
    public async Task Handle_InvalidTags_ReturnsFailureResult()
    {
        // Arrange
        var listingId = Guid.NewGuid();
        var existingListing = new ProjectListing(
            listingId,
            "Original Title",
            "Original Description",
            "Original Requirements",
            new List<Tag> { new Tag("original") },
            Guid.NewGuid(),
            ListingStatus.Published,
            DateTime.UtcNow
        );

        var command = new UpdateListingCommand(
            listingId,
            "Updated Title",
            "Updated Description",
            "Updated Requirements",
            new List<string> { "C#", "ASP.NET" }, // Invalid tags
            Guid.NewGuid()
        );

        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Tags", "Tags can only contain letters, numbers, underscores and hyphens")
        };
        _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Tags can only contain letters, numbers, underscores and hyphens");
        _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProjectListing>()), Times.Never);
    }
}
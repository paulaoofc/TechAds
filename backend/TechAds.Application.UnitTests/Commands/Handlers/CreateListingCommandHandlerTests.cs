using Xunit;
using Moq;
using FluentAssertions;
using TechAds.Application.Commands.Handlers;
using TechAds.Application.Commands;
using TechAds.Domain.Interfaces;
using TechAds.Application.Common;
using FluentValidation;
using FluentValidation.Results;

namespace TechAds.Application.UnitTests.Commands.Handlers;

public class CreateListingCommandHandlerTests
{
    private readonly Mock<IProjectListingRepository> _repositoryMock;
    private readonly Mock<IValidator<CreateListingCommand>> _validatorMock;
    private readonly CreateListingCommandHandler _handler;

    public CreateListingCommandHandlerTests()
    {
        _repositoryMock = new Mock<IProjectListingRepository>();
        _validatorMock = new Mock<IValidator<CreateListingCommand>>();
        _handler = new CreateListingCommandHandler(_repositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        var command = new CreateListingCommand(
            "Senior Developer",
            "Great opportunity",
            "5+ years experience",
            new List<string> { "csharp", "aspnet" },
            Guid.NewGuid()
        );

        _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<TechAds.Domain.Entities.ProjectListing>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<TechAds.Domain.Entities.ProjectListing>()), Times.Once);
    }

    [Fact]
    public async Task Handle_EmptyTitle_ReturnsFailureResult()
    {
        // Arrange
        var command = new CreateListingCommand(
            "",
            "Great opportunity",
            "5+ years experience",
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
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<TechAds.Domain.Entities.ProjectListing>()), Times.Never);
    }

    [Fact]
    public async Task Handle_EmptyUserId_ReturnsFailureResult()
    {
        // Arrange
        var command = new CreateListingCommand(
            "Senior Developer",
            "Great opportunity",
            "5+ years experience",
            new List<string> { "csharp", "aspnet" },
            Guid.Empty
        );

        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("UserId", "User ID is required")
        };
        _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("User ID is required");
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<TechAds.Domain.Entities.ProjectListing>()), Times.Never);
    }

    [Fact]
    public async Task Handle_InvalidTags_ReturnsFailureResult()
    {
        // Arrange
        var command = new CreateListingCommand(
            "Senior Developer",
            "Great opportunity",
            "5+ years experience",
            new List<string> { "C#", "ASP.NET" }, // Invalid tags with special characters
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
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<TechAds.Domain.Entities.ProjectListing>()), Times.Never);
    }
}
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

public class SubmitApplicationCommandHandlerTests
{
    private readonly Mock<IApplicationRepository> _repositoryMock;
    private readonly Mock<IValidator<SubmitApplicationCommand>> _validatorMock;
    private readonly SubmitApplicationCommandHandler _handler;

    public SubmitApplicationCommandHandlerTests()
    {
        _repositoryMock = new Mock<IApplicationRepository>();
        _validatorMock = new Mock<IValidator<SubmitApplicationCommand>>();
        _handler = new SubmitApplicationCommandHandler(_repositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        var command = new SubmitApplicationCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "I am very interested in this project"
        );

        _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<TechAds.Domain.Entities.Application>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<TechAds.Domain.Entities.Application>()), Times.Once);
    }

    [Fact]
    public async Task Handle_EmptyProjectListingId_ReturnsFailureResult()
    {
        // Arrange
        var command = new SubmitApplicationCommand(
            Guid.Empty,
            Guid.NewGuid(),
            "I am very interested in this project"
        );

        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("ProjectListingId", "Project listing ID is required")
        };
        _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Project listing ID is required");
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<TechAds.Domain.Entities.Application>()), Times.Never);
    }

    [Fact]
    public async Task Handle_EmptyCandidateUserId_ReturnsFailureResult()
    {
        // Arrange
        var command = new SubmitApplicationCommand(
            Guid.NewGuid(),
            Guid.Empty,
            "I am very interested in this project"
        );

        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("CandidateUserId", "Candidate user ID is required")
        };
        _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Candidate user ID is required");
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<TechAds.Domain.Entities.Application>()), Times.Never);
    }

    [Fact]
    public async Task Handle_EmptyMessage_ReturnsFailureResult()
    {
        // Arrange
        var command = new SubmitApplicationCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            ""
        );

        var validationErrors = new List<ValidationFailure>
        {
            new ValidationFailure("Message", "Message is required")
        };
        _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationErrors));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Message is required");
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<TechAds.Domain.Entities.Application>()), Times.Never);
    }
}
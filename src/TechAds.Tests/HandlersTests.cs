using Xunit;
using Moq;
using FluentAssertions;
using TechAds.Application.Commands;
using TechAds.Application.Commands.Handlers;
using TechAds.Application.Queries;
using TechAds.Application.Queries.Handlers;
using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using TechAds.Domain.ValueObjects;
using TechAds.Domain.Enums;
using TechAds.Application.Interfaces;
using TechAds.Application.DTOs;

public class HandlersTests
{
    [Fact]
    public async Task CreateListingCommandHandler_Handle_ShouldAddListing()
    {
        var mockRepo = new Mock<IProjectListingRepository>();
        mockRepo.Setup(r => r.AddAsync(It.IsAny<ProjectListing>())).Returns(Task.CompletedTask);
        var handler = new CreateListingCommandHandler(mockRepo.Object);
        var command = new CreateListingCommand("Title", "Desc", "Req", new List<string> { "tag" }, Guid.NewGuid());
        var result = await handler.Handle(command, CancellationToken.None);
        result.Should().NotBeEmpty();
        mockRepo.Verify(r => r.AddAsync(It.Is<ProjectListing>(l => l.Title == "Title")), Times.Once);
    }

    [Fact]
    public async Task AuthenticateUserCommandHandler_ValidCredentials_ShouldReturnUserDto()
    {
        var userId = Guid.NewGuid();
        var userDto = new UserDto { Id = userId, Email = "test@example.com", DisplayName = "Test User", Role = Role.Candidate };
        var mockAuthService = new Mock<IAuthService>();
        mockAuthService.Setup(s => s.AuthenticateAsync("test@example.com", "password")).ReturnsAsync(userDto);

        var handler = new AuthenticateUserCommandHandler(mockAuthService.Object);
        var command = new AuthenticateUserCommand("test@example.com", "password");

        var result = await handler.Handle(command, CancellationToken.None);
        result.Should().NotBeNull();
        result.Id.Should().Be(userId);
        result.Email.Should().Be("test@example.com");
        result.DisplayName.Should().Be("Test User");
        result.Role.Should().Be(Role.Candidate);
    }

    [Fact]
    public async Task AuthenticateUserCommandHandler_InvalidCredentials_ShouldThrowException()
    {
        var mockAuthService = new Mock<IAuthService>();
        mockAuthService.Setup(s => s.AuthenticateAsync("test@example.com", "wrongpassword")).ThrowsAsync(new UnauthorizedAccessException("Invalid credentials"));

        var handler = new AuthenticateUserCommandHandler(mockAuthService.Object);
        var command = new AuthenticateUserCommand("test@example.com", "wrongpassword");

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task GetListingByIdQueryHandler_ExistingListing_ShouldReturnListingDto()
    {
        var listingId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var listing = new ProjectListing(listingId, "Title", "Short Desc", "Requirements", new List<Tag> { new Tag("csharp") }, userId, ListingStatus.Published, DateTime.UtcNow);
        var mockRepo = new Mock<IProjectListingRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(listing);

        var handler = new GetListingByIdQueryHandler(mockRepo.Object);
        var query = new GetListingByIdQuery(listingId);

        var result = await handler.Handle(query, CancellationToken.None);
        result.Should().NotBeNull();
        result.Id.Should().Be(listingId);
        result.Title.Should().Be("Title");
        result.Tags.Should().Contain("csharp");
    }

    [Fact]
    public async Task GetListingByIdQueryHandler_NonExistingListing_ShouldReturnNull()
    {
        var mockRepo = new Mock<IProjectListingRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ProjectListing)null);

        var handler = new GetListingByIdQueryHandler(mockRepo.Object);
        var query = new GetListingByIdQuery(Guid.NewGuid());

        var result = await handler.Handle(query, CancellationToken.None);
        result.Should().BeNull();
    }
}
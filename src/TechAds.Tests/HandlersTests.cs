using Xunit;
using Moq;
using FluentAssertions;
using TechAds.Application.Commands;
using TechAds.Application.Commands.Handlers;
using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using TechAds.Domain.ValueObjects;
using TechAds.Domain.Enums;

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
}
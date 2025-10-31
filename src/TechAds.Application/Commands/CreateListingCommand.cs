using MediatR;

namespace TechAds.Application.Commands;

public class CreateListingCommand : IRequest<Guid>
{
    public string Title { get; }
    public string ShortDescription { get; }
    public string Requirements { get; }
    public List<string> Tags { get; }
    public Guid CreatedByUserId { get; }

    public CreateListingCommand(string title, string shortDescription, string requirements, List<string> tags, Guid createdByUserId)
    {
        Title = title;
        ShortDescription = shortDescription;
        Requirements = requirements;
        Tags = tags;
        CreatedByUserId = createdByUserId;
    }
}
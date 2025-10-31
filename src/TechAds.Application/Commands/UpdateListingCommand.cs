using MediatR;

namespace TechAds.Application.Commands;

public class UpdateListingCommand : IRequest<Unit>
{
    public Guid Id { get; }
    public string Title { get; }
    public string ShortDescription { get; }
    public string Requirements { get; }
    public List<string> Tags { get; }
    public Guid UpdatedByUserId { get; }

    public UpdateListingCommand(Guid id, string title, string shortDescription, string requirements, List<string> tags, Guid updatedByUserId)
    {
        Id = id;
        Title = title;
        ShortDescription = shortDescription;
        Requirements = requirements;
        Tags = tags;
        UpdatedByUserId = updatedByUserId;
    }
}
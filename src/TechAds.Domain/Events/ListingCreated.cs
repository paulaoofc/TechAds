namespace TechAds.Domain.Events;

public class ListingCreated
{
    public Guid ListingId { get; }
    public Guid CreatedByUserId { get; }
    public DateTime CreatedAt { get; }

    public ListingCreated(Guid listingId, Guid createdByUserId, DateTime createdAt)
    {
        ListingId = listingId;
        CreatedByUserId = createdByUserId;
        CreatedAt = createdAt;
    }
}
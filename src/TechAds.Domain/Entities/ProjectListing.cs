using System.Collections.Generic;
using TechAds.Domain.ValueObjects;
using TechAds.Domain.Enums;

namespace TechAds.Domain.Entities;

public class ProjectListing
{
    public Guid Id { get; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Requirements { get; set; }
    public List<Tag> Tags { get; set; }
    public Guid CreatedByUserId { get; }
    public ListingStatus Status { get; set; }
    public DateTime CreatedAt { get; }

    public ProjectListing(Guid id, string title, string shortDescription, string requirements, List<Tag> tags, Guid createdByUserId, ListingStatus status, DateTime createdAt)
    {
        Id = id;
        Title = title;
        ShortDescription = shortDescription;
        Requirements = requirements;
        Tags = tags;
        CreatedByUserId = createdByUserId;
        Status = status;
        CreatedAt = createdAt;
    }
}
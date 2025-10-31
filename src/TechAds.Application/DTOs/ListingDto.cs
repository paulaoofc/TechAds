using TechAds.Domain.Enums;

namespace TechAds.Application.DTOs;

public class ListingDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Requirements { get; set; }
    public List<string> Tags { get; set; }
    public Guid CreatedByUserId { get; set; }
    public ListingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
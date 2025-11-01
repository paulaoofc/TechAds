namespace TechAds.Api.DTOs;

public class CreateListingRequest
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Requirements { get; set; }
    public List<string> Tags { get; set; }
}
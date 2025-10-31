using TechAds.Domain.Enums;

namespace TechAds.Application.DTOs;

public class ApplicationDto
{
    public Guid Id { get; set; }
    public Guid ProjectListingId { get; set; }
    public Guid CandidateUserId { get; set; }
    public string Message { get; set; }
    public ApplicationStatus Status { get; set; }
    public DateTime AppliedAt { get; set; }
}
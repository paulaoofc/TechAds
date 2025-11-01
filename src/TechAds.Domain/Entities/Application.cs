using TechAds.Domain.Enums;

namespace TechAds.Domain.Entities;

public class Application
{
    public Guid Id { get; }
    public Guid ProjectListingId { get; }
    public Guid CandidateUserId { get; }
    public string Message { get; }
    public ApplicationStatus Status { get; }
    public DateTime AppliedAt { get; }

    public Application(Guid id, Guid projectListingId, Guid candidateUserId, string message, ApplicationStatus status, DateTime appliedAt)
    {
        Id = id;
        ProjectListingId = projectListingId;
        CandidateUserId = candidateUserId;
        Message = message;
        Status = status;
        AppliedAt = appliedAt;
    }

    private Application() { }
}
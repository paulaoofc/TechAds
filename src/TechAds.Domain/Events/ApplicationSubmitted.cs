namespace TechAds.Domain.Events;

public class ApplicationSubmitted
{
    public Guid ApplicationId { get; }
    public Guid ProjectListingId { get; }
    public Guid CandidateUserId { get; }
    public DateTime AppliedAt { get; }

    public ApplicationSubmitted(Guid applicationId, Guid projectListingId, Guid candidateUserId, DateTime appliedAt)
    {
        ApplicationId = applicationId;
        ProjectListingId = projectListingId;
        CandidateUserId = candidateUserId;
        AppliedAt = appliedAt;
    }
}
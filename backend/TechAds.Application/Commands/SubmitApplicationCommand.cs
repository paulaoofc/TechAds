using MediatR;
using TechAds.Application.Common;

namespace TechAds.Application.Commands;

public class SubmitApplicationCommand : IRequest<Result<Guid>>
{
    public Guid ProjectListingId { get; }
    public Guid CandidateUserId { get; }
    public string Message { get; }

    public SubmitApplicationCommand(Guid projectListingId, Guid candidateUserId, string message)
    {
        ProjectListingId = projectListingId;
        CandidateUserId = candidateUserId;
        Message = message;
    }
}
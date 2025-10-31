using MediatR;
using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using TechAds.Domain.Enums;

namespace TechAds.Application.Commands.Handlers;

public class SubmitApplicationCommandHandler : IRequestHandler<SubmitApplicationCommand, Guid>
{
    private readonly IApplicationRepository _repository;

    public SubmitApplicationCommandHandler(IApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var application = new TechAds.Domain.Entities.Application(id, request.ProjectListingId, request.CandidateUserId, request.Message, ApplicationStatus.Pending, DateTime.UtcNow);
        await _repository.AddAsync(application);
        return id;
    }
}
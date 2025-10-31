using MediatR;
using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using TechAds.Domain.ValueObjects;
using TechAds.Application.DTOs;

namespace TechAds.Application.Commands.Handlers;

public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, UserDto>
{
    private readonly IUserRepository _repository;

    public AuthenticateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UserDto> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        var email = new Email(request.Email);
        var user = await _repository.GetByEmailAsync(email);
        if (user == null) throw new Exception("Invalid credentials");
        return new UserDto { Id = user.Id, Email = user.Email.Value, DisplayName = user.DisplayName, Role = user.Role };
    }
}
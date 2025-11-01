using MediatR;
using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using TechAds.Domain.ValueObjects;
using TechAds.Application.DTOs;
using TechAds.Application.Interfaces;

namespace TechAds.Application.Commands.Handlers;

public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, UserDto>
{
    private readonly IAuthService _authService;

    public AuthenticateUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<UserDto> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        return await _authService.AuthenticateAsync(request.Email, request.Password);
    }
}
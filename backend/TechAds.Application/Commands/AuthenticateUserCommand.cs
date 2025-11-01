using MediatR;
using TechAds.Application.DTOs;

namespace TechAds.Application.Commands;

public class AuthenticateUserCommand : IRequest<UserDto>
{
    public string Email { get; }
    public string Password { get; }

    public AuthenticateUserCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
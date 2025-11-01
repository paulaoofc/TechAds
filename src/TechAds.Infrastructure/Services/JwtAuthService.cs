using TechAds.Application.Interfaces;
using TechAds.Application.DTOs;
using TechAds.Domain.Interfaces;
using TechAds.Domain.Enums;
using BCrypt.Net;

namespace TechAds.Infrastructure.Services;

public class JwtAuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public JwtAuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(new Domain.ValueObjects.Email(email));
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email.Value,
            DisplayName = user.DisplayName,
            Role = user.Role
        };
    }
}
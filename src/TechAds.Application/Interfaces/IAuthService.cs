using TechAds.Application.DTOs;

namespace TechAds.Application.Interfaces;

public interface IAuthService
{
    Task<UserDto> AuthenticateAsync(string email, string password);
}
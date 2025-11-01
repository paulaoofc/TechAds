using TechAds.Domain.Enums;

namespace TechAds.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public Role Role { get; set; }
}
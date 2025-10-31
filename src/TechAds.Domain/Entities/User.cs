using TechAds.Domain.ValueObjects;
using TechAds.Domain.Enums;

namespace TechAds.Domain.Entities;

public class User
{
    public Guid Id { get; }
    public Email Email { get; }
    public string PasswordHash { get; }
    public string DisplayName { get; }
    public Role Role { get; }

    public User(Guid id, Email email, string passwordHash, string displayName, Role role)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        DisplayName = displayName;
        Role = role;
    }
}
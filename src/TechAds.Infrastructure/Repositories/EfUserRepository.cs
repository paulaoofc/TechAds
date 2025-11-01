using Microsoft.AspNetCore.Identity;
using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using TechAds.Domain.ValueObjects;
using TechAds.Domain.Enums;
using BCrypt.Net;

namespace TechAds.Infrastructure.Repositories;

public class EfUserRepository : IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    public EfUserRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var identityUser = await _userManager.FindByIdAsync(id.ToString());
        if (identityUser == null) return null;
        var roles = await _userManager.GetRolesAsync(identityUser);
        var role = roles.FirstOrDefault() ?? "Candidate";
        return new User(Guid.Parse(identityUser.Id), new Email(identityUser.Email), identityUser.PasswordHash, identityUser.UserName, Enum.Parse<Role>(role));
    }

    public async Task<User> GetByEmailAsync(Email email)
    {
        var identityUser = await _userManager.FindByEmailAsync(email.Value);
        if (identityUser == null) return null;
        var roles = await _userManager.GetRolesAsync(identityUser);
        var role = roles.FirstOrDefault() ?? "Candidate";
        return new User(Guid.Parse(identityUser.Id), new Email(identityUser.Email), identityUser.PasswordHash, identityUser.UserName, Enum.Parse<Role>(role));
    }

    public async Task AddAsync(User user)
    {
        var identityUser = new IdentityUser { Id = user.Id.ToString(), UserName = user.DisplayName, Email = user.Email.Value };
        identityUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword("TempPassword123!");
        var result = await _userManager.CreateAsync(identityUser);
        await _userManager.AddToRoleAsync(identityUser, user.Role.ToString());
    }
}
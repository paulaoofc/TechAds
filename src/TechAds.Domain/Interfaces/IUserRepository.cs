using System.Threading.Tasks;
using TechAds.Domain.Entities;
using TechAds.Domain.ValueObjects;

namespace TechAds.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByEmailAsync(Email email);
    Task AddAsync(User user);
}
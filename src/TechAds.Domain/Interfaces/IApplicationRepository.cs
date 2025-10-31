using System.Threading.Tasks;
using System.Collections.Generic;
using TechAds.Domain.Entities;

namespace TechAds.Domain.Interfaces;

public interface IApplicationRepository
{
    Task<Application> GetByIdAsync(Guid id);
    Task<IEnumerable<Application>> GetByProjectListingIdAsync(Guid projectListingId);
    Task AddAsync(Application application);
}
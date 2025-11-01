using System.Threading.Tasks;
using System.Collections.Generic;
using TechAds.Domain.Entities;

namespace TechAds.Domain.Interfaces;

public interface IProjectListingRepository
{
    Task<ProjectListing> GetByIdAsync(Guid id);
    Task<IEnumerable<ProjectListing>> GetAllAsync();
    Task AddAsync(ProjectListing listing);
    Task UpdateAsync(ProjectListing listing);
}
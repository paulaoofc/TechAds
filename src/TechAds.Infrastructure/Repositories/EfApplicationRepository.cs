using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TechAds.Infrastructure.Repositories;

public class EfApplicationRepository : IApplicationRepository
{
    private readonly TechAdsDbContext _context;

    public EfApplicationRepository(TechAdsDbContext context)
    {
        _context = context;
    }

    public async Task<TechAds.Domain.Entities.Application> GetByIdAsync(Guid id)
    {
        return await _context.Applications.FindAsync(id);
    }

    public async Task<IEnumerable<TechAds.Domain.Entities.Application>> GetByProjectListingIdAsync(Guid projectListingId)
    {
        return await _context.Applications.Where(a => a.ProjectListingId == projectListingId).ToListAsync();
    }

    public async Task AddAsync(TechAds.Domain.Entities.Application application)
    {
        await _context.Applications.AddAsync(application);
        await _context.SaveChangesAsync();
    }
}
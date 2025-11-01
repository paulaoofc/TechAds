using TechAds.Domain.Interfaces;
using TechAds.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TechAds.Infrastructure.Repositories;

public class EfProjectListingRepository : IProjectListingRepository
{
    private readonly TechAdsDbContext _context;

    public EfProjectListingRepository(TechAdsDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectListing> GetByIdAsync(Guid id)
    {
        return await _context.ProjectListings.FindAsync(id);
    }

    public async Task<IEnumerable<ProjectListing>> GetAllAsync()
    {
        return await _context.ProjectListings.ToListAsync();
    }

    public async Task AddAsync(ProjectListing listing)
    {
        await _context.ProjectListings.AddAsync(listing);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProjectListing listing)
    {
        _context.ProjectListings.Update(listing);
        await _context.SaveChangesAsync();
    }
}
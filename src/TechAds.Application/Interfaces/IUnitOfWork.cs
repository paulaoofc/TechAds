namespace TechAds.Application.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
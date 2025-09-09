using DirectoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application;

public interface IDirectoryServiceDbContext
{
    DbSet<Location> Locations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
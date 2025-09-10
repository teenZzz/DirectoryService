using DirectoryService.Application.Repositories;
using DirectoryService.Domain.Entities;

namespace DirectoryService.Infrastructure.Postgres.Repositories;

public class EfCoreLocationsRepository : ILocationRepository
{
    private readonly DirectoryServiceDbContext _dbContext;
    
    public EfCoreLocationsRepository(DirectoryServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Guid> Add(Location location, CancellationToken cancellationToken)
    {
        await _dbContext.Locations.AddAsync(location, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return location.Id;
    }
}
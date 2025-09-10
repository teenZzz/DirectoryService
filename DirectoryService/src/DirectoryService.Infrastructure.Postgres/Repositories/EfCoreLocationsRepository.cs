using CSharpFunctionalExtensions;
using DirectoryService.Application.Repositories;
using DirectoryService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Postgres.Repositories;

public class EfCoreLocationsRepository : ILocationRepository
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILogger<EfCoreLocationsRepository> _logger;

    public EfCoreLocationsRepository(DirectoryServiceDbContext dbContext, ILogger<EfCoreLocationsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<Result<Guid>> Add(Location location, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Locations.AddAsync(location, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return location.Id;
        }
        catch (Exception ex)
        {   
            _logger.LogError(ex, "Fail to insert location");

            // Временно, пока нет Error
            // TODO: заменить на Error
            return Guid.Empty;
        }
    }
}
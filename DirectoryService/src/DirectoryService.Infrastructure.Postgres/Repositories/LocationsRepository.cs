using CSharpFunctionalExtensions;
using DirectoryService.Application.Repositories;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Postgres.Repositories;

public class LocationsRepository : ILocationRepository
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILogger<LocationsRepository> _logger;

    public LocationsRepository(DirectoryServiceDbContext dbContext, ILogger<LocationsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<Result<Guid, Error>> Add(Location location, CancellationToken cancellationToken)
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

            return Error.Failure(null, ex.Message);
        }
    }
}
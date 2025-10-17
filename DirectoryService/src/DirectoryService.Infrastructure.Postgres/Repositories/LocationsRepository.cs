using CSharpFunctionalExtensions;
using DirectoryService.Application.Locations;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
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


    public async Task<Result<Guid, Error>> AddAsync(Location location, CancellationToken cancellationToken)
    { 
        await _dbContext.Locations.AddAsync(location, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return location.Id;
    }

    public async Task<Result<bool, Error>> ExistsByNameAsync(Name locationName, CancellationToken cancellationToken)
    {
        bool exists = await _dbContext.Locations
            .AnyAsync(l => l.Name == locationName, cancellationToken);

        return exists == true;
    }

    public async Task<Result<bool, Error>> ExistsByAddressAsync(Address address, CancellationToken cancellationToken)
    {
        bool exists = await _dbContext.Locations
            .AnyAsync(l => l.Address == address, cancellationToken);

        return exists == true;
    }
}
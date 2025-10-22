using CSharpFunctionalExtensions;
using DirectoryService.Application.Locations;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
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

        var saveResult = await SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
            return saveResult.Error;
                
        return location.Id;
    }

    public async Task<Result<bool, Error>> ExistsByNameAsync(Name locationName, CancellationToken cancellationToken)
    {
        bool exists = await _dbContext.Locations
            .AnyAsync(l => l.Name.Value == locationName.Value, cancellationToken);

        return exists;
    }

    public async Task<Result<bool, Error>> ExistsByAddressAsync(Address address, CancellationToken cancellationToken)
    {
        bool exists = await _dbContext.Locations
            .AnyAsync(
                l =>
                    l.Address.Country == address.Country &&
                    l.Address.City == address.City &&
                    l.Address.Street == address.Street &&
                    l.Address.HouseNumber == address.HouseNumber &&
                    l.Address.OfficeNumber == address.OfficeNumber &&
                    l.Address.AdditionalInfo == address.AdditionalInfo,
                cancellationToken);

        return exists;
    }
    
    public async Task<Result<bool, Errors>> AllExistAsync(IReadOnlyCollection<Guid> locationIds, CancellationToken cancellationToken)
    {
        if (locationIds.Count == 0)
            return true;

        var existingCount = await _dbContext.Locations
            .Where(l => locationIds.Contains(l.Id))
            .CountAsync(cancellationToken);

        return existingCount == locationIds.Count;
    }

    private async Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error saving changes!");
            return GeneralErrors.General.Failure();
        }
    }
}
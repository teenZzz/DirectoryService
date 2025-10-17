using CSharpFunctionalExtensions;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Application.Locations;

public interface ILocationRepository
{
    Task<Result<Guid, Error>> AddAsync(Location location, CancellationToken cancellationToken = default);
    
    Task<Result<bool, Error>> ExistsByNameAsync(Name locationName, CancellationToken cancellationToken);

    Task<Result<bool, Error>> ExistsByAddressAsync(Address address, CancellationToken cancellationToken);
}
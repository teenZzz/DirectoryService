using DirectoryService.Domain.Entities;

namespace DirectoryService.Application.Repositories;

public interface ILocationRepository
{
    Task<Guid> Add(Location location, CancellationToken cancellationToken = default);
}
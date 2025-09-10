using CSharpFunctionalExtensions;
using DirectoryService.Domain.Entities;

namespace DirectoryService.Application.Repositories;

public interface ILocationRepository
{
    Task<Result<Guid>> Add(Location location, CancellationToken cancellationToken = default);
}
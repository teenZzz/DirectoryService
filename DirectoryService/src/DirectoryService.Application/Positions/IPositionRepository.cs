using CSharpFunctionalExtensions;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Application.Positions;

public interface IPositionRepository
{
    Task<Result<Guid, Error>> AddAsync(Position position, CancellationToken cancellationToken = default);

    Task<Result<bool, Error>> NameExistAndActiveAsync(string name, CancellationToken cancellationToken);

    Task<Result<Position, Errors>> GetById(Guid positionId, CancellationToken cancellationToken);
}
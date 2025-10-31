using CSharpFunctionalExtensions;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Application.Positions;

public interface IPositionRepository
{
    Task<Result<Guid, Error>> AddAsync(Position position, CancellationToken cancellationToken = default);

    Task<Result<bool, Error>> NameExistAndActiveAsync(Name name, CancellationToken cancellationToken);

    Task<Result<Position, Errors>> GetById(Guid positionId, CancellationToken cancellationToken);

    Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken);
}
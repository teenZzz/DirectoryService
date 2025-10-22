using CSharpFunctionalExtensions;
using DirectoryService.Application.Positions;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Postgres.Repositories;

public class PositionsRepository : IPositionRepository
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILogger<PositionsRepository> _logger;
    
    public PositionsRepository(DirectoryServiceDbContext dbContext, ILogger<PositionsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> AddAsync(Position position, CancellationToken cancellationToken)
    {
        await _dbContext.Positions.AddAsync(position, cancellationToken);

        var saveResult = await SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
            return saveResult.Error;

        return position.Id;
    }

    public async Task<Result<bool, Error>> NameExistAndActiveAsync(string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Validation("position.name", "Position name cannot be empty");

        var exists = await _dbContext.Positions
            .AnyAsync(p => p.Name.Value == name && p.IsActive, cancellationToken);

        return exists;
    }

    public async Task<Result<Position, Errors>> GetById(Guid positionId, CancellationToken cancellationToken)
    {
        var position = await _dbContext.Positions
            .FirstOrDefaultAsync(d => d.Id == positionId, cancellationToken);

        if (position is null)
            return GeneralErrors.General.NotFound(positionId).ToErrors();

        return position;
    }

    private async Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<Error>();
        }
        catch (DbUpdateException ex)
        {
            var baseMsg = ex.GetBaseException().Message;
            _logger.LogError(ex, "DB update error: {Message}", baseMsg);
            return Error.Failure("db.update", $"DB update failed: {baseMsg}");
        }
        catch (Exception ex)
        {
            var baseMsg = ex.GetBaseException().Message;
            _logger.LogError(ex, "Unexpected DB error: {Message}", baseMsg);
            return Error.Failure("db.unexpected", $"Unexpected DB error: {baseMsg}");
        }
    }
}
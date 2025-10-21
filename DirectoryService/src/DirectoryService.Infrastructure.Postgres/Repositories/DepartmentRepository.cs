using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Postgres.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILogger<DepartmentRepository> _logger;
    
    public DepartmentRepository(DirectoryServiceDbContext dbContext, ILogger<DepartmentRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> AddAsync(Department department, CancellationToken cancellationToken)
    { 
        await _dbContext.Departments.AddAsync(department, cancellationToken);

        var saveResult = await SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
            return Error.Failure(null, "Error saving changes!");
                
        return department.Id;
    }
    
    public async Task<Result<Department, Errors>> GetById(Guid departmentId, CancellationToken cancellationToken)
    {
        var department = await _dbContext.Departments
            .FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);

        if (department is null)
            return GeneralErrors.General.NotFound(departmentId).ToErrors();

        return department;
    }

    public async Task<Result<bool, Error>> ExistsByIdentifierAsync(Identifier identifier, CancellationToken cancellationToken)
    {
        bool exists = await _dbContext.Departments
            .AnyAsync(d => d.Identifier.Value == identifier.Value, cancellationToken);
        
        return exists;
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
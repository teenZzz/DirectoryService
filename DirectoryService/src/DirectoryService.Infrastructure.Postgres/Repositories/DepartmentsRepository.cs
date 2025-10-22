using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Postgres.Repositories;

public class DepartmentsRepository : IDepartmentRepository
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILogger<DepartmentsRepository> _logger;
    
    public DepartmentsRepository(DirectoryServiceDbContext dbContext, ILogger<DepartmentsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<Result<Guid, Error>> AddAsync(Department department, CancellationToken cancellationToken)
    { 
        await _dbContext.Departments.AddAsync(department, cancellationToken);

        var saveResult = await SaveChangesAsync(cancellationToken);
        if (saveResult.IsFailure)
            return saveResult.Error;
                
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
    
    public async Task<Result<bool, Errors>> AllExistAndActiveAsync(IReadOnlyCollection<Guid> departmentIds, CancellationToken cancellationToken)
    {
        if (departmentIds.Count == 0)
            return true;

        var existingCount = await _dbContext.Departments
            .Where(d => departmentIds.Contains(d.Id) && d.IsActive)
            .CountAsync(cancellationToken);

        return existingCount == departmentIds.Count;
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
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Application.Departments;

public interface IDepartmentRepository
{
    Task<Result<Guid, Error>> AddAsync(Department department, CancellationToken cancellationToken = default);

    Task<Result<Department, Errors>> GetById(Guid departmentId, CancellationToken cancellationToken = default);

    Task<Result<bool, Error>> ExistsByIdentifierAsync(Identifier identifier, CancellationToken cancellationToken);
    
    Task<Result<bool, Errors>> AllExistAndActiveAsync(IReadOnlyCollection<Guid> departmentsId, CancellationToken cancellationToken);

    Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken);
}
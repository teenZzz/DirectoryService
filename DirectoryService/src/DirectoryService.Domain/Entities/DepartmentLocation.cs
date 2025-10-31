using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities;

public class DepartmentLocation
{
    // EF Core
    public DepartmentLocation()
    {
    }
    
    public DepartmentLocation(Guid departmentId, Guid locationId)
    {
        DepartmentId = departmentId;
        LocationId = locationId;
    }

    public Guid Id { get; private set; }

    public Guid DepartmentId { get; private set; }

    public Guid LocationId { get; private set; }
    
}
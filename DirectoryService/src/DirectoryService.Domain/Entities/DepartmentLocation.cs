using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities;

public class DepartmentLocation
{
    // EF Core
    private DepartmentLocation()
    {
    }
    
    private DepartmentLocation(Guid departmentId, Guid locationId)
    {
        DepartmentId = departmentId;
        LocationId = locationId;
    }

    public Guid DepartmentId { get; private set; }

    public Guid LocationId { get; private set; }

    public static Result<DepartmentLocation, Error> Create(Guid departmentId, Guid locationId)
    {
        return new DepartmentLocation(departmentId, locationId);
    }
}
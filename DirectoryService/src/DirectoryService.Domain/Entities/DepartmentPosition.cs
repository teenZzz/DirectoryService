using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities;

public class DepartmentPosition
{
    // EF Core
    private DepartmentPosition()
    {
    }
    
    private DepartmentPosition(Guid departmentId, Guid positionId)
    {
        DepartmentId = departmentId;
        PositionId = positionId;
    }

    public Guid DepartmentId { get; private set; }

    public Guid PositionId { get; private set; }

    public static Result<DepartmentPosition, Error> Create(Guid departmentId, Guid positionId)
    {
        return new DepartmentPosition(departmentId, positionId);
    }
}
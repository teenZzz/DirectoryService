using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.Entities;

public class DepartmentPosition
{
    // EF Core
    public DepartmentPosition()
    {
    }
    
    public DepartmentPosition(Guid departmentId, Guid positionId)
    {
        DepartmentId = departmentId;
        PositionId = positionId;
    }

    public Guid Id { get; private set; }
    
    public Guid DepartmentId { get; private set; }

    public Guid PositionId { get; private set; }
    
}
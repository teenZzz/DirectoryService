using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.Entities;

public class DepartmentPosition
{
    private DepartmentPosition(Guid departmentId, Guid positionId)
    {
        DepartmentId = departmentId;
        PositionId = positionId;
    }

    public Guid DepartmentId { get; private set; }

    public Guid PositionId { get; private set; }

    public static Result<DepartmentPosition> Create(Guid departmentId, Guid positionId)
    {
        var obj = new DepartmentPosition(departmentId, positionId);
        return Result.Success(obj);
    }
}
using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.Entities;

public class DepartmentPosition
{
    private DepartmentPosition(Department department, Position position)
    {
        Department = department;
        Position = position;
    }

    public Department Department { get; private set; }

    public Position Position { get; private set; }

    public static Result<DepartmentPosition> Create(Department department, Position position)
    {
        var obj = new DepartmentPosition(department, position);
        return Result.Success(obj);
    }
}
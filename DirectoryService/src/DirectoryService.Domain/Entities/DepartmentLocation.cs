using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.Entities;

public class DepartmentLocation
{
    private DepartmentLocation(Department department, Location location)
    {
        Department = department;
        Location = location;
    }

    public Department Department { get; private set; }

    public Location Location { get; private set; }

    public static Result<DepartmentLocation> Create(Department department, Location location)
    {
        var obj = new DepartmentLocation(department, location);
        return Result.Success(obj);
    }
}
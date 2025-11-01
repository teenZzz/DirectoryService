using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;
using Path = DirectoryService.Domain.ValueObjects.Path;

namespace DirectoryService.Domain.Entities;

public class Department
{
    // EF Core
    private Department()
    {
    }
    
    private List<DepartmentLocation> _departmentLocations = [];
    private readonly List<DepartmentPosition> _departmentPositions = [];
    
    private Department(
        Guid id,
        Name name, 
        Identifier identifier, 
        Guid? parentId, 
        Path path,
        DepartmentDepth depth,
        IEnumerable<DepartmentLocation> departmentLocations)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = depth;
        IsActive = true;
        _departmentLocations = departmentLocations.ToList();
    }

    public Guid Id { get; private set; }

    public Name Name { get; private set; }
    
    public Identifier Identifier { get; private set; }

    public Guid? ParentId { get; private set; }

    public Path Path { get; private set; }

    public DepartmentDepth Depth { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }
    
    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentLocations;

    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;
    
    public List<Department> Children => [];

    public static Result<Department, Error> CreateParent(
        Guid id,
        Name name, 
        Identifier identifier, 
        List<DepartmentLocation> departmentLocations)
    {
        if (departmentLocations.Count == 0)
        {
            return Error.Validation("department.location", "Department locations must contain at least one location");
        }
        
        var path = Path.CreateParent(identifier);

        var depth = DepartmentDepth.Create(0).Value;
        
        return new Department(id, name, identifier, null, path, depth, departmentLocations);
    }

    public static Result<Department, Error> CreateChild(
        Guid id,
        Name name, 
        Identifier identifier,
        Department parent,
        List<DepartmentLocation> departmentLocations)
    {
        if (departmentLocations.Count == 0)
        {
            return Error.Validation("department.location", "Department locations must contain at least one location");
        }

        var depth = DepartmentDepth.Create(parent.Depth.Value + 1).Value;
        
        var path = parent.Path.CreateChild(identifier);

        return new Department(id, name, identifier, parent.Id, path, depth, departmentLocations);
    }
    
    public UnitResult<Error> UpdateLocations(IEnumerable<DepartmentLocation> newLocations)
    {
        var newLocationsList = newLocations.ToList();

        if (newLocationsList.Count == 0)
        {
            return Error.Validation("department.location", "Department locations must contain at least one location");
        }

        _departmentLocations = newLocationsList;
        
        UpdatedAt = DateTime.UtcNow;

        return UnitResult.Success<Error>();
    }

}
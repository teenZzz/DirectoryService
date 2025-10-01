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
    
    private Department(
        Name name, 
        Identifier identifier, 
        Guid? parentId, 
        Path path,
        DepartmentDepth depth,
        bool isActive, 
        List<Department> children, 
        List<DepartmentLocation> departmentLocations, 
        List<DepartmentPosition> departmentPositions)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = depth;
        IsActive = isActive;
        _children = children;
        _departmentLocations = departmentLocations;
        _departmentPositions = departmentPositions;
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

    private readonly List<Department> _children;
    
    public IReadOnlyList<Department> Children => _children;

    private readonly List<DepartmentLocation> _departmentLocations;
    
    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentLocations;

    private readonly List<DepartmentPosition> _departmentPositions;

    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;

    public static Result<Department, Error> Create(
        Name name, 
        Identifier identifier, 
        Guid? parentId, 
        Path path, 
        DepartmentDepth depth, 
        bool isActive, 
        List<Department> children, 
        List<DepartmentLocation> departmentLocations, 
        List<DepartmentPosition> departmentPositions)
    {
        if (departmentLocations == null)
            return Error.Validation(null, "DepartmentLocations cannot be null!");

        if (departmentPositions == null)
            return Error.Validation(null, "DepartmentPosition cannot be null!");

        return new Department(name, identifier, parentId, path, depth, isActive, children, departmentLocations, departmentPositions);
        
    }

}
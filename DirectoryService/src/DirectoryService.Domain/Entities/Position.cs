
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain.Entities;

public class Position
{
    // EF Core
    private Position()
    {
    }
    
    private readonly List<DepartmentPosition> _departments = [];
    
    private Position(Guid id, Name name, string? description, List<DepartmentPosition> departmentPositions)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Name = name;
        Description = description;
        IsActive = true;
        _departments = departmentPositions;
    }

    public Guid Id { get; private set; }

    public Name Name { get; private set; }

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }
    
    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departments;

    public static Result<Position, Error> Create(Guid id, Name name, string? description, List<DepartmentPosition> departmentPositions)
    {
        if (description != null && description.Length > 1000)
            return Error.Validation(null, "Incorrect description length!");
        
        if (departmentPositions.Count == 0)
        {
            return Error.Validation("department.positions", "Department positions must contain at least one location");
        }
        
        return new Position(id, name, description, departmentPositions);
    }
}
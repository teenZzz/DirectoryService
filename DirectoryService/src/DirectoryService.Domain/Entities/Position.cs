
using CSharpFunctionalExtensions;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain.Entities;

public class Position
{
    // EF Core
    private Position()
    {
    }
    
    private Position(Name name, string? description, bool isActive)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    public Guid Id { get; private set; }

    public Name Name { get; private set; }

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static Result<Position> Create(Name name, string? description, bool isActive)
    {
        var obj = new Position(name, description, isActive);
        return Result.Success(obj);
    }
}
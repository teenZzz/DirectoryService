using DirectoryService.Domain.ValueObjects;
using Path = DirectoryService.Domain.ValueObjects.Path;

namespace DirectoryService.Domain.Entities;

public class Department
{
    private Department(Guid id, Name name, Identifier identifier, Guid? parentId, Path path, DepartmentDepth depth, DateTime createAt)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = depth;
        CreateAt = createAt;
    }

    public Guid Id { get; private set; }

    public Name Name { get; private set; }

    public Identifier Identifier { get; private set; }

    public Guid? ParentId { get; private set; }

    public Path Path { get; private set; }

    public DepartmentDepth Depth { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTime CreateAt { get; private set; }

    public DateTime UpdateAt { get; private set; }
}
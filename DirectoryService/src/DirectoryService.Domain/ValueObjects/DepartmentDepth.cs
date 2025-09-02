using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.ValueObjects;

public record DepartmentDepth
{
    private DepartmentDepth(int value)
    {
        Value = value;
    }

    public int Value { get; private set; }

    public static Result<DepartmentDepth> Create(int depth)
    {
        if (depth < 0)
            return Result.Failure<DepartmentDepth>("Depth cannot be less than 0!");

        var obj = new DepartmentDepth(depth);
        return Result.Success(obj);
    }
}
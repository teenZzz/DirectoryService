using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.ValueObjects;

public record DepartmentDepth
{
    private DepartmentDepth(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Result<DepartmentDepth, Error> Create(int value)
    {
        if (value < 0)
            return Error.Validation(null, "Depth cannot be less than 0!");

        return new DepartmentDepth(value);
    }
}
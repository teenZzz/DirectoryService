using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.ValueObjects;

public record Name
{
    private Name(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static Result<Name> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Name>("Name cannot be empty!");

        if (name.Length < 3 || name.Length > 150)
            return Result.Failure<Name>("Incorrect name length!");

        var obj = new Name(name);
        return Result.Success(obj);
    }
}
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.ValueObjects;

public record Name
{
    private Name(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Name> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Name>("Name cannot be empty!");

        if (name.Length < Const.Text.MIN_LENGHT || name.Length > Const.Text.MAX_LENGHT)
            return Result.Failure<Name>("Incorrect name length!");

        var obj = new Name(name);
        return Result.Success(obj);
    }
}
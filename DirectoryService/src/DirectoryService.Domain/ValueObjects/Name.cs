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

    public static Result<Name, Error> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.Validation(null, "Name cannot be empty!");

        if (name.Length < Const.Text.MIN_LENGHT || name.Length > Const.Text.MAX_LENGHT)
            return Error.Validation(null, "Incorrect name length!");

        return new Name(name);

    }
}
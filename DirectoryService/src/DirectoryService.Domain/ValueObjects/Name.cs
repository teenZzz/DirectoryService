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

    public static Result<Name, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(null, "Name cannot be empty!");

        if (value.Length < Const.Text.MIN_LENGHT || value.Length > Const.Text.MAX_LENGHT)
            return Error.Validation(null, "Incorrect name length!");

        return new Name(value);
    }
}
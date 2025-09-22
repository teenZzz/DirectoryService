using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;


namespace DirectoryService.Domain.ValueObjects;

public record Identifier
{
    private Identifier(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Identifier, Error> Create(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            return Error.Validation(null, "Identifier cannot be empty!");

        if (identifier.Length < Const.Text.MIN_LENGHT || identifier.Length > Const.Text.MAX_LENGHT)
            return Error.Validation(null, "Incorrect identifier length!");

        if (!Regex.IsMatch(identifier, Const.Regex.LATIN_REGEX))
            return Error.Validation(null, "The identifier must be in Latin!");

        return new Identifier(identifier);
    }
}
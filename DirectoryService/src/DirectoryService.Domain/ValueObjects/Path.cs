using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.ValueObjects;

public record Path
{
    private Path(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Path, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(null, "Path cannot be empty!");

        if (value.Length < Const.Text.MIN_LENGHT || value.Length > Const.Text.MAX_LENGHT)
            return Error.Validation(null, "Incorrect path length!");

        if (!Regex.IsMatch(value, Const.Regex.LATIN_REGEX))
            return Error.Validation(null, "The path must be in Latin!");

        return new Path(value);
    }
}
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;


namespace DirectoryService.Domain.ValueObjects;

public record Identifier
{
    private Identifier(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Identifier> Create(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            return Result.Failure<Identifier>("Identifier cannot be empty!");

        if (identifier.Length < 3 || identifier.Length > 150)
            return Result.Failure<Identifier>("Incorrect identifier length!");

        if (!Regex.IsMatch(identifier, "^[A-Za-z]+$"))
            return Result.Failure<Identifier>("The identifier must be in Latin!");

        var obj = new Identifier(identifier);
        return Result.Success(obj);
    }
}
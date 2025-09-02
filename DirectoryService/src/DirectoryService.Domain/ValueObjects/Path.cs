using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.ValueObjects;

public record Path
{
    private Path(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Path> Create(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Result.Failure<Path>("Path cannot be empty!");

        if (path.Length < 3 || path.Length > 150)
            return Result.Failure<Path>("Incorrect path length!");

        if (!Regex.IsMatch(path, "^[A-Za-z]+$"))
            return Result.Failure<Path>("The path must be in Latin!");

        var obj = new Path(path);
        return Result.Success(obj);
    }
}
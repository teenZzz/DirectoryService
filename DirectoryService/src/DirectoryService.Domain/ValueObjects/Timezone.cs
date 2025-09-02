using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.ValueObjects;

public record Timezone
{
    private static readonly Regex _ianaRegex = new(@"^[A-Za-z]+/[A-Za-z_]+$", RegexOptions.Compiled);
    
    private Timezone(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static Result<Timezone> Create(string timeZone)
    {
        if (string.IsNullOrWhiteSpace(timeZone))
            return Result.Failure<Timezone>("Timezone cannot be empty!");
        
        string trimmed = timeZone.Trim();
        
        if (!_ianaRegex.IsMatch(trimmed))
            return Result.Failure<Timezone>("Timezone must be a valid IANA code!");

        var obj = new Timezone(timeZone);
        return Result.Success(obj);
    }
}
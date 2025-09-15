using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DirectoryService.Domain.Shared;

namespace DirectoryService.Domain.ValueObjects;

public record Timezone
{
    private static readonly Regex _ianaRegex = new(@"^[A-Za-z]+/[A-Za-z_]+$", RegexOptions.Compiled);
    
    private Timezone(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Timezone, Error> Create(string timeZone)
    {
        if (string.IsNullOrWhiteSpace(timeZone))
            return Error.Validation(null, "Timezone cannot be empty!");
        
        string trimmed = timeZone.Trim();

        if (!_ianaRegex.IsMatch(trimmed))
            return Error.Validation(null, "Timezone must be a valid IANA code!");

        return new Timezone(timeZone);
    }
}
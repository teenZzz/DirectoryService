namespace DirectoryService.Domain.Shared;

public record Error
{
    private Error(string code, string message, ErrorType type, string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }

    public static Error Validation(string? code, string message, string? invalidField = null)
    {
        return new Error(code ?? "validation.error", message, ErrorType.VALIDATION, invalidField);
    }
    
    public static Error NotFound(string? code, string message, Guid? id)
    {
        return new Error(code ?? "record.not.found", message, ErrorType.NOT_FOUND);
    }
    
    public static Error Failure(string? code, string message)
    {
        return new Error(code ?? "failure", message, ErrorType.FAILURE);
    }
    
    public static Error Conflict(string? code, string message)
    {
        return new Error(code ?? "value.is.conflict", message, ErrorType.CONFLICT);
    }
    
    public string Code { get; }

    public string Message { get; }

    public ErrorType Type { get; }

    public string? InvalidField { get; }
}
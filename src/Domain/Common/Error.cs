namespace Domain.Common;

public enum ErrorType
{
    Failure,
    Validation,
    NotFound,
    Conflict,
      Unauthorized
}
public sealed record Error(string Code, string Message, ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public IReadOnlyDictionary<string, string[]>? ValidationErrors { get; init; }

    public static Error Validation(IReadOnlyDictionary<string, string[]> errors) =>
        new("Validation", "One or more validation errors occurred.", ErrorType.Validation)
        {
            ValidationErrors = errors
        };

    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);

    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);
     public static Error Unauthorized(string code, string message) => new(code, message, ErrorType.Unauthorized);
}

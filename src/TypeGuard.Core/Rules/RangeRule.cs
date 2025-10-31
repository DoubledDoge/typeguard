namespace TypeGuard.Core.Rules;

public class RangeRule<T>(T min, T max, string? customMessage = null) : IValidationRule<T>
    where T : IComparable<T>
{
    public bool IsValid(T value) => value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;

    public string ErrorMessage { get; } = customMessage ?? $"Value must be between {min} and {max}";
}

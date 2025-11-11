namespace TypeGuard.Core.Rules;

/// <summary>
/// A validation rule that ensures a value falls within a specified range.
/// </summary>
/// <typeparam name="T">The type of value that this rule can validate. Must implement <see cref="IComparable{T}"/>. (Generic Type)</typeparam>
/// <param name="min">The minimum acceptable value.</param>
/// <param name="max">The maximum acceptable value.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class RangeRule<T>(T min, T max, string? customMessage = null) : IValidationRule<T>
    where T : IComparable<T>
{
    /// <summary>
    /// Determines whether the specified value falls within the configured range.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><c>true</c> if the value is greater than or equal to the minimum and less than or equal to the maximum; otherwise, <c>false</c>.</returns>
    public bool IsValid(T value) => value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Value must be between {min} and {max}";
}

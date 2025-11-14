using System.Numerics;

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

/// <summary>
/// A validation rule that ensures a numeric value is positive (greater than zero).
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/>.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class PositiveRule<T>(string? customMessage = null) : IValidationRule<T>
    where T : INumber<T>
{
    /// <summary>
    /// Determines whether the specified value is positive.
    /// </summary>
    /// <param name="value">The numeric value to validate.</param>
    /// <returns><c>true</c> if the value is greater than zero; otherwise, <c>false</c>.</returns>
    public bool IsValid(T value) => value > T.Zero;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Value must be positive";
}

/// <summary>
/// A validation rule that ensures a numeric value is non-negative (greater than or equal to zero).
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/>.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NonNegativeRule<T>(string? customMessage = null) : IValidationRule<T>
    where T : INumber<T>
{
    /// <summary>
    /// Determines whether the specified value is non-negative.
    /// </summary>
    /// <param name="value">The numeric value to validate.</param>
    /// <returns><c>true</c> if the value is greater than or equal to zero; otherwise, <c>false</c>.</returns>
    public bool IsValid(T value) => value >= T.Zero;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Value must be non-negative";
}

/// <summary>
/// A validation rule that ensures a numeric value is negative (less than zero).
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/>.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NegativeRule<T>(string? customMessage = null) : IValidationRule<T>
    where T : INumber<T>
{
    /// <summary>
    /// Determines whether the specified value is negative.
    /// </summary>
    /// <param name="value">The numeric value to validate.</param>
    /// <returns><c>true</c> if the value is less than zero; otherwise, <c>false</c>.</returns>
    public bool IsValid(T value) => value < T.Zero;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Value must be negative";
}

/// <summary>
/// A validation rule that ensures a numeric value meets a minimum threshold.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="IComparable{T}"/>.</typeparam>
/// <param name="min">The minimum acceptable value (inclusive).</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class MinimumRule<T>(T min, string? customMessage = null) : IValidationRule<T>
    where T : IComparable<T>
{
    /// <summary>
    /// Determines whether the specified value meets the minimum threshold.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><c>true</c> if the value is greater than or equal to the minimum; otherwise, <c>false</c>.</returns>
    public bool IsValid(T value) => value.CompareTo(min) >= 0;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Value must be at least {min}";
}

/// <summary>
/// A validation rule that ensures a numeric value does not exceed a maximum threshold.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="IComparable{T}"/>.</typeparam>
/// <param name="max">The maximum acceptable value (inclusive).</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class MaximumRule<T>(T max, string? customMessage = null) : IValidationRule<T>
    where T : IComparable<T>
{
    /// <summary>
    /// Determines whether the specified value does not exceed the maximum threshold.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><c>true</c> if the value is less than or equal to the maximum; otherwise, <c>false</c>.</returns>
    public bool IsValid(T value) => value.CompareTo(max) <= 0;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Value must be at most {max}";
}

/// <summary>
/// A validation rule that ensures a numeric value is even.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/> and <see cref="IModulusOperators{TSelf, TOther, TResult}"/>.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class EvenRule<T>(string? customMessage = null) : IValidationRule<T>
    where T : INumber<T>, IModulusOperators<T, T, T>
{
    /// <summary>
    /// Determines whether the specified value is even.
    /// </summary>
    /// <param name="value">The numeric value to validate.</param>
    /// <returns><c>true</c> if the value is even; otherwise, <c>false</c>.</returns>
    public bool IsValid(T value) => value % (T.One + T.One) == T.Zero;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Value must be even";
}

/// <summary>
/// A validation rule that ensures a numeric value is odd.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/> and <see cref="IModulusOperators{TSelf, TOther, TResult}"/>.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class OddRule<T>(string? customMessage = null) : IValidationRule<T>
    where T : INumber<T>, IModulusOperators<T, T, T>
{
    /// <summary>
    /// Determines whether the specified value is odd.
    /// </summary>
    /// <param name="value">The numeric value to validate.</param>
    /// <returns><c>true</c> if the value is odd; otherwise, <c>false</c>.</returns>
    public bool IsValid(T value) => value % (T.One + T.One) != T.Zero;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Value must be odd";
}

/// <summary>
/// A validation rule that ensures a numeric value is a multiple of a specified divisor.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/> and <see cref="IModulusOperators{TSelf, TOther, TResult}"/>.</typeparam>
/// <param name="divisor">The divisor that the value must be a multiple of.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class MultipleOfRule<T>(T divisor, string? customMessage = null) : IValidationRule<T>
    where T : INumber<T>, IModulusOperators<T, T, T>
{
    /// <summary>
    /// Determines whether the specified value is a multiple of the divisor.
    /// </summary>
    /// <param name="value">The numeric value to validate.</param>
    /// <returns><c>true</c> if the value is evenly divisible by the divisor; otherwise, <c>false</c>.</returns>
    public bool IsValid(T value) => value % divisor == T.Zero;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Value must be a multiple of {divisor}";
}

/// <summary>
/// A validation rule that ensures a numeric value is not a multiple of a specified divisor.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/> and <see cref="IModulusOperators{TSelf, TOther, TResult}"/>.</typeparam>
/// <param name="divisor">The divisor that the value must not be a multiple of.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NotMultipleOfRule<T>(T divisor, string? customMessage = null) : IValidationRule<T>
    where T : INumber<T>, IModulusOperators<T, T, T>
{
    /// <summary>
    /// Determines whether the specified value is not a multiple of the divisor.
    /// </summary>
    /// <param name="value">The numeric value to validate.</param>
    /// <returns><c>true</c> if the value is not evenly divisible by the divisor; otherwise, <c>false</c>.</returns>
    public bool IsValid(T value) => value % divisor != T.Zero;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } =
        customMessage ?? $"Value must not be a multiple of {divisor}";
}

/// <summary>
/// A validation rule that ensures a numeric value passes the Luhn algorithm.
/// The Luhn algorithm is a checksum formula used to validate identification numbers.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must be an integer related type.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class LuhnRule<T>(string? customMessage = null) : IValidationRule<T>
    where T : IBinaryInteger<T>
{
    /// <summary>
    /// Determines whether the specified numeric value passes the Luhn algorithm check.
    /// </summary>
    /// <param name="value">The numeric value to validate.</param>
    /// <returns><c>true</c> if the value passes the Luhn check; otherwise, <c>false</c>.</returns>
    public bool IsValid(T value)
    {
        string digits = value.ToString() ?? string.Empty;
        int length = digits.Length;

        if (length == 0)
            return false;

        int sum = 0;
        int parity = length % 2;

        for (int i = 0; i < length - 1; i++)
        {
            int digit = digits[i] - '0';

            if (i % 2 == parity)
                sum += digit;
            else if (digit > 4)
                sum += 2 * digit - 9;
            else
                sum += 2 * digit;
        }

        int checkDigit = digits[length - 1] - '0';
        return checkDigit == (10 - sum % 10) % 10;
    }

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } =
        customMessage ?? "Value must pass Luhn checksum validation";
}

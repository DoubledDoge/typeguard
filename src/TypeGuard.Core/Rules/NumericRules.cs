using System.Numerics;

namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
/// A validation rule that ensures a value falls within a specified range.
/// </summary>
/// <typeparam name="T">The type of value this rule validates. Must implement <see cref="IComparable{T}"/>.</typeparam>
/// <param name="min">The minimum acceptable value (inclusive).</param>
/// <param name="max">The maximum acceptable value (inclusive).</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
public class RangeRule<T>(T min, T max, string? customMessage = null) : IValidatorRule<T>
    where T : IComparable<T>
{
    private readonly T _min =
        min.CompareTo(max) > 0
            ? throw new ArgumentException(
                $"min ({min}) must be less than or equal to max ({max}).",
                nameof(min)
            )
            : min;

    /// <inheritdoc/>
    public bool IsValid(T value) => value.CompareTo(_min) >= 0 && value.CompareTo(max) <= 0;

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? $"Value must be between {min} and {max}";
}

/// <summary>
/// A validation rule that ensures a numeric value is positive (greater than zero).
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/>.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class PositiveRule<T>(string? customMessage = null) : IValidatorRule<T>
    where T : INumber<T>
{
    /// <inheritdoc/>
    public bool IsValid(T value) => value > T.Zero;

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "Value must be positive";
}

/// <summary>
/// A validation rule that ensures a numeric value is non-negative (greater than or equal to zero).
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/>.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NonNegativeRule<T>(string? customMessage = null) : IValidatorRule<T>
    where T : INumber<T>
{
    /// <inheritdoc/>
    public bool IsValid(T value) => value >= T.Zero;

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "Value must be non-negative";
}

/// <summary>
/// A validation rule that ensures a numeric value is negative (less than zero).
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/>.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NegativeRule<T>(string? customMessage = null) : IValidatorRule<T>
    where T : INumber<T>
{
    /// <inheritdoc/>
    public bool IsValid(T value) => value < T.Zero;

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "Value must be negative";
}

/// <summary>
/// A validation rule that ensures a numeric value meets a minimum threshold.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="IComparable{T}"/>.</typeparam>
/// <param name="min">The minimum acceptable value (inclusive).</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class MinimumRule<T>(T min, string? customMessage = null) : IValidatorRule<T>
    where T : IComparable<T>
{
    /// <inheritdoc/>
    public bool IsValid(T value) => value.CompareTo(min) >= 0;

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? $"Value must be at least {min}";
}

/// <summary>
/// A validation rule that ensures a numeric value does not exceed a maximum threshold.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="IComparable{T}"/>.</typeparam>
/// <param name="max">The maximum acceptable value (inclusive).</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class MaximumRule<T>(T max, string? customMessage = null) : IValidatorRule<T>
    where T : IComparable<T>
{
    /// <inheritdoc/>
    public bool IsValid(T value) => value.CompareTo(max) <= 0;

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? $"Value must be at most {max}";
}

/// <summary>
/// A validation rule that ensures a numeric value is even.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/> and <see cref="IModulusOperators{TSelf, TOther, TResult}"/>.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class EvenRule<T>(string? customMessage = null) : IValidatorRule<T>
    where T : INumber<T>, IModulusOperators<T, T, T>
{
    /// <inheritdoc/>
    public bool IsValid(T value) => value % (T.One + T.One) == T.Zero;

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "Value must be even";
}

/// <summary>
/// A validation rule that ensures a numeric value is odd.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/> and <see cref="IModulusOperators{TSelf, TOther, TResult}"/>.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class OddRule<T>(string? customMessage = null) : IValidatorRule<T>
    where T : INumber<T>, IModulusOperators<T, T, T>
{
    /// <inheritdoc/>
    public bool IsValid(T value) => value % (T.One + T.One) != T.Zero;

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "Value must be odd";
}

/// <summary>
/// A validation rule that ensures a numeric value is a multiple of a specified divisor.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/> and <see cref="IModulusOperators{TSelf, TOther, TResult}"/>.</typeparam>
/// <param name="divisor">The divisor the value must be a multiple of. Cannot be zero.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="divisor"/> is zero.</exception>
public class MultipleOfRule<T>(T divisor, string? customMessage = null) : IValidatorRule<T>
    where T : INumber<T>, IModulusOperators<T, T, T>
{
    private readonly T _divisor = T.IsZero(divisor)
        ? throw new ArgumentException("divisor cannot be zero.", nameof(divisor))
        : divisor;

    /// <inheritdoc/>
    public bool IsValid(T value) => value % _divisor == T.Zero;

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? $"Value must be a multiple of {divisor}";
}

/// <summary>
/// A validation rule that ensures a numeric value is not a multiple of a specified divisor.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/> and <see cref="IModulusOperators{TSelf, TOther, TResult}"/>.</typeparam>
/// <param name="divisor">The divisor the value must not be a multiple of. Cannot be zero.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="divisor"/> is zero.</exception>
public class NotMultipleOfRule<T>(T divisor, string? customMessage = null) : IValidatorRule<T>
    where T : INumber<T>, IModulusOperators<T, T, T>
{
    private readonly T _divisor = T.IsZero(divisor)
        ? throw new ArgumentException("divisor cannot be zero.", nameof(divisor))
        : divisor;

    /// <inheritdoc/>
    public bool IsValid(T value) => value % _divisor != T.Zero;

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage ?? $"Value must not be a multiple of {divisor}";
}

/// <summary>
/// A validation rule that ensures a numeric value passes the Luhn algorithm checksum.
/// The Luhn algorithm is a checksum formula used to validate identification numbers
/// such as credit card numbers and IMEI numbers.
/// </summary>
/// <remarks>
/// Negative values are considered invalid since identification numbers are always non-negative.
/// The algorithm processes digits right to left, doubling every second digit from the right,
/// and verifies the total sum is divisible by 10.
/// </remarks>
/// <typeparam name="T">The integer type to validate. Must implement <see cref="IBinaryInteger{TSelf}"/>.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class LuhnRule<T>(string? customMessage = null) : IValidatorRule<T>
    where T : IBinaryInteger<T>
{
    /// <inheritdoc/>
    public bool IsValid(T value)
    {
        // Negative values are not valid identification numbers
        if (value < T.Zero)
        {
            return false;
        }

        string digits = value.ToString() ?? string.Empty;
        int length = digits.Length;

        if (length == 0)
        {
            return false;
        }

        int sum = 0;

        // Process digits right to left
        for (int i = 0; i < length; i++)
        {
            int digit = digits[length - 1 - i] - '0';

            if (i % 2 == 1)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
        }

        return sum % 10 == 0;
    }

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "Value must pass the Luhn checksum";
}

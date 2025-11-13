namespace TypeGuard.Core.Builders;

using System.Numerics;
using Abstractions;
using Rules;
using Validators;

/// <summary>
/// A fluent builder for constructing and configuring a numeric validator with validation rules.
/// Supports all numeric types including: int, long, float, double, decimal, byte, short, uint, ulong, ushort, sbyte, and Half
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{TSelf}"/> and <see cref="IMinMaxValue{TSelf}"/>.</typeparam>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
public class NumericValidatorBuilder<T>(
    string prompt,
    IInputProvider inputProvider,
    IOutputProvider outputProvider
) where T : INumber<T>, IMinMaxValue<T>, IComparable<T>
{
    private readonly NumericValidator<T> _validator = new(inputProvider, outputProvider, prompt);

    /// <summary>
    /// Adds a range validation rule to the validator.
    /// </summary>
    /// <param name="min">The minimum acceptable value.</param>
    /// <param name="max">The maximum acceptable value.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericValidatorBuilder<T> WithRange(T min, T max, string? customMessage = null)
    {
        _validator.AddRule(new RangeRule<T>(min, max, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the value is positive (greater than zero).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericValidatorBuilder<T> WithPositive(string? customMessage = null)
    {
        _validator.AddRule(new CustomRule<T>(
            value => value > T.Zero,
            customMessage ?? "Value must be positive"
        ));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the value is non-negative (greater than or equal to zero).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericValidatorBuilder<T> WithNonNegative(string? customMessage = null)
    {
        _validator.AddRule(new CustomRule<T>(
            value => value >= T.Zero,
            customMessage ?? "Value must be non-negative"
        ));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the value is negative (less than zero).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericValidatorBuilder<T> WithNegative(string? customMessage = null)
    {
        _validator.AddRule(new CustomRule<T>(
            value => value < T.Zero,
            customMessage ?? "Value must be negative"
        ));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the value is within a specified minimum bound.
    /// </summary>
    /// <param name="min">The minimum acceptable value (inclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericValidatorBuilder<T> WithMinimum(T min, string? customMessage = null)
    {
        _validator.AddRule(new CustomRule<T>(
            value => value >= min,
            customMessage ?? $"Value must be at least {min}"
        ));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the value is within a specified maximum bound.
    /// </summary>
    /// <param name="max">The maximum acceptable value (inclusive).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericValidatorBuilder<T> WithMaximum(T max, string? customMessage = null)
    {
        _validator.AddRule(new CustomRule<T>(
            value => value <= max,
            customMessage ?? $"Value must be at most {max}"
        ));
        return this;
    }

    /// <summary>
    /// Adds a custom validation rule to the validator.
    /// </summary>
    /// <param name="predicate">The function that determines whether a numeric value is valid.</param>
    /// <param name="errorMessage">The error message to display when validation fails.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericValidatorBuilder<T> WithCustomRule(Func<T, bool> predicate, string errorMessage)
    {
        _validator.AddRule(new CustomRule<T>(predicate, errorMessage));
        return this;
    }

    /// <summary>
    /// Asynchronously prompts the user for input and returns the validated numeric value.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated numeric value.</returns>
    public async Task<T> GetAsync(CancellationToken cancellationToken = default) =>
        await _validator.GetValidInputAsync(cancellationToken);

    /// <summary>
    /// Synchronously prompts the user for input and returns the validated numeric value.
    /// </summary>
    /// <returns>The validated numeric value.</returns>
    public T Get() => _validator.GetValidInput();
}

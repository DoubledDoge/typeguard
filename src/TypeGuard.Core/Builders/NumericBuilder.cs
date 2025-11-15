namespace TypeGuard.Core.Builders;

using System.Numerics;
using Abstractions;
using Rules;
using Validators;

/// <summary>
/// Provides a fluent interface for building validators for all numeric types.
/// Supports int, long, float, double, decimal, byte, short, sbyte, ushort, uint, ulong, nint, nuint, and Half.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{T}"/> and <see cref="IMinMaxValue{T}"/>.</typeparam>
/// <param name="prompt">The prompt message to display when requesting user input.</param>
/// <param name="inputProvider">The provider responsible for reading user input.</param>
/// <param name="outputProvider">The provider responsible for displaying output messages.</param>
public class NumericBuilder<T>(
    string prompt,
    IInputProvider inputProvider,
    IOutputProvider outputProvider
)
    where T : INumber<T>, IMinMaxValue<T>
{
    /// <summary>
    ///
    /// </summary>
    protected readonly NumericValidator<T> Validator = new(inputProvider, outputProvider, prompt);

    /// <summary>
    /// Adds a validation rule that ensures the numeric value falls within a specified range.
    /// </summary>
    /// <param name="min">The minimum allowed value (inclusive).</param>
    /// <param name="max">The maximum allowed value (inclusive).</param>
    /// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericBuilder<T> WithRange(T min, T max, string? customMessage = null)
    {
        Validator.AddRule(new RangeRule<T>(min, max, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the numeric value is positive.
    /// </summary>
    /// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericBuilder<T> WithPositive(string? customMessage = null)
    {
        Validator.AddRule(new PositiveRule<T>(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the numeric value is non-negative (greater than or equal to zero).
    /// </summary>
    /// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericBuilder<T> WithNonNegative(string? customMessage = null)
    {
        Validator.AddRule(new NonNegativeRule<T>(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the numeric value is negative.
    /// </summary>
    /// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericBuilder<T> WithNegative(string? customMessage = null)
    {
        Validator.AddRule(new NegativeRule<T>(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the numeric value is greater than or equal to a specified minimum.
    /// </summary>
    /// <param name="min">The minimum allowed value.</param>
    /// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericBuilder<T> WithMinimum(T min, string? customMessage = null)
    {
        Validator.AddRule(new MinimumRule<T>(min, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the numeric value is less than or equal to a specified maximum.
    /// </summary>
    /// <param name="max">The maximum allowed value.</param>
    /// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericBuilder<T> WithMaximum(T max, string? customMessage = null)
    {
        Validator.AddRule(new MaximumRule<T>(max, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a custom validation rule using a predicate function.
    /// </summary>
    /// <param name="predicate">A function that returns <c>true</c> if the value is valid; otherwise, <c>false</c>.</param>
    /// <param name="errorMessage">The error message to display when validation fails.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public NumericBuilder<T> WithCustomRule(Func<T, bool> predicate, string errorMessage)
    {
        Validator.AddRule(new CustomRule<T>(predicate, errorMessage));
        return this;
    }

    /// <summary>
    /// Asynchronously retrieves and validates user input according to the configured rules.
    /// Continuously prompts the user until valid input is provided.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation, containing the validated numeric value.</returns>
    public async Task<T> GetAsync(CancellationToken cancellationToken = default) =>
        await Validator.GetValidInputAsync(cancellationToken);

    /// <summary>
    /// Retrieves and validates user input according to the configured rules.
    /// Continuously prompts the user until valid input is provided.
    /// </summary>
    /// <returns>The validated numeric value.</returns>
    public T Get() => Validator.GetValidInput();
}

/// <summary>
/// Provides a fluent interface for building validators for integer types with integer-specific validation rules.
/// Supports int, long, byte, short, sbyte, ushort, uint, ulong, nint, and nuint.
/// Inherits all base numeric validation rules from <see cref="NumericBuilder{T}"/>.
/// </summary>
/// <typeparam name="T">The integer type to validate. Must implement <see cref="IBinaryInteger{T}"/> and <see cref="IMinMaxValue{T}"/>.</typeparam>
/// <param name="prompt">The prompt message to display when requesting user input.</param>
/// <param name="inputProvider">The provider responsible for reading user input.</param>
/// <param name="outputProvider">The provider responsible for displaying output messages.</param>
public class IntegerBuilder<T>(
    string prompt,
    IInputProvider inputProvider,
    IOutputProvider outputProvider
) : NumericBuilder<T>(prompt, inputProvider, outputProvider)
    where T : IBinaryInteger<T>, IMinMaxValue<T>
{
    /// <summary>
    /// Adds a validation rule that ensures the integer value is even.
    /// </summary>
    /// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IntegerBuilder<T> WithEven(string? customMessage = null)
    {
        Validator.AddRule(new EvenRule<T>(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the integer value is odd.
    /// </summary>
    /// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IntegerBuilder<T> WithOdd(string? customMessage = null)
    {
        Validator.AddRule(new OddRule<T>(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the integer value is a multiple of a specified factor.
    /// </summary>
    /// <param name="factor">The factor that the value must be divisible by.</param>
    /// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IntegerBuilder<T> WithMultipleOf(T factor, string? customMessage = null)
    {
        Validator.AddRule(new MultipleOfRule<T>(factor, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the integer value is not a multiple of a specified factor.
    /// </summary>
    /// <param name="factor">The factor that the value must not be divisible by.</param>
    /// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IntegerBuilder<T> WithNotMultipleOf(T factor, string? customMessage = null)
    {
        Validator.AddRule(new NotMultipleOfRule<T>(factor, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the integer value passes the Luhn algorithm checksum.
    /// The Luhn algorithm is commonly used to validate credit card numbers, IMEI numbers, and other identification numbers.
    /// </summary>
    /// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IntegerBuilder<T> WithLuhnCheck(string? customMessage = null)
    {
        Validator.AddRule(new LuhnRule<T>(customMessage));
        return this;
    }
}

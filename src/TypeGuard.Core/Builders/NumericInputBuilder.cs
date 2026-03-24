using System.Numerics;

namespace TypeGuard.Core.Builders;

using Handlers;
using Interfaces;
using Rules;

/// <summary>
/// A fluent builder for constructing and configuring a numeric input handler with validation rules.
/// Each <c>With*</c> method accumulates a rule onto the internal validator while the rules are evaluated
/// in the order they are added.
/// Supports int, long, float, double, decimal, byte, short, sbyte, ushort, uint, ulong, nint, nuint, and Half.
/// </summary>
/// <typeparam name="T">The numeric type to validate. Must implement <see cref="INumber{T}"/> and <see cref="IMinMaxValue{T}"/>.</typeparam>
/// <param name="prompt">The prompt message to display when requesting user input.</param>
/// <param name="inputProvider">The provider responsible for reading user input.</param>
/// <param name="outputProvider">The provider responsible for displaying output messages.</param>
/// <param name="validatorFactory">
/// An optional factory for creating the internal <see cref="NumericHandler{T}"/>.
/// Defaults to constructing a standard <see cref="NumericHandler{T}"/> from the provided providers.
/// </param>
public class NumericInputBuilder<T>(
	string prompt,
	IInputProvider inputProvider,
	IOutputProvider outputProvider,
	Func<string, IInputProvider, IOutputProvider, NumericHandler<T>>? validatorFactory = null
)
	: BuilderBase<T, NumericInputBuilder<T>>(
		(validatorFactory ?? ((p, i, o) => new NumericHandler<T>(i, o, p)))(
			prompt ?? throw new ArgumentNullException(nameof(prompt)),
			inputProvider ?? throw new ArgumentNullException(nameof(inputProvider)),
			outputProvider ?? throw new ArgumentNullException(nameof(outputProvider))
		)
	)
	where T : INumber<T>, IMinMaxValue<T>
{
	/// <summary>
	/// Adds a validation rule that ensures the numeric value falls within the specified range.
	/// </summary>
	/// <param name="min">The minimum allowed value (inclusive).</param>
	/// <param name="max">The maximum allowed value (inclusive).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="min"/> is greater than <paramref name="max"/>.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public NumericInputBuilder<T> WithRange(T min, T max, string? customMessage = null) =>
		min > max
			? throw new ArgumentException(
				$"min ({min}) must be less than or equal to max ({max}).",
				nameof(min)
			)
			: AddRule(new RangeRule<T>(min, max, customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the numeric value is positive.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public NumericInputBuilder<T> WithPositive(string? customMessage = null) =>
		AddRule(new PositiveRule<T>(customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the numeric value is non-negative (greater than or equal to zero).
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public NumericInputBuilder<T> WithNonNegative(string? customMessage = null) =>
		AddRule(new NonNegativeRule<T>(customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the numeric value is negative.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public NumericInputBuilder<T> WithNegative(string? customMessage = null) =>
		AddRule(new NegativeRule<T>(customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the numeric value is greater than or equal to the specified minimum.
	/// </summary>
	/// <param name="min">The minimum allowed value (inclusive).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public NumericInputBuilder<T> WithMinimum(T min, string? customMessage = null) =>
		AddRule(new MinimumRule<T>(min, customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the numeric value is less than or equal to the specified maximum.
	/// </summary>
	/// <param name="max">The maximum allowed value (inclusive).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public NumericInputBuilder<T> WithMaximum(T max, string? customMessage = null) =>
		AddRule(new MaximumRule<T>(max, customMessage));

	/// <summary>
	/// Adds a custom validation rule to the input handler.
	/// </summary>
	/// <param name="predicate">The function that determines whether a numeric value is valid. Cannot be null.</param>
	/// <param name="errorMessage">The error message to display when validation fails. Cannot be null.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> or <paramref name="errorMessage"/> is null.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public NumericInputBuilder<T> WithCustomRule(Func<T, bool> predicate, string errorMessage)
	{
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentNullException.ThrowIfNull(errorMessage);

		return AddRule(new CustomRule<T>(predicate, errorMessage));
	}
}

/// <summary>
/// A fluent builder for constructing and configuring an input handler for integer types, extending
/// <see cref="NumericInputBuilder{T}"/> with integer-specific validation rules.
/// Each <c>With*</c> method accumulates a rule onto the internal validator while the rules are evaluated
/// in the order they are added.
/// Supports int, long, byte, short, sbyte, ushort, uint, ulong, nint, and nuint.
/// </summary>
/// <remarks>
/// Calling NumericInputBuilder{T}.Get or NumericInputBuilder{T}.GetAsync permanently
/// freezes this builder. Any subsequent call to a <c>With*</c> method will throw
/// <see cref="InvalidOperationException"/>. Create a new <see cref="IntegerInputBuilder{T}"/> instance
/// if you need to reconfigure and re-prompt.
/// </remarks>
/// <typeparam name="T">The integer type to validate. Must implement <see cref="IBinaryInteger{T}"/> and <see cref="IMinMaxValue{T}"/>.</typeparam>
/// <param name="prompt">The prompt message to display when requesting user input.</param>
/// <param name="inputProvider">The provider responsible for reading user input.</param>
/// <param name="outputProvider">The provider responsible for displaying output messages.</param>
public class IntegerInputBuilder<T>(
	string prompt,
	IInputProvider inputProvider,
	IOutputProvider outputProvider
) : NumericInputBuilder<T>(prompt, inputProvider, outputProvider)
	where T : IBinaryInteger<T>, IMinMaxValue<T>
{
	/// <summary>
	/// Adds a validation rule that ensures the integer value is even.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public IntegerInputBuilder<T> WithEven(string? customMessage = null)
	{
		AddRule(new EvenRule<T>(customMessage));
		return this;
	}

	/// <summary>
	/// Adds a validation rule that ensures the integer value is odd.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public IntegerInputBuilder<T> WithOdd(string? customMessage = null)
	{
		AddRule(new OddRule<T>(customMessage));
		return this;
	}

	/// <summary>
	/// Adds a validation rule that ensures the integer value is a multiple of the specified factor.
	/// </summary>
	/// <param name="factor">The factor the value must be divisible by. Cannot be zero.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="factor"/> is zero.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public IntegerInputBuilder<T> WithMultipleOf(T factor, string? customMessage = null) =>
		T.IsZero(factor)
			? throw new ArgumentException("factor cannot be zero.", nameof(factor))
			: (IntegerInputBuilder<T>)AddRule(new MultipleOfRule<T>(factor, customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the integer value is not a multiple of the specified factor.
	/// </summary>
	/// <param name="factor">The factor the value must not be divisible by. Cannot be zero.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="factor"/> is zero.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public IntegerInputBuilder<T> WithNotMultipleOf(T factor, string? customMessage = null) =>
		T.IsZero(factor)
			? throw new ArgumentException("factor cannot be zero.", nameof(factor))
			: (IntegerInputBuilder<T>)AddRule(new NotMultipleOfRule<T>(factor, customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the integer value passes the Luhn algorithm checksum.
	/// The Luhn algorithm is commonly used to validate credit card numbers, IMEI numbers, and other identification numbers.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public IntegerInputBuilder<T> WithLuhnCheck(string? customMessage = null)
	{
		AddRule(new LuhnRule<T>(customMessage));
		return this;
	}
}

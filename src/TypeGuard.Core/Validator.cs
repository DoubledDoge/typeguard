namespace TypeGuard.Core;

using Interfaces;

/// <summary>
/// Validates an already-held value of type <typeparamref name="T"/> against a configured set of
/// rules, without any prompt or I/O involvement.
/// </summary>
/// <remarks>
/// Rules are evaluated in the order they are added. <see cref="Validate"/> stops at the first
/// failure, while <see cref="ValidateAll"/> collects every failure.
/// </remarks>
/// <typeparam name="T">The type of value to validate.</typeparam>
public sealed class Validator<T>
{
	private readonly List<IValidatorRule<T>> _rules = [];

	/// <summary>
	/// Adds a validation rule to this validator.
	/// </summary>
	/// <param name="rule">The rule to add. Cannot be null.</param>
	/// <returns>The current <see cref="Validator{T}"/> instance for method chaining.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="rule"/> is null.</exception>
	public Validator<T> AddRule(IValidatorRule<T> rule)
	{
		ArgumentNullException.ThrowIfNull(rule);
		_rules.Add(rule);
		return this;
	}

	/// <summary>
	/// Validates the specified value against all configured rules, stopping at the first failure.
	/// </summary>
	/// <param name="value">The value to validate.</param>
	/// <returns>
	/// <see cref="ValidationResult.Success"/> if all rules pass; otherwise, a
	/// <see cref="ValidationResult"/> carrying the error message of the first failing rule.
	/// </returns>
	public ValidationResult Validate(T value)
	{
		foreach (IValidatorRule<T> rule in _rules.Where(rule => !rule.IsValid(value)))
		{
			return ValidationResult.Failure(rule.ErrorMessage);
		}

		return ValidationResult.Success;
	}

	/// <summary>
	/// Validates the specified value against all configured rules, collecting every failure.
	/// </summary>
	/// <remarks>
	/// Use this overload when you need to display all validation errors simultaneously, such as
	/// in form-level validation scenarios.
	/// </remarks>
	/// <param name="value">The value to validate.</param>
	/// <returns>
	/// A read-only list of <see cref="ValidationResult"/> instances for each failing rule.
	/// Returns an empty list if all rules pass.
	/// </returns>
	public IReadOnlyList<ValidationResult> ValidateAll(T value)
	{
		List<ValidationResult> failures = [];
		failures.AddRange(
			from rule in _rules
			where !rule.IsValid(value)
			select ValidationResult.Failure(rule.ErrorMessage)
		);

		return failures;
	}
}

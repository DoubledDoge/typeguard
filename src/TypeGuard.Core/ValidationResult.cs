namespace TypeGuard.Core;

/// <summary>
/// Represents the outcome of a validation operation, carrying either a success state or
/// a failure state with an associated error message.
/// </summary>
/// <remarks>
/// Use <see cref="Success"/> for a successful result and <see cref="Failure"/> to construct
/// a failed result with a descriptive error message. This type is immutable and cannot be
/// subclassed.
/// </remarks>
public sealed class ValidationResult
{
	/// <summary>
	/// Gets a <see cref="ValidationResult"/> representing a successful validation.
	/// </summary>
	public static readonly ValidationResult Success = new(true, null);

	/// <summary>
	/// Gets a value indicating whether the validation passed.
	/// </summary>
	public bool IsValid { get; }

	/// <summary>
	/// Gets the error message describing why validation failed, or null if validation succeeded.
	/// </summary>
	public string? ErrorMessage { get; }

	private ValidationResult(bool isValid, string? errorMessage)
	{
		IsValid = isValid;
		ErrorMessage = errorMessage;
	}

	/// <summary>
	/// Creates a <see cref="ValidationResult"/> representing a failed validation with the
	/// specified error message.
	/// </summary>
	/// <param name="errorMessage">The message describing why validation failed. Cannot be null.</param>
	/// <returns>A <see cref="ValidationResult"/> with <see cref="IsValid"/> set to false.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessage"/> is null.</exception>
	public static ValidationResult Failure(string errorMessage) =>
		new(false, errorMessage ?? throw new ArgumentNullException(nameof(errorMessage)));
}

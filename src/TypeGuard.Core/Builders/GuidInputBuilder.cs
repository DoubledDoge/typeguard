namespace TypeGuard.Core.Builders;

using Handlers;
using Interfaces;
using Rules;

/// <summary>
/// A fluent builder for constructing and configuring a GUID input handler with validation rules.
/// Each <c>With*</c> method accumulates a rule onto the internal validator while the rules are evaluated
/// in the order they are added.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="validatorFactory">
/// An optional factory for creating the internal <see cref="GuidHandler"/>.
/// Defaults to constructing a standard <see cref="GuidHandler"/> from the provided providers.
/// </param>
public class GuidInputBuilder(
	string prompt,
	IInputProvider inputProvider,
	IOutputProvider outputProvider,
	Func<string, IInputProvider, IOutputProvider, GuidHandler>? validatorFactory = null
)
	: BuilderBase<Guid, GuidInputBuilder>(
		(validatorFactory ?? ((p, i, o) => new GuidHandler(i, o, p)))(
			prompt ?? throw new ArgumentNullException(nameof(prompt)),
			inputProvider ?? throw new ArgumentNullException(nameof(inputProvider)),
			outputProvider ?? throw new ArgumentNullException(nameof(outputProvider))
		)
	)
{
	/// <summary>
	/// Adds a validation rule that ensures the GUID is not empty (not <see cref="Guid.Empty"/>).
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public GuidInputBuilder WithNonEmpty(string? customMessage = null) =>
		AddRule(new NonEmptyGuidRule(customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the GUID matches a specific version.
	/// </summary>
	/// <param name="version">The GUID version to validate against (1-5).</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="version"/> is not between 1 and 5.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public GuidInputBuilder WithVersion(int version, string? customMessage = null) =>
		version is < 1 or > 5
			? throw new ArgumentOutOfRangeException(
				nameof(version),
				version,
				"version must be between 1 and 5."
			)
			: AddRule(new GuidVersionRule(version, customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the GUID is not in the specified collection of excluded values.
	/// </summary>
	/// <param name="excludedGuids">The collection of GUIDs that are not allowed. Cannot be null or empty.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="excludedGuids"/> is null.</exception>
	/// <exception cref="ArgumentException">Thrown when <paramref name="excludedGuids"/> is empty.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public GuidInputBuilder WithExcluded(
		IEnumerable<Guid> excludedGuids,
		string? customMessage = null
	)
	{
		ArgumentNullException.ThrowIfNull(excludedGuids);

		IEnumerable<Guid> enumerable = excludedGuids.ToList();
		return !enumerable.Any()
			? throw new ArgumentException("Cannot be empty.", nameof(excludedGuids))
			: AddRule(new ExcludedGuidRule(enumerable, customMessage));
	}

	/// <summary>
	/// Adds a validation rule that ensures the GUID is in the specified collection of allowed values.
	/// </summary>
	/// <param name="allowedGuids">The collection of GUIDs that are allowed. Cannot be null or empty.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="allowedGuids"/> is null.</exception>
	/// <exception cref="ArgumentException">Thrown when <paramref name="allowedGuids"/> is empty.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public GuidInputBuilder WithAllowed(
		IEnumerable<Guid> allowedGuids,
		string? customMessage = null
	)
	{
		ArgumentNullException.ThrowIfNull(allowedGuids);

		IEnumerable<Guid> enumerable = allowedGuids.ToList();
		return !enumerable.Any()
			? throw new ArgumentException("Cannot be empty.", nameof(allowedGuids))
			: AddRule(new AllowedGuidRule(enumerable, customMessage));
	}

	/// <summary>
	/// Adds a custom validation rule to the handler.
	/// </summary>
	/// <param name="predicate">The function that determines whether a GUID is valid. Cannot be null.</param>
	/// <param name="errorMessage">The error message to display when validation fails. Cannot be null.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> or <paramref name="errorMessage"/> is null.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public GuidInputBuilder WithCustomRule(Func<Guid, bool> predicate, string errorMessage)
	{
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentNullException.ThrowIfNull(errorMessage);

		return AddRule(new CustomRule<Guid>(predicate, errorMessage));
	}
}

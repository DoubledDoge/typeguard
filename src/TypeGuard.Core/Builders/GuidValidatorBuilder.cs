namespace TypeGuard.Core.Builders;

using Abstractions;
using Rules;
using Validators;

/// <summary>
/// A fluent builder for constructing and configuring a GUID validator with validation rules.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
public class GuidValidatorBuilder(
    string prompt,
    IInputProvider inputProvider,
    IOutputProvider outputProvider
)
{
    private readonly GuidValidator _validator = new(inputProvider, outputProvider, prompt);

    /// <summary>
    /// Adds a validation rule that ensures the GUID is not empty (not <see cref="Guid.Empty"/>).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public GuidValidatorBuilder WithNonEmpty(string? customMessage = null)
    {
        _validator.AddRule(new NonEmptyGuidRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the GUID matches a specific version.
    /// </summary>
    /// <param name="version">The GUID version to validate against (1-5).</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public GuidValidatorBuilder WithVersion(int version, string? customMessage = null)
    {
        _validator.AddRule(new GuidVersionRule(version, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the GUID is not in a list of excluded values.
    /// </summary>
    /// <param name="excludedGuids">The collection of GUIDs that are not allowed.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public GuidValidatorBuilder WithExcluded(IEnumerable<Guid> excludedGuids, string? customMessage = null)
    {
        _validator.AddRule(new ExcludedGuidRule(excludedGuids, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the GUID is in a list of allowed values.
    /// </summary>
    /// <param name="allowedGuids">The collection of GUIDs that are allowed.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public GuidValidatorBuilder WithAllowed(IEnumerable<Guid> allowedGuids, string? customMessage = null)
    {
        _validator.AddRule(new AllowedGuidRule(allowedGuids, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a custom validation rule to the validator.
    /// </summary>
    /// <param name="predicate">The function that determines whether a GUID is valid.</param>
    /// <param name="errorMessage">The error message to display when validation fails.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public GuidValidatorBuilder WithCustomRule(Func<Guid, bool> predicate, string errorMessage)
    {
        _validator.AddRule(new CustomRule<Guid>(predicate, errorMessage));
        return this;
    }

    /// <summary>
    /// Asynchronously prompts the user for input and returns the validated GUID.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated GUID.</returns>
    public async Task<Guid> GetAsync(CancellationToken cancellationToken = default) =>
        await _validator.GetValidInputAsync(cancellationToken);

    /// <summary>
    /// Synchronously prompts the user for input and returns the validated GUID.
    /// </summary>
    /// <returns>The validated GUID.</returns>
    public Guid Get() => _validator.GetValidInput();
}

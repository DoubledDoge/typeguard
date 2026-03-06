namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
/// A base class for character validation rules that apply a single predicate.
/// </summary>
/// <param name="predicate">The function that determines whether a character is valid.</param>
/// <param name="defaultMessage">The default error message used when no custom message is provided.</param>
/// <param name="customMessage">An optional custom error message.</param>
public abstract class RulesBase<T>(
    Func<T, bool> predicate,
    string defaultMessage,
    string? customMessage = null
) : IValidatorRule<T>
{
    /// <inheritdoc/>
    public bool IsValid(T value) => predicate(value);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? defaultMessage;
}

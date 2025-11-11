using System.Text.RegularExpressions;

namespace TypeGuard.Core.Rules;

/// <summary>
/// A validation rule that ensures a string matches a specified regular expression pattern.
/// </summary>
/// <param name="pattern">The regular expression pattern that the string must match.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class RegexRule(string pattern, string? customMessage = null) : IValidationRule<string>
{
    private readonly Regex _regex = new(pattern);

    /// <summary>
    /// Determines whether the specified string matches the regular expression pattern.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string matches the pattern; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => _regex.IsMatch(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Value must match pattern: {pattern}";
}

namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
/// A validation rule that ensures a character is a letter.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class LetterRule(string? customMessage = null)
    : RulesBase<char>(char.IsLetter, "Character must be a letter", customMessage);

/// <summary>
/// A validation rule that ensures a character is a digit.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class DigitRule(string? customMessage = null)
    : RulesBase<char>(char.IsDigit, "Character must be a digit", customMessage);

/// <summary>
/// A validation rule that ensures a character is uppercase.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class UpperCaseRule(string? customMessage = null)
    : RulesBase<char>(char.IsUpper, "Character must be uppercase", customMessage);

/// <summary>
/// A validation rule that ensures a character is lowercase.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class LowerCaseRule(string? customMessage = null)
    : RulesBase<char>(char.IsLower, "Character must be lowercase", customMessage);

/// <summary>
/// A validation rule that ensures a character is alphanumeric (letter or digit).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class AlphanumericCharRule(string? customMessage = null)
    : RulesBase<char>(char.IsLetterOrDigit, "Character must be alphanumeric", customMessage);

/// <summary>
/// A validation rule that ensures a character is whitespace.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class WhitespaceRule(string? customMessage = null)
    : RulesBase<char>(char.IsWhiteSpace, "Character must be whitespace", customMessage);

/// <summary>
/// A validation rule that ensures a character is a punctuation mark.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class PunctuationRule(string? customMessage = null)
    : RulesBase<char>(char.IsPunctuation, "Character must be punctuation", customMessage);

/// <summary>
/// A validation rule that ensures a character is within a specified set of allowed characters.
/// </summary>
/// <remarks>
/// The allowed character set and the default error message are both captured at construction time.
/// </remarks>
/// <param name="allowedChars">The string containing all allowed characters. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="allowedChars"/> is null or empty.</exception>
public class AllowedCharsRule(string allowedChars, string? customMessage = null)
    : IValidatorRule<char>
{
    private readonly HashSet<char> _allowed = string.IsNullOrEmpty(allowedChars)
        ? throw new ArgumentException("Cannot be null or empty.", nameof(allowedChars))
        : [.. allowedChars];

    /// <inheritdoc/>
    public bool IsValid(char value) => _allowed.Contains(value);

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage ?? $"Character must be one of: {allowedChars}";
}

/// <summary>
/// A validation rule that ensures a character is not within a specified set of excluded characters.
/// </summary>
/// <remarks>
/// The excluded character set and the default error message are both captured at construction time.
/// </remarks>
/// <param name="excludedChars">The string containing all excluded characters. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="excludedChars"/> is null or empty.</exception>
public class ExcludedCharsRule(string excludedChars, string? customMessage = null)
    : IValidatorRule<char>
{
    private readonly HashSet<char> _excluded = string.IsNullOrEmpty(excludedChars)
        ? throw new ArgumentException("Cannot be null or empty.", nameof(excludedChars))
        : [.. excludedChars];

    /// <inheritdoc/>
    public bool IsValid(char value) => !_excluded.Contains(value);

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage ?? $"Character cannot be one of: {excludedChars}";
}

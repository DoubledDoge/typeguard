namespace TypeGuard.Core.Rules;

/// <summary>
/// A validation rule that ensures a character is a letter.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class LetterRule(string? customMessage = null) : IValidationRule<char>
{
    /// <summary>
    /// Determines whether the specified character is a letter.
    /// </summary>
    /// <param name="value">The character to validate.</param>
    /// <returns><c>true</c> if the character is a letter; otherwise, <c>false</c>.</returns>
    public bool IsValid(char value) => char.IsLetter(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Character must be a letter";
}

/// <summary>
/// A validation rule that ensures a character is a digit.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class DigitRule(string? customMessage = null) : IValidationRule<char>
{
    /// <summary>
    /// Determines whether the specified character is a digit.
    /// </summary>
    /// <param name="value">The character to validate.</param>
    /// <returns><c>true</c> if the character is a digit; otherwise, <c>false</c>.</returns>
    public bool IsValid(char value) => char.IsDigit(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Character must be a digit";
}

/// <summary>
/// A validation rule that ensures a character is uppercase.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class UpperCaseRule(string? customMessage = null) : IValidationRule<char>
{
    /// <summary>
    /// Determines whether the specified character is uppercase.
    /// </summary>
    /// <param name="value">The character to validate.</param>
    /// <returns><c>true</c> if the character is uppercase; otherwise, <c>false</c>.</returns>
    public bool IsValid(char value) => char.IsUpper(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Character must be uppercase";
}

/// <summary>
/// A validation rule that ensures a character is lowercase.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class LowerCaseRule(string? customMessage = null) : IValidationRule<char>
{
    /// <summary>
    /// Determines whether the specified character is lowercase.
    /// </summary>
    /// <param name="value">The character to validate.</param>
    /// <returns><c>true</c> if the character is lowercase; otherwise, <c>false</c>.</returns>
    public bool IsValid(char value) => char.IsLower(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Character must be lowercase";
}

/// <summary>
/// A validation rule that ensures a character is alphanumeric (letter or digit).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class AlphanumericCharRule(string? customMessage = null) : IValidationRule<char>
{
    /// <summary>
    /// Determines whether the specified character is alphanumeric.
    /// </summary>
    /// <param name="value">The character to validate.</param>
    /// <returns><c>true</c> if the character is a letter or digit; otherwise, <c>false</c>.</returns>
    public bool IsValid(char value) => char.IsLetterOrDigit(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Character must be alphanumeric";
}

/// <summary>
/// A validation rule that ensures a character is whitespace.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class WhitespaceRule(string? customMessage = null) : IValidationRule<char>
{
    /// <summary>
    /// Determines whether the specified character is whitespace.
    /// </summary>
    /// <param name="value">The character to validate.</param>
    /// <returns><c>true</c> if the character is whitespace; otherwise, <c>false</c>.</returns>
    public bool IsValid(char value) => char.IsWhiteSpace(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Character must be whitespace";
}

/// <summary>
/// A validation rule that ensures a character is a punctuation mark.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class PunctuationRule(string? customMessage = null) : IValidationRule<char>
{
    /// <summary>
    /// Determines whether the specified character is a punctuation mark.
    /// </summary>
    /// <param name="value">The character to validate.</param>
    /// <returns><c>true</c> if the character is punctuation; otherwise, <c>false</c>.</returns>
    public bool IsValid(char value) => char.IsPunctuation(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Character must be punctuation";
}

/// <summary>
/// A validation rule that ensures a character is within a specified set of allowed characters.
/// </summary>
/// <param name="allowedChars">The string containing all allowed characters.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class AllowedCharsRule(string allowedChars, string? customMessage = null)
    : IValidationRule<char>
{
    private readonly HashSet<char> _allowed = [.. allowedChars];

    /// <summary>
    /// Determines whether the specified character is in the allowed set.
    /// </summary>
    /// <param name="value">The character to validate.</param>
    /// <returns><c>true</c> if the character is in the allowed set; otherwise, <c>false</c>.</returns>
    public bool IsValid(char value) => _allowed.Contains(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } =
        customMessage ?? $"Character must be one of: {allowedChars}";
}

/// <summary>
/// A validation rule that ensures a character is not within a specified set of excluded characters.
/// </summary>
/// <param name="excludedChars">The string containing all excluded characters.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class ExcludedCharsRule(string excludedChars, string? customMessage = null)
    : IValidationRule<char>
{
    private readonly HashSet<char> _excluded = [.. excludedChars];

    /// <summary>
    /// Determines whether the specified character is not in the excluded set.
    /// </summary>
    /// <param name="value">The character to validate.</param>
    /// <returns><c>true</c> if the character is not in the excluded set; otherwise, <c>false</c>.</returns>
    public bool IsValid(char value) => !_excluded.Contains(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } =
        customMessage ?? $"Character cannot be one of: {excludedChars}";
}

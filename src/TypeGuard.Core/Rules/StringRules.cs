using System.Text.RegularExpressions;

namespace TypeGuard.Core.Rules;

/// <summary>
/// A validation rule that ensures a string's length falls within specified minimum and maximum length.
/// </summary>
/// <param name="minLength">The minimum acceptable length.</param>
/// <param name="maxLength">The maximum acceptable length.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message based on the constraints is used.</param>
public class StringLengthRule(
    int? minLength = null,
    int? maxLength = null,
    string? customMessage = null
) : IValidationRule<string>
{
    /// <summary>
    /// Determines whether the specified string's length falls within the configured bounds.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string length meets the minimum and maximum requirements; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value)
    {
        int length = value.Length;

        if (length < minLength)
            return false;

        return length <= maxLength;
    }

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } =
        customMessage
        ?? (
            minLength.HasValue && maxLength.HasValue
                ? $"Length must be between {minLength} and {maxLength} characters"
            : minLength.HasValue ? $"Length must be at least {minLength} characters"
            : maxLength.HasValue ? $"Length must be at most {maxLength} characters"
            : "Invalid length"
        );
}

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

/// <summary>
/// A validation rule that ensures a string contains only letters.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class AlphabeticRule(string? customMessage = null) : IValidationRule<string>
{
    /// <summary>
    /// Determines whether the specified string contains only letters.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string contains only letters; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => value.All(char.IsLetter);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Input must contain only letters";
}

/// <summary>
/// A validation rule that ensures a string contains only letters and digits.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class AlphanumericStringRule(string? customMessage = null) : IValidationRule<string>
{
    /// <summary>
    /// Determines whether the specified string contains only letters and digits.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string contains only letters and digits; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => value.All(char.IsLetterOrDigit);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } =
        customMessage ?? "Input must contain only letters and digits";
}

/// <summary>
/// A validation rule that ensures a string contains only digits.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NumericRule(string? customMessage = null) : IValidationRule<string>
{
    /// <summary>
    /// Determines whether the specified string contains only digits.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string contains only digits; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => value.All(char.IsDigit);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Input must contain only digits";
}

/// <summary>
/// A validation rule that ensures a string contains only uppercase letters.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class UpperCaseStringRule(string? customMessage = null) : IValidationRule<string>
{
    /// <summary>
    /// Determines whether the specified string contains only uppercase letters.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if all letters in the string are uppercase; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => value.All(c => !char.IsLetter(c) || char.IsUpper(c));

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Input must be uppercase";
}

/// <summary>
/// A validation rule that ensures a string contains only lowercase letters.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class LowerCaseStringRule(string? customMessage = null) : IValidationRule<string>
{
    /// <summary>
    /// Determines whether the specified string contains only lowercase letters.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if all letters in the string are lowercase; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => value.All(c => !char.IsLetter(c) || char.IsLower(c));

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Input must be lowercase";
}

/// <summary>
/// A validation rule that ensures a string starts with a specific prefix.
/// </summary>
/// <param name="prefix">The required prefix.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class StartsWithRule(string prefix, string? customMessage = null) : IValidationRule<string>
{
    /// <summary>
    /// Determines whether the specified string starts with the required prefix.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string starts with the prefix; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => value.StartsWith(prefix);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Input must start with '{prefix}'";
}

/// <summary>
/// A validation rule that ensures a string ends with a specific suffix.
/// </summary>
/// <param name="suffix">The required suffix.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class EndsWithRule(string suffix, string? customMessage = null) : IValidationRule<string>
{
    /// <summary>
    /// Determines whether the specified string ends with the required suffix.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string ends with the suffix; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => value.EndsWith(suffix);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Input must end with '{suffix}'";
}

/// <summary>
/// A validation rule that ensures a string contains a specific substring.
/// </summary>
/// <param name="substring">The required substring.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class ContainsRule(string substring, string? customMessage = null) : IValidationRule<string>
{
    /// <summary>
    /// Determines whether the specified string contains the required substring.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string contains the substring; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => value.Contains(substring);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Input must contain '{substring}'";
}

/// <summary>
/// A validation rule that ensures a string does not contain a specific substring.
/// </summary>
/// <param name="substring">The forbidden substring.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NotContainsRule(string substring, string? customMessage = null)
    : IValidationRule<string>
{
    /// <summary>
    /// Determines whether the specified string does not contain the forbidden substring.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string does not contain the substring; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => !value.Contains(substring);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Input must not contain '{substring}'";
}

/// <summary>
/// A validation rule that ensures a string matches one of the allowed values (case-sensitive).
/// </summary>
/// <param name="allowedValues">The collection of allowed string values.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class AllowedValuesRule(IEnumerable<string> allowedValues, string? customMessage = null)
    : IValidationRule<string>
{
    private readonly HashSet<string> _allowed = [.. allowedValues];

    /// <summary>
    /// Determines whether the specified string matches one of the allowed values.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string is in the allowed set; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => _allowed.Contains(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage =>
        customMessage ?? $"Input must be one of: {string.Join(", ", _allowed)}";
}

/// <summary>
/// A validation rule that ensures a string does not match any of the excluded values (case-sensitive).
/// </summary>
/// <param name="excludedValues">The collection of excluded string values.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class ExcludedValuesRule(IEnumerable<string> excludedValues, string? customMessage = null)
    : IValidationRule<string>
{
    private readonly HashSet<string> _excluded = [.. excludedValues];

    /// <summary>
    /// Determines whether the specified string does not match any of the excluded values.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string is not in the excluded set; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => !_excluded.Contains(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Input value is not allowed";
}

/// <summary>
/// A validation rule that ensures a string is a valid email address format.
/// Uses a regex pattern that covers most common email formats.
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public partial class EmailRule(string? customMessage = null) : IValidationRule<string>
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex EmailRegex();

    /// <summary>
    /// Determines whether the specified string is a valid email address.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string is a valid email format; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => EmailRegex().IsMatch(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Input must be a valid email address";
}

/// <summary>
/// A validation rule that ensures a string contains only digits, spaces, hyphens, parentheses, and plus signs.
/// Does not enforce a specific phone number format to allow international use.
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public partial class PhoneRule(string? customMessage = null) : IValidationRule<string>
{
    [GeneratedRegex(@"^[\d\s\-\(\)\+]+$", RegexOptions.Compiled)]
    private static partial Regex PhoneRegex();

    /// <summary>
    /// Determines whether the specified string contains only valid phone number characters.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string contains only phone number characters; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value) => PhoneRegex().IsMatch(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Input must be a valid phone number";
}

/// <summary>
/// A validation rule that ensures a string is a valid file path and optionally checks if the file exists.
/// </summary>
/// <param name="mustExist">If true, validates that the file actually exists on the file system.</param>
/// <param name="customMessage">An optional custom error message.</param>
public class FilePathRule(bool mustExist = false, string? customMessage = null) : IValidationRule<string>
{
    /// <summary>
    /// Determines whether the specified string is a valid file path.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <returns><c>true</c> if the string is a valid file path and meets existence requirements; otherwise, <c>false</c>.</returns>
    public bool IsValid(string value)
    {
        try
        {
            string fullPath = Path.GetFullPath(value);

            return !mustExist || File.Exists(fullPath);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? (mustExist ? "File path must exist" : "Input must be a valid file path");
}

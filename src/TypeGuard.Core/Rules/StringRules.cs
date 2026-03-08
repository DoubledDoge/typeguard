using System.Text.RegularExpressions;

namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
/// A validation rule that ensures a string's length falls within the specified minimum and maximum bounds.
/// </summary>
/// <param name="minLength">The optional minimum acceptable length (inclusive). Must be greater than or equal to zero if provided.</param>
/// <param name="maxLength">The optional maximum acceptable length (inclusive). Must be greater than or equal to zero if provided.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message based on the constraints is used.</param>
/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="minLength"/> or <paramref name="maxLength"/> is negative.</exception>
/// <exception cref="ArgumentException">Thrown when <paramref name="minLength"/> is greater than <paramref name="maxLength"/>.</exception>
public class StringLengthRule(
    int? minLength = null,
    int? maxLength = null,
    string? customMessage = null
) : IValidatorRule<string>
{
    private readonly int? _minLength = ValidateArgs(minLength, maxLength);

    /// <inheritdoc/>
    public bool IsValid(string value)
    {
        int length = value.Length;

        return (!_minLength.HasValue || length >= _minLength.Value)
            && (!maxLength.HasValue || length <= maxLength.Value);
    }

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage
        ?? (
            minLength.HasValue && maxLength.HasValue
                ? $"Length must be between {minLength} and {maxLength} characters"
            : minLength.HasValue ? $"Length must be at least {minLength} characters"
            : maxLength.HasValue ? $"Length must be at most {maxLength} characters"
            : "Invalid length"
        );

    private static int? ValidateArgs(int? min, int? max)
    {
        return min is < 0
                ? throw new ArgumentOutOfRangeException(
                    nameof(minLength),
                    min,
                    "minLength must be greater than or equal to zero."
                )
            : max is < 0
                ? throw new ArgumentOutOfRangeException(
                    nameof(maxLength),
                    max,
                    "maxLength must be greater than or equal to zero."
                )
            : min.HasValue && max.HasValue && min.Value > max.Value
                ? throw new ArgumentException(
                    $"minLength ({min}) must be less than or equal to maxLength ({max}).",
                    nameof(minLength)
                )
            : min;
    }
}

/// <summary>
/// A validation rule that ensures a string matches a specified regular expression pattern.
/// </summary>
/// <param name="pattern">The regular expression pattern the string must match. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="pattern"/> is null or empty.</exception>
public class RegexRule(string pattern, string? customMessage = null) : IValidatorRule<string>
{
    private readonly Regex _regex = string.IsNullOrEmpty(pattern)
        ? throw new ArgumentException("Cannot be null or empty.", nameof(pattern))
        : new Regex(pattern, RegexOptions.Compiled);

    /// <inheritdoc/>
    public bool IsValid(string value) => _regex.IsMatch(value);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? $"Input must match pattern: {pattern}";
}

/// <summary>
/// A validation rule that ensures a string contains only letters.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class AlphabeticRule(string? customMessage = null)
    : RulesBase<string>(
        v => v.All(char.IsLetter),
        "Input must contain only letters",
        customMessage
    );

/// <summary>
/// A validation rule that ensures a string contains only letters and digits.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class AlphanumericStringRule(string? customMessage = null)
    : RulesBase<string>(
        v => v.All(char.IsLetterOrDigit),
        "Input must contain only letters and digits",
        customMessage
    );

/// <summary>
/// A validation rule that ensures a string contains only digits.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NumericRule(string? customMessage = null)
    : RulesBase<string>(v => v.All(char.IsDigit), "Input must contain only digits", customMessage);

/// <summary>
/// A validation rule that ensures a string contains only uppercase letters.
/// Non-letter characters are permitted.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class UpperCaseStringRule(string? customMessage = null)
    : RulesBase<string>(
        v => v.All(c => !char.IsLetter(c) || char.IsUpper(c)),
        "Input must be uppercase",
        customMessage
    );

/// <summary>
/// A validation rule that ensures a string contains only lowercase letters.
/// Non-letter characters are permitted.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class LowerCaseStringRule(string? customMessage = null)
    : RulesBase<string>(
        v => v.All(c => !char.IsLetter(c) || char.IsLower(c)),
        "Input must be lowercase",
        customMessage
    );

/// <summary>
/// A validation rule that ensures a string starts with a specific prefix.
/// </summary>
/// <param name="prefix">The required prefix. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="prefix"/> is null or empty.</exception>
public class StartsWithRule(string prefix, string? customMessage = null)
    : RulesBase<string>(BuildPredicate(prefix), $"Input must start with '{prefix}'", customMessage)
{
    private static Func<string, bool> BuildPredicate(string prefix)
    {
        return string.IsNullOrEmpty(prefix)
            ? throw new ArgumentException("Cannot be null or empty.", nameof(prefix))
            : v => v.StartsWith(prefix);
    }
}

/// <summary>
/// A validation rule that ensures a string ends with a specific suffix.
/// </summary>
/// <param name="suffix">The required suffix. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="suffix"/> is null or empty.</exception>
public class EndsWithRule(string suffix, string? customMessage = null)
    : RulesBase<string>(BuildPredicate(suffix), $"Input must end with '{suffix}'", customMessage)
{
    private static Func<string, bool> BuildPredicate(string suffix)
    {
        return string.IsNullOrEmpty(suffix)
            ? throw new ArgumentException("Cannot be null or empty.", nameof(suffix))
            : v => v.EndsWith(suffix);
    }
}

/// <summary>
/// A validation rule that ensures a string contains a specific substring.
/// </summary>
/// <param name="substring">The required substring. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="substring"/> is null or empty.</exception>
public class ContainsRule(string substring, string? customMessage = null)
    : RulesBase<string>(
        BuildPredicate(substring),
        $"Input must contain '{substring}'",
        customMessage
    )
{
    private static Func<string, bool> BuildPredicate(string substring)
    {
        return string.IsNullOrEmpty(substring)
            ? throw new ArgumentException("Cannot be null or empty.", nameof(substring))
            : v => v.Contains(substring);
    }
}

/// <summary>
/// A validation rule that ensures a string does not contain a specific substring.
/// </summary>
/// <param name="substring">The forbidden substring. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="substring"/> is null or empty.</exception>
public class NotContainsRule(string substring, string? customMessage = null)
    : RulesBase<string>(
        BuildPredicate(substring),
        $"Input must not contain '{substring}'",
        customMessage
    )
{
    private static Func<string, bool> BuildPredicate(string substring)
    {
        return string.IsNullOrEmpty(substring)
            ? throw new ArgumentException("Cannot be null or empty.", nameof(substring))
            : v => !v.Contains(substring);
    }
}

/// <summary>
/// A validation rule that ensures a string matches one of the allowed values (case-sensitive).
/// </summary>
/// <param name="allowedValues">The collection of allowed string values. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="allowedValues"/> is null.</exception>
/// <exception cref="ArgumentException">Thrown when <paramref name="allowedValues"/> is empty.</exception>
public class AllowedValuesRule(IEnumerable<string> allowedValues, string? customMessage = null)
    : IValidatorRule<string>
{
    private readonly (HashSet<string> Set, string Joined) _built = BuildSet(
        allowedValues,
        nameof(allowedValues)
    );

    /// <inheritdoc/>
    public bool IsValid(string value) => _built.Set.Contains(value);

    /// <inheritdoc/>
    public string ErrorMessage => customMessage ?? $"Input must be one of: {_built.Joined}";

    private static (HashSet<string> Set, string Joined) BuildSet(
        IEnumerable<string> values,
        string paramName
    )
    {
        ArgumentNullException.ThrowIfNull(values, paramName);
        HashSet<string> set = [.. values];
        return set.Count == 0
            ? throw new ArgumentException("Cannot be empty.", paramName)
            : (set, string.Join(", ", set));
    }
}

/// <summary>
/// A validation rule that ensures a string does not match any of the excluded values (case-sensitive).
/// </summary>
/// <param name="excludedValues">The collection of excluded string values. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="excludedValues"/> is null.</exception>
/// <exception cref="ArgumentException">Thrown when <paramref name="excludedValues"/> is empty.</exception>
public class ExcludedValuesRule(IEnumerable<string> excludedValues, string? customMessage = null)
    : IValidatorRule<string>
{
    private readonly (HashSet<string> Set, string Joined) _built = BuildSet(
        excludedValues,
        nameof(excludedValues)
    );

    /// <inheritdoc/>
    public bool IsValid(string value) => !_built.Set.Contains(value);

    /// <inheritdoc/>
    public string ErrorMessage => customMessage ?? $"Input must not be one of: {_built.Joined}";

    private static (HashSet<string> Set, string Joined) BuildSet(
        IEnumerable<string> values,
        string paramName
    )
    {
        ArgumentNullException.ThrowIfNull(values, paramName);
        HashSet<string> set = [.. values];
        return set.Count == 0
            ? throw new ArgumentException("Cannot be empty.", paramName)
            : (set, string.Join(", ", set));
    }
}

/// <summary>
/// A validation rule that ensures a string is a valid email address format.
/// Uses a pattern that covers most common email formats.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public partial class EmailRule(string? customMessage = null) : IValidatorRule<string>
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex EmailRegex();

    /// <inheritdoc/>
    public bool IsValid(string value) => EmailRegex().IsMatch(value);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "Input must be a valid email address";
}

/// <summary>
/// A validation rule that ensures a string is a valid phone number format.
/// Permits digits, spaces, hyphens, parentheses, and plus signs to support international formats.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public partial class PhoneRule(string? customMessage = null) : IValidatorRule<string>
{
    [GeneratedRegex(@"^[\d\s\-\(\)\+]+$", RegexOptions.Compiled)]
    private static partial Regex PhoneRegex();

    /// <inheritdoc/>
    public bool IsValid(string value) => PhoneRegex().IsMatch(value);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "Input must be a valid phone number";
}

/// <summary>
/// A validation rule that ensures a string is a valid file path, and optionally that the file exists.
/// </summary>
/// <param name="mustExist">If true, validates that the file actually exists on the file system. Defaults to false.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when the file path is invalid.</exception>
public class FilePathRule(bool mustExist = false, string? customMessage = null)
    : RulesBase<string>(BuildPredicate(mustExist), BuildMessage(mustExist), customMessage)
{
    private static Func<string, bool> BuildPredicate(bool mustExist)
    {
        return value =>
        {
            string fullPath = Path.GetFullPath(value);
            return !mustExist || File.Exists(fullPath);
        };
    }

    private static string BuildMessage(bool mustExist) =>
        mustExist ? "File path must exist" : "Input must be a valid file path";
}

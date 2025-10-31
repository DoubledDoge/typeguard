using System.Text.RegularExpressions;

namespace TypeGuard.Core.Rules;

public class RegexRule(string pattern, string? customMessage = null) : IValidationRule<string>
{
    private readonly Regex _regex = new(pattern);

    public bool IsValid(string value) => _regex.IsMatch(value);

    public string ErrorMessage { get; } = customMessage ?? $"Value must match pattern: {pattern}";
}

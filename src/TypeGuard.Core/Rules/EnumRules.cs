namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
/// A validation rule that ensures an enum value is defined.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class DefinedEnumRule<TEnum>(string? customMessage = null) : IValidatorRule<TEnum>
    where TEnum : struct, Enum
{
    /// <inheritdoc/>
    public bool IsValid(TEnum value) => Enum.IsDefined(value);

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage ?? $"Value must be a defined {typeof(TEnum).Name}";
}

/// <summary>
/// A validation rule that ensures an enum value is not the default value.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NotDefaultEnumRule<TEnum>(string? customMessage = null) : IValidatorRule<TEnum>
    where TEnum : struct, Enum
{
    /// <inheritdoc/>
    public bool IsValid(TEnum value) => !value.Equals(default(TEnum));

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage ?? $"Value cannot be the default {typeof(TEnum).Name}";
}

/// <summary>
/// A validation rule that ensures an enum value is one of the allowed values.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="allowedValues">The collection of allowed enum values. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="allowedValues"/> is null.</exception>
/// <exception cref="ArgumentException">Thrown when <paramref name="allowedValues"/> is empty.</exception>
public class AllowedEnumValuesRule<TEnum>(
    IEnumerable<TEnum> allowedValues,
    string? customMessage = null
) : IValidatorRule<TEnum>
    where TEnum : struct, Enum
{
    private readonly (HashSet<TEnum> Set, string Joined) _built = BuildSet(
        allowedValues,
        nameof(allowedValues)
    );

    /// <inheritdoc/>
    public bool IsValid(TEnum value) => _built.Set.Contains(value);

    /// <inheritdoc/>
    public string ErrorMessage => customMessage ?? $"Value must be one of: {_built.Joined}";

    private static (HashSet<TEnum> Set, string Joined) BuildSet(
        IEnumerable<TEnum> values,
        string paramName
    )
    {
        ArgumentNullException.ThrowIfNull(values, paramName);
        HashSet<TEnum> set = [.. values];
        return set.Count == 0
            ? throw new ArgumentException("Cannot be empty.", paramName)
            : (set, string.Join(", ", set));
    }
}

/// <summary>
/// A validation rule that ensures an enum value is not one of the excluded values.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="excludedValues">The collection of excluded enum values. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="excludedValues"/> is null.</exception>
/// <exception cref="ArgumentException">Thrown when <paramref name="excludedValues"/> is empty.</exception>
public class ExcludedEnumValuesRule<TEnum>(
    IEnumerable<TEnum> excludedValues,
    string? customMessage = null
) : IValidatorRule<TEnum>
    where TEnum : struct, Enum
{
    private readonly (HashSet<TEnum> Set, string Joined) _built = BuildSet(
        excludedValues,
        nameof(excludedValues)
    );

    /// <inheritdoc/>
    public bool IsValid(TEnum value) => !_built.Set.Contains(value);

    /// <inheritdoc/>
    public string ErrorMessage => customMessage ?? $"Value must not be one of: {_built.Joined}";

    private static (HashSet<TEnum> Set, string Joined) BuildSet(
        IEnumerable<TEnum> values,
        string paramName
    )
    {
        ArgumentNullException.ThrowIfNull(values, paramName);
        HashSet<TEnum> set = [.. values];
        return set.Count == 0
            ? throw new ArgumentException("Cannot be empty.", paramName)
            : (set, string.Join(", ", set));
    }
}

/// <summary>
/// A validation rule that ensures an enum value has the specified flag set.
/// Intended for use with enums decorated with the <c>[Flags]</c> attribute.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="requiredFlag">The flag that must be set.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class HasFlagRule<TEnum>(TEnum requiredFlag, string? customMessage = null)
    : IValidatorRule<TEnum>
    where TEnum : struct, Enum
{
    /// <inheritdoc/>
    public bool IsValid(TEnum value) => value.HasFlag(requiredFlag);

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage ?? $"Value must have the flag {requiredFlag}";
}

/// <summary>
/// A validation rule that ensures an enum value does not have the specified flag set.
/// Intended for use with enums decorated with the <c>[Flags]</c> attribute.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="forbiddenFlag">The flag that must not be set.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NotHasFlagRule<TEnum>(TEnum forbiddenFlag, string? customMessage = null)
    : IValidatorRule<TEnum>
    where TEnum : struct, Enum
{
    /// <inheritdoc/>
    public bool IsValid(TEnum value) => !value.HasFlag(forbiddenFlag);

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage ?? $"Value must not have the flag {forbiddenFlag}";
}

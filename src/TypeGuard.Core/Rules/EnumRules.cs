namespace TypeGuard.Core.Rules;

/// <summary>
/// A validation rule that ensures an enum value is defined.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class DefinedEnumRule<TEnum>(string? customMessage = null) : IValidationRule<TEnum>
    where TEnum : struct, Enum
{
    /// <summary>
    /// Determines whether the specified enum value is defined in the enum type.
    /// </summary>
    /// <param name="value">The enum value to validate.</param>
    /// <returns><c>true</c> if the enum value is defined; otherwise, <c>false</c>.</returns>
    public bool IsValid(TEnum value) => Enum.IsDefined(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Value must be a defined {typeof(TEnum).Name}";
}

/// <summary>
/// A validation rule that ensures an enum value is not the default value.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NotDefaultEnumRule<TEnum>(string? customMessage = null) : IValidationRule<TEnum>
    where TEnum : struct, Enum
{
    /// <summary>
    /// Determines whether the specified enum value is not the default value.
    /// </summary>
    /// <param name="value">The enum value to validate.</param>
    /// <returns><c>true</c> if the enum value is not the default; otherwise, <c>false</c>.</returns>
    public bool IsValid(TEnum value) => !value.Equals(default(TEnum));

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Value cannot be the default {typeof(TEnum).Name}";
}

/// <summary>
/// A validation rule that ensures an enum value is one of the allowed values.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="allowedValues">The collection of allowed enum values.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class AllowedEnumValuesRule<TEnum>(IEnumerable<TEnum> allowedValues, string? customMessage = null) : IValidationRule<TEnum>
    where TEnum : struct, Enum
{
    private readonly HashSet<TEnum> _allowed = [..allowedValues];

    /// <summary>
    /// Determines whether the specified enum value is in the allowed list.
    /// </summary>
    /// <param name="value">The enum value to validate.</param>
    /// <returns><c>true</c> if the enum value is allowed; otherwise, <c>false</c>.</returns>
    public bool IsValid(TEnum value) => _allowed.Contains(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage => customMessage ?? $"Value must be one of: {string.Join(", ", allowedValues)}";
}

/// <summary>
/// A validation rule that ensures an enum value is not one of the excluded values.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="excludedValues">The collection of excluded enum values.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class ExcludedEnumValuesRule<TEnum>(IEnumerable<TEnum> excludedValues, string? customMessage = null) : IValidationRule<TEnum>
    where TEnum : struct, Enum
{
    private readonly HashSet<TEnum> _excluded = [.. excludedValues];

    /// <summary>
    /// Determines whether the specified enum value is not in the excluded list.
    /// </summary>
    /// <param name="value">The enum value to validate.</param>
    /// <returns><c>true</c> if the enum value is not excluded; otherwise, <c>false</c>.</returns>
    public bool IsValid(TEnum value) => !_excluded.Contains(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "Value is not allowed";
}

/// <summary>
/// A validation rule that ensures an enum value has a specific flag set.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate. Should be decorated with the flag attribute.</typeparam>
/// <param name="requiredFlag">The flag that must be set.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class HasFlagRule<TEnum>(TEnum requiredFlag, string? customMessage = null) : IValidationRule<TEnum>
    where TEnum : struct, Enum
{
    /// <summary>
    /// Determines whether the specified enum value has the required flag set.
    /// </summary>
    /// <param name="value">The enum value to validate.</param>
    /// <returns><c>true</c> if the required flag is set; otherwise, <c>false</c>.</returns>
    public bool IsValid(TEnum value) => value.HasFlag(requiredFlag);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Value must have the flag {requiredFlag}";
}

/// <summary>
/// A validation rule that ensures an enum value does not have a specific flag set.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate. Should be decorated with the flags attribute.</typeparam>
/// <param name="forbiddenFlag">The flag that must not be set.</param>
/// <param name="customMessage">An optional custom error message.</param>
public class NotHasFlagRule<TEnum>(TEnum forbiddenFlag, string? customMessage = null) : IValidationRule<TEnum>
    where TEnum : struct, Enum
{
    /// <summary>
    /// Determines whether the specified enum value does not have the forbidden flag set.
    /// </summary>
    /// <param name="value">The enum value to validate.</param>
    /// <returns><c>true</c> if the forbidden flag is not set; otherwise, <c>false</c>.</returns>
    public bool IsValid(TEnum value) => !value.HasFlag(forbiddenFlag);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"Value must not have the flag {forbiddenFlag}";
}

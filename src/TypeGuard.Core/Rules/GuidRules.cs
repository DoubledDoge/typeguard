namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
/// A validation rule that ensures a GUID is not empty (not <see cref="Guid.Empty"/>).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NonEmptyGuidRule(string? customMessage = null)
    : RulesBase<Guid>(v => v != Guid.Empty, "GUID cannot be empty", customMessage);

/// <summary>
/// A validation rule that ensures a GUID matches a specific version.
/// </summary>
/// <param name="version">The GUID version to validate against (1-5).</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="version"/> is not between 1 and 5.</exception>
public class GuidVersionRule(int version, string? customMessage = null) : IValidatorRule<Guid>
{
    private readonly int _version = version is < 1 or > 5
        ? throw new ArgumentOutOfRangeException(
            nameof(version),
            version,
            "version must be between 1 and 5."
        )
        : version;

    /// <inheritdoc/>
    public bool IsValid(Guid value)
    {
        byte[] bytes = value.ToByteArray();
        int versionByte = (bytes[7] & 0xF0) >> 4;
        return versionByte == _version;
    }

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? $"GUID must be version {version}";
}

/// <summary>
/// A validation rule that ensures a GUID is not in the specified collection of excluded values.
/// </summary>
/// <param name="excludedGuids">The collection of GUIDs that are not allowed. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="excludedGuids"/> is null.</exception>
/// <exception cref="ArgumentException">Thrown when <paramref name="excludedGuids"/> is empty.</exception>
public class ExcludedGuidRule(IEnumerable<Guid> excludedGuids, string? customMessage = null)
    : IValidatorRule<Guid>
{
    private readonly HashSet<Guid> _excluded = BuildSet(excludedGuids, nameof(excludedGuids));

    /// <inheritdoc/>
    public bool IsValid(Guid value) => !_excluded.Contains(value);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "This GUID is not allowed";

    private static HashSet<Guid> BuildSet(IEnumerable<Guid> values, string paramName)
    {
        ArgumentNullException.ThrowIfNull(values, paramName);
        HashSet<Guid> set = [.. values];
        return set.Count == 0 ? throw new ArgumentException("Cannot be empty.", paramName) : set;
    }
}

/// <summary>
/// A validation rule that ensures a GUID is in the specified collection of allowed values.
/// </summary>
/// <param name="allowedGuids">The collection of GUIDs that are allowed. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="allowedGuids"/> is null.</exception>
/// <exception cref="ArgumentException">Thrown when <paramref name="allowedGuids"/> is empty.</exception>
public class AllowedGuidRule(IEnumerable<Guid> allowedGuids, string? customMessage = null)
    : IValidatorRule<Guid>
{
    private readonly HashSet<Guid> _allowed = BuildSet(allowedGuids, nameof(allowedGuids));

    /// <inheritdoc/>
    public bool IsValid(Guid value) => _allowed.Contains(value);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "This GUID is not in the allowed list";

    private static HashSet<Guid> BuildSet(IEnumerable<Guid> values, string paramName)
    {
        ArgumentNullException.ThrowIfNull(values, paramName);
        HashSet<Guid> set = [.. values];
        return set.Count == 0 ? throw new ArgumentException("Cannot be empty.", paramName) : set;
    }
}

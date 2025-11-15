namespace TypeGuard.Core.Rules;

/// <summary>
/// A validation rule that ensures a GUID is not empty (not <see cref="Guid.Empty"/>).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NonEmptyGuidRule(string? customMessage = null) : IValidationRule<Guid>
{
    /// <summary>
    /// Determines whether the specified GUID is not empty.
    /// </summary>
    /// <param name="value">The GUID value to validate.</param>
    /// <returns><c>true</c> if the GUID is not <see cref="Guid.Empty"/>; otherwise, <c>false</c>.</returns>
    public bool IsValid(Guid value) => value != Guid.Empty;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "GUID cannot be empty";
}

/// <summary>
/// A validation rule that ensures a GUID matches a specific version.
/// </summary>
/// <param name="version">The GUID version to validate against (1-5).</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class GuidVersionRule(int version, string? customMessage = null) : IValidationRule<Guid>
{
    /// <summary>
    /// Determines whether the specified GUID matches the expected version.
    /// </summary>
    /// <param name="value">The GUID value to validate.</param>
    /// <returns><c>true</c> if the GUID version matches; otherwise, <c>false</c>.</returns>
    public bool IsValid(Guid value)
    {
        byte[] bytes = value.ToByteArray();
        int versionByte = (bytes[7] & 0xF0) >> 4;
        return versionByte == version;
    }

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"GUID must be version {version}";
}

/// <summary>
/// A validation rule that ensures a GUID is not in a list of excluded values.
/// </summary>
/// <param name="excludedGuids">The collection of GUIDs that are not allowed.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class ExcludedGuidRule(IEnumerable<Guid> excludedGuids, string? customMessage = null)
    : IValidationRule<Guid>
{
    private readonly HashSet<Guid> _excluded = [.. excludedGuids];

    /// <summary>
    /// Determines whether the specified GUID is not in the excluded list.
    /// </summary>
    /// <param name="value">The GUID value to validate.</param>
    /// <returns><c>true</c> if the GUID is not in the excluded list; otherwise, <c>false</c>.</returns>
    public bool IsValid(Guid value) => !_excluded.Contains(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "This GUID is not allowed";
}

/// <summary>
/// A validation rule that ensures a GUID is in a list of allowed values.
/// </summary>
/// <param name="allowedGuids">The collection of GUIDs that are allowed.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class AllowedGuidRule(IEnumerable<Guid> allowedGuids, string? customMessage = null)
    : IValidationRule<Guid>
{
    private readonly HashSet<Guid> _allowed = [.. allowedGuids];

    /// <summary>
    /// Determines whether the specified GUID is in the allowed list.
    /// </summary>
    /// <param name="value">The GUID value to validate.</param>
    /// <returns><c>true</c> if the GUID is in the allowed list; otherwise, <c>false</c>.</returns>
    public bool IsValid(Guid value) => _allowed.Contains(value);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "This GUID is not in the allowed list";
}

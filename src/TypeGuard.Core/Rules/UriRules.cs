namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
/// A validation rule that ensures a URI uses the specified scheme (e.g., "http", "https", "ftp").
/// </summary>
/// <param name="scheme">The required URI scheme. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="scheme"/> is null or empty.</exception>
public class UriSchemeRule(string scheme, string? customMessage = null) : IValidationRule<Uri>
{
    private readonly string _scheme = string.IsNullOrEmpty(scheme)
        ? throw new ArgumentException("Cannot be null or empty.", nameof(scheme))
        : scheme;

    /// <inheritdoc/>
    public bool IsValid(Uri value) =>
        value.Scheme.Equals(_scheme, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? $"URI must use the {scheme} scheme";
}

/// <summary>
/// A validation rule that ensures a URI uses HTTPS only.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class HttpsOnlyRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <inheritdoc/>
    public bool IsValid(Uri value) =>
        value.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "URI must use HTTPS";
}

/// <summary>
/// A validation rule that ensures a URI uses either HTTP or HTTPS.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class HttpOrHttpsRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <inheritdoc/>
    public bool IsValid(Uri value) =>
        value.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase)
        || value.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "URI must use HTTP or HTTPS";
}

/// <summary>
/// A validation rule that ensures a URI belongs to the specified domain.
/// </summary>
/// <param name="domain">The required domain (e.g. "example.com"). Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="domain"/> is null or empty.</exception>
public class DomainRule(string domain, string? customMessage = null) : IValidationRule<Uri>
{
    private readonly string _domain = string.IsNullOrEmpty(domain)
        ? throw new ArgumentException("Cannot be null or empty.", nameof(domain))
        : domain;

    /// <inheritdoc/>
    public bool IsValid(Uri value) =>
        value.Host.Equals(_domain, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? $"URI must be from domain {domain}";
}

/// <summary>
/// A validation rule that ensures a URI belongs to one of the specified allowed domains.
/// </summary>
/// <param name="allowedDomains">The collection of allowed domains. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="allowedDomains"/> is null.</exception>
/// <exception cref="ArgumentException">Thrown when <paramref name="allowedDomains"/> is empty.</exception>
public class AllowedDomainsRule(IEnumerable<string> allowedDomains, string? customMessage = null)
    : IValidationRule<Uri>
{
    private readonly (HashSet<string> Set, string Joined) _built = BuildSet(
        allowedDomains,
        nameof(allowedDomains)
    );

    /// <inheritdoc/>
    public bool IsValid(Uri value) => _built.Set.Contains(value.Host);

    /// <inheritdoc/>
    public string ErrorMessage => customMessage ?? $"URI must be from one of: {_built.Joined}";

    private static (HashSet<string> Set, string Joined) BuildSet(
        IEnumerable<string> values,
        string paramName
    )
    {
        ArgumentNullException.ThrowIfNull(values, paramName);
        HashSet<string> set = new(values, StringComparer.OrdinalIgnoreCase);
        return set.Count == 0
            ? throw new ArgumentException("Cannot be empty.", paramName)
            : (set, string.Join(", ", set));
    }
}

/// <summary>
/// A validation rule that ensures a URI uses the specified port number.
/// </summary>
/// <param name="port">The required port number. Must be between 0 and 65535.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="port"/> is not between 0 and 65535.</exception>
public class PortRule(int port, string? customMessage = null) : IValidationRule<Uri>
{
    private readonly int _port = port is < 0 or > 65535
        ? throw new ArgumentOutOfRangeException(
            nameof(port),
            port,
            "port must be between 0 and 65535."
        )
        : port;

    /// <inheritdoc/>
    public bool IsValid(Uri value) => value.Port == _port;

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? $"URI must use port {port}";
}

/// <summary>
/// A validation rule that ensures a URI is absolute (has a scheme and host).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class AbsoluteUriRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <inheritdoc/>
    public bool IsValid(Uri value) => value.IsAbsoluteUri;

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "URI must be absolute";
}

/// <summary>
/// A validation rule that ensures the URI path starts with the specified prefix.
/// </summary>
/// <param name="pathPrefix">The required path prefix (e.g. "/api/v1"). Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="pathPrefix"/> is null or empty.</exception>
public class PathPrefixRule(string pathPrefix, string? customMessage = null) : IValidationRule<Uri>
{
    private readonly string _pathPrefix = string.IsNullOrEmpty(pathPrefix)
        ? throw new ArgumentException("Cannot be null or empty.", nameof(pathPrefix))
        : pathPrefix;

    /// <inheritdoc/>
    public bool IsValid(Uri value) =>
        value.AbsolutePath.StartsWith(_pathPrefix, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public string ErrorMessage { get; } =
        customMessage ?? $"URI path must start with '{pathPrefix}'";
}

/// <summary>
/// A validation rule that ensures the URI includes a query string.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class HasQueryStringRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <inheritdoc/>
    public bool IsValid(Uri value) => !string.IsNullOrEmpty(value.Query);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "URI must include a query string";
}

/// <summary>
/// A validation rule that ensures the URI does not include a query string.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NoQueryStringRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <inheritdoc/>
    public bool IsValid(Uri value) => string.IsNullOrEmpty(value.Query);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "URI must not include a query string";
}

/// <summary>
/// A validation rule that ensures the URI includes a fragment (the portion after #).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class HasFragmentRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <inheritdoc/>
    public bool IsValid(Uri value) => !string.IsNullOrEmpty(value.Fragment);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "URI must include a fragment";
}

/// <summary>
/// A validation rule that ensures the URI is a localhost address.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class LocalhostRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <inheritdoc/>
    public bool IsValid(Uri value) =>
        value.IsLoopback || value.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public string ErrorMessage { get; } = customMessage ?? "URI must be a localhost address";
}

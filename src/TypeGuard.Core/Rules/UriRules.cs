namespace TypeGuard.Core.Rules;

/// <summary>
/// A validation rule that ensures a Uri uses a specific scheme (e.g., http, https, ftp).
/// </summary>
/// <param name="scheme">The required URI scheme.</param>
/// <param name="customMessage">An optional custom error message.</param>
public class UriSchemeRule(string scheme, string? customMessage = null) : IValidationRule<Uri>
{
    /// <summary>
    /// Determines whether the specified Uri uses the required scheme.
    /// </summary>
    /// <param name="value">The Uri to validate.</param>
    /// <returns><c>true</c> if the Uri uses the specified scheme; otherwise, <c>false</c>.</returns>
    public bool IsValid(Uri value) => value.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"URI must use {scheme} scheme";
}

/// <summary>
/// A validation rule that ensures a Uri uses HTTPS.
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public class HttpsOnlyRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <summary>
    /// Determines whether the specified Uri uses HTTPS.
    /// </summary>
    /// <param name="value">The Uri to validate.</param>
    /// <returns><c>true</c> if the Uri uses HTTPS; otherwise, <c>false</c>.</returns>
    public bool IsValid(Uri value) => value.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "URI must use HTTPS";
}

/// <summary>
/// A validation rule that ensures a Uri uses either HTTP or HTTPS.
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public class HttpOrHttpsRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <summary>
    /// Determines whether the specified Uri uses HTTP or HTTPS.
    /// </summary>
    /// <param name="value">The Uri to validate.</param>
    /// <returns><c>true</c> if the Uri uses HTTP or HTTPS; otherwise, <c>false</c>.</returns>
    public bool IsValid(Uri value) =>
        value.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) ||
        value.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "URI must use HTTP or HTTPS";
}

/// <summary>
/// A validation rule that ensures a Uri belongs to a specific domain.
/// </summary>
/// <param name="domain">The required domain.</param>
/// <param name="customMessage">An optional custom error message.</param>
public class DomainRule(string domain, string? customMessage = null) : IValidationRule<Uri>
{
    /// <summary>
    /// Determines whether the specified Uri belongs to the required domain.
    /// </summary>
    /// <param name="value">The Uri to validate.</param>
    /// <returns><c>true</c> if the Uri's host matches the domain; otherwise, <c>false</c>.</returns>
    public bool IsValid(Uri value) => value.Host.Equals(domain, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"URI must be from domain {domain}";
}

/// <summary>
/// A validation rule that ensures a Uri belongs to one of the allowed domains.
/// </summary>
/// <param name="allowedDomains">The collection of allowed domains.</param>
/// <param name="customMessage">An optional custom error message.</param>
public class AllowedDomainsRule(IEnumerable<string> allowedDomains, string? customMessage = null) : IValidationRule<Uri>
{
    private readonly HashSet<string> _allowed = new(allowedDomains, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Determines whether the specified Uri belongs to one of the allowed domains.
    /// </summary>
    /// <param name="value">The Uri to validate.</param>
    /// <returns><c>true</c> if the Uri's host is in the allowed list; otherwise, <c>false</c>.</returns>
    public bool IsValid(Uri value) => _allowed.Contains(value.Host);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage => customMessage ?? $"URI must be from one of: {string.Join(", ", _allowed)}";
}

/// <summary>
/// A validation rule that ensures a Uri has a specific port number.
/// </summary>
/// <param name="port">The required port number.</param>
/// <param name="customMessage">An optional custom error message.</param>
public class PortRule(int port, string? customMessage = null) : IValidationRule<Uri>
{
    /// <summary>
    /// Determines whether the specified Uri uses the required port.
    /// </summary>
    /// <param name="value">The Uri to validate.</param>
    /// <returns><c>true</c> if the Uri uses the specified port; otherwise, <c>false</c>.</returns>
    public bool IsValid(Uri value) => value.Port == port;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"URI must use port {port}";
}

/// <summary>
/// A validation rule that ensures a Uri is absolute (has a scheme and host).
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public class AbsoluteUriRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <summary>
    /// Determines whether the specified Uri is absolute.
    /// </summary>
    /// <param name="value">The Uri to validate.</param>
    /// <returns><c>true</c> if the Uri is absolute; otherwise, <c>false</c>.</returns>
    public bool IsValid(Uri value) => value.IsAbsoluteUri;

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "URI must be absolute";
}

/// <summary>
/// A validation rule that ensures a Uri path starts with a specific prefix.
/// </summary>
/// <param name="pathPrefix">The required path prefix.</param>
/// <param name="customMessage">An optional custom error message.</param>
public class PathPrefixRule(string pathPrefix, string? customMessage = null) : IValidationRule<Uri>
{
    /// <summary>
    /// Determines whether the specified Uri path starts with the required prefix.
    /// </summary>
    /// <param name="value">The Uri to validate.</param>
    /// <returns><c>true</c> if the Uri path starts with the prefix; otherwise, <c>false</c>.</returns>
    public bool IsValid(Uri value) => value.AbsolutePath.StartsWith(pathPrefix, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? $"URI path must start with {pathPrefix}";
}

/// <summary>
/// A validation rule that ensures a Uri has a query string.
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public class HasQueryStringRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <summary>
    /// Determines whether the specified Uri has a query string.
    /// </summary>
    /// <param name="value">The Uri to validate.</param>
    /// <returns><c>true</c> if the Uri has a query string; otherwise, <c>false</c>.</returns>
    public bool IsValid(Uri value) => !string.IsNullOrEmpty(value.Query);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "URI must include a query string";
}

/// <summary>
/// A validation rule that ensures a Uri does not have a query string.
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public class NoQueryStringRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <summary>
    /// Determines whether the specified Uri does not have a query string.
    /// </summary>
    /// <param name="value">The Uri to validate.</param>
    /// <returns><c>true</c> if the Uri has no query string; otherwise, <c>false</c>.</returns>
    public bool IsValid(Uri value) => string.IsNullOrEmpty(value.Query);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "URI must not include a query string";
}

/// <summary>
/// A validation rule that ensures a Uri has a fragment (hash).
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public class HasFragmentRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <summary>
    /// Determines whether the specified Uri has a fragment.
    /// </summary>
    /// <param name="value">The Uri to validate.</param>
    /// <returns><c>true</c> if the Uri has a fragment; otherwise, <c>false</c>.</returns>
    public bool IsValid(Uri value) => !string.IsNullOrEmpty(value.Fragment);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "URI must include a fragment";
}

/// <summary>
/// A validation rule that ensures a Uri is a localhost address.
/// </summary>
/// <param name="customMessage">An optional custom error message.</param>
public class LocalhostRule(string? customMessage = null) : IValidationRule<Uri>
{
    /// <summary>
    /// Determines whether the specified Uri points to localhost.
    /// </summary>
    /// <param name="value">The Uri to validate.</param>
    /// <returns><c>true</c> if the Uri is localhost; otherwise, <c>false</c>.</returns>
    public bool IsValid(Uri value) =>
        value.IsLoopback ||
        value.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the error message that should be displayed when validation fails.
    /// </summary>
    public string errorMessage { get; } = customMessage ?? "URI must be a localhost address";
}

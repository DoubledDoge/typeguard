namespace TypeGuard.Core.Rules;

using Interfaces;

/// <summary>
/// A validation rule that ensures a URI uses the specified scheme (e.g., "http", "https", "ftp").
/// </summary>
/// <param name="scheme">The required URI scheme. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="scheme"/> is null or empty.</exception>
public class UriSchemeRule(string scheme, string? customMessage = null)
	: RulesBase<Uri>(BuildPredicate(scheme), $"URI must use the {scheme} scheme", customMessage)
{
	private static Func<Uri, bool> BuildPredicate(string scheme) =>
		string.IsNullOrEmpty(scheme)
			? throw new ArgumentException("Cannot be null or empty.", nameof(scheme))
			: v => v.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase);
}

/// <summary>
/// A validation rule that ensures a URI uses HTTPS only.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class HttpsOnlyRule(string? customMessage = null)
	: RulesBase<Uri>(
		v => v.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase),
		"URI must use HTTPS",
		customMessage
	);

/// <summary>
/// A validation rule that ensures a URI uses either HTTP or HTTPS.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class HttpOrHttpsRule(string? customMessage = null)
	: RulesBase<Uri>(
		v =>
			v.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase)
			|| v.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase),
		"URI must use HTTP or HTTPS",
		customMessage
	);

/// <summary>
/// A validation rule that ensures a URI belongs to the specified domain.
/// </summary>
/// <param name="domain">The required domain (e.g. "example.com"). Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="domain"/> is null or empty.</exception>
public class DomainRule(string domain, string? customMessage = null)
	: RulesBase<Uri>(BuildPredicate(domain), $"URI must be from domain {domain}", customMessage)
{
	private static Func<Uri, bool> BuildPredicate(string domain) =>
		string.IsNullOrEmpty(domain)
			? throw new ArgumentException("Cannot be null or empty.", nameof(domain))
			: v => v.Host.Equals(domain, StringComparison.OrdinalIgnoreCase);
}

/// <summary>
/// A validation rule that ensures a URI belongs to one of the specified allowed domains.
/// </summary>
/// <param name="allowedDomains">The collection of allowed domains. Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="allowedDomains"/> is null.</exception>
/// <exception cref="ArgumentException">Thrown when <paramref name="allowedDomains"/> is empty.</exception>
public class AllowedDomainsRule(IEnumerable<string> allowedDomains, string? customMessage = null)
	: IValidatorRule<Uri>
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
public class PortRule(int port, string? customMessage = null)
	: RulesBase<Uri>(BuildPredicate(port), $"URI must use port {port}", customMessage)
{
	private static Func<Uri, bool> BuildPredicate(int port) =>
		port is < 0 or > 65535
			? throw new ArgumentOutOfRangeException(
				nameof(port),
				port,
				"port must be between 0 and 65535."
			)
			: v => v.Port == port;
}

/// <summary>
/// A validation rule that ensures a URI is absolute (has a scheme and host).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class AbsoluteUriRule(string? customMessage = null)
	: RulesBase<Uri>(v => v.IsAbsoluteUri, "URI must be absolute", customMessage);

/// <summary>
/// A validation rule that ensures the URI path starts with the specified prefix.
/// </summary>
/// <param name="pathPrefix">The required path prefix (e.g. "/api/v1"). Cannot be null or empty.</param>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
/// <exception cref="ArgumentException">Thrown when <paramref name="pathPrefix"/> is null or empty.</exception>
public class PathPrefixRule(string pathPrefix, string? customMessage = null)
	: RulesBase<Uri>(
		BuildPredicate(pathPrefix),
		$"URI path must start with '{pathPrefix}'",
		customMessage
	)
{
	private static Func<Uri, bool> BuildPredicate(string pathPrefix) =>
		string.IsNullOrEmpty(pathPrefix)
			? throw new ArgumentException("Cannot be null or empty.", nameof(pathPrefix))
			: v => v.AbsolutePath.StartsWith(pathPrefix, StringComparison.OrdinalIgnoreCase);
}

/// <summary>
/// A validation rule that ensures the URI includes a query string.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class HasQueryStringRule(string? customMessage = null)
	: RulesBase<Uri>(
		v => !string.IsNullOrEmpty(v.Query),
		"URI must include a query string",
		customMessage
	);

/// <summary>
/// A validation rule that ensures the URI does not include a query string.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class NoQueryStringRule(string? customMessage = null)
	: RulesBase<Uri>(
		v => string.IsNullOrEmpty(v.Query),
		"URI must not include a query string",
		customMessage
	);

/// <summary>
/// A validation rule that ensures the URI includes a fragment (the portion after #).
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class HasFragmentRule(string? customMessage = null)
	: RulesBase<Uri>(
		v => !string.IsNullOrEmpty(v.Fragment),
		"URI must include a fragment",
		customMessage
	);

/// <summary>
/// A validation rule that ensures the URI is a localhost address.
/// </summary>
/// <param name="customMessage">An optional custom error message. If not provided, a default message is used.</param>
public class LocalhostRule(string? customMessage = null)
	: RulesBase<Uri>(
		v => v.IsLoopback || v.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase),
		"URI must be a localhost address",
		customMessage
	);

namespace TypeGuard.Core.Builders;

using Handlers;
using Interfaces;
using Rules;

/// <summary>
/// A fluent builder for constructing and configuring a URI input handler with validation rules.
/// Each <c>With*</c> method accumulates a rule onto the internal validator while the rules are evaluated
/// in the order they are added.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="uriKind">The kind of URI to accept. Defaults to <see cref="UriKind.Absolute"/>.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="validatorFactory">
/// An optional factory for creating the internal <see cref="UriHandler"/>.
/// Defaults to constructing a standard <see cref="UriHandler"/> from the provided providers.
/// </param>
public class UriInputBuilder(
	string prompt,
	UriKind uriKind,
	IInputProvider inputProvider,
	IOutputProvider outputProvider,
	Func<string, UriKind, IInputProvider, IOutputProvider, UriHandler>? validatorFactory = null
)
	: BuilderBase<Uri, UriInputBuilder>(
		(validatorFactory ?? ((p, u, i, o) => new UriHandler(i, o, p, u)))(
			prompt ?? throw new ArgumentNullException(nameof(prompt)),
			uriKind,
			inputProvider ?? throw new ArgumentNullException(nameof(inputProvider)),
			outputProvider ?? throw new ArgumentNullException(nameof(outputProvider))
		)
	)
{
	/// <summary>
	/// Adds a validation rule that ensures the URI uses the specified scheme.
	/// </summary>
	/// <param name="scheme">The required URI scheme (e.g. "https", "ftp"). Cannot be null or empty.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="scheme"/> is null or empty.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public UriInputBuilder WithScheme(string scheme, string? customMessage = null) =>
		string.IsNullOrEmpty(scheme)
			? throw new ArgumentException("Cannot be null or empty.", nameof(scheme))
			: AddRule(new UriSchemeRule(scheme, customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the URI uses HTTPS only.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public UriInputBuilder WithHttpsOnly(string? customMessage = null) =>
		AddRule(new HttpsOnlyRule(customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the URI uses HTTP or HTTPS.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public UriInputBuilder WithHttpOrHttps(string? customMessage = null) =>
		AddRule(new HttpOrHttpsRule(customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the URI belongs to the specified domain.
	/// </summary>
	/// <param name="domain">The required domain (e.g. "example.com"). Cannot be null or empty.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="domain"/> is null or empty.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public UriInputBuilder WithDomain(string domain, string? customMessage = null) =>
		string.IsNullOrEmpty(domain)
			? throw new ArgumentException("Cannot be null or empty.", nameof(domain))
			: AddRule(new DomainRule(domain, customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the URI belongs to one of the specified allowed domains.
	/// </summary>
	/// <param name="allowedDomains">The collection of allowed domains. Cannot be null or empty.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="allowedDomains"/> is null.</exception>
	/// <exception cref="ArgumentException">Thrown when <paramref name="allowedDomains"/> is empty.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public UriInputBuilder WithAllowedDomains(
		IEnumerable<string> allowedDomains,
		string? customMessage = null
	)
	{
		ArgumentNullException.ThrowIfNull(allowedDomains);

		IEnumerable<string> enumerable = allowedDomains.ToList();
		return !enumerable.Any()
			? throw new ArgumentException("Cannot be empty.", nameof(allowedDomains))
			: AddRule(new AllowedDomainsRule(enumerable, customMessage));
	}

	/// <summary>
	/// Adds a validation rule that ensures the URI uses the specified port.
	/// </summary>
	/// <param name="port">The required port number. Must be between 0 and 65535.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="port"/> is not between 0 and 65535.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public UriInputBuilder WithPort(int port, string? customMessage = null) =>
		port is < 0 or > 65535
			? throw new ArgumentOutOfRangeException(
				nameof(port),
				port,
				"port must be between 0 and 65535."
			)
			: AddRule(new PortRule(port, customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the URI is absolute.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public UriInputBuilder WithAbsoluteUri(string? customMessage = null) =>
		AddRule(new AbsoluteUriRule(customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the URI path starts with the specified prefix.
	/// </summary>
	/// <param name="pathPrefix">The required path prefix (e.g. "/api/v1"). Cannot be null or empty.</param>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="pathPrefix"/> is null or empty.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public UriInputBuilder WithPathPrefix(string pathPrefix, string? customMessage = null) =>
		string.IsNullOrEmpty(pathPrefix)
			? throw new ArgumentException("Cannot be null or empty.", nameof(pathPrefix))
			: AddRule(new PathPrefixRule(pathPrefix, customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the URI has a query string.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public UriInputBuilder WithQueryString(string? customMessage = null) =>
		AddRule(new HasQueryStringRule(customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the URI does not have a query string.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public UriInputBuilder WithoutQueryString(string? customMessage = null) =>
		AddRule(new NoQueryStringRule(customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the URI has a fragment.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public UriInputBuilder WithFragment(string? customMessage = null) =>
		AddRule(new HasFragmentRule(customMessage));

	/// <summary>
	/// Adds a validation rule that ensures the URI is a localhost address.
	/// </summary>
	/// <param name="customMessage">An optional custom error message.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public UriInputBuilder WithLocalhost(string? customMessage = null) =>
		AddRule(new LocalhostRule(customMessage));

	/// <summary>
	/// Adds a custom validation rule to the input handler.
	/// </summary>
	/// <param name="predicate">The function that determines whether a URI is valid. Cannot be null.</param>
	/// <param name="errorMessage">The error message to display when validation fails. Cannot be null.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> or <paramref name="errorMessage"/> is null.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	public UriInputBuilder WithCustomRule(Func<Uri, bool> predicate, string errorMessage)
	{
		ArgumentNullException.ThrowIfNull(predicate);
		ArgumentNullException.ThrowIfNull(errorMessage);

		return AddRule(new CustomRule<Uri>(predicate, errorMessage));
	}
}

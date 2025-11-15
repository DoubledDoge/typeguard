namespace TypeGuard.Core.Builders;

using Abstractions;
using Rules;
using Validators;

/// <summary>
/// A fluent builder for constructing and configuring a Uri validator with validation rules.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="uriKind">The type of URI to accept. Default is Absolute.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
public class UriBuilder(
    string prompt,
    UriKind uriKind,
    IInputProvider inputProvider,
    IOutputProvider outputProvider
)
{
    private readonly UriValidator _validator = new(inputProvider, outputProvider, prompt, uriKind);

    /// <summary>
    /// Adds a validation rule that ensures the Uri uses a specific scheme.
    /// </summary>
    /// <param name="scheme">The required URI scheme.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public UriBuilder WithScheme(string scheme, string? customMessage = null)
    {
        _validator.AddRule(new UriSchemeRule(scheme, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the Uri uses HTTPS only.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public UriBuilder WithHttpsOnly(string? customMessage = null)
    {
        _validator.AddRule(new HttpsOnlyRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the Uri uses HTTP or HTTPS.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public UriBuilder WithHttpOrHttps(string? customMessage = null)
    {
        _validator.AddRule(new HttpOrHttpsRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the Uri belongs to a specific domain.
    /// </summary>
    /// <param name="domain">The required domain.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public UriBuilder WithDomain(string domain, string? customMessage = null)
    {
        _validator.AddRule(new DomainRule(domain, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the Uri belongs to one of the allowed domains.
    /// </summary>
    /// <param name="allowedDomains">The collection of allowed domains.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public UriBuilder WithAllowedDomains(IEnumerable<string> allowedDomains, string? customMessage = null)
    {
        _validator.AddRule(new AllowedDomainsRule(allowedDomains, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the Uri uses a specific port.
    /// </summary>
    /// <param name="port">The required port number.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public UriBuilder WithPort(int port, string? customMessage = null)
    {
        _validator.AddRule(new PortRule(port, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the Uri is absolute.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public UriBuilder WithAbsoluteUri(string? customMessage = null)
    {
        _validator.AddRule(new AbsoluteUriRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the Uri path starts with a specific prefix.
    /// </summary>
    /// <param name="pathPrefix">The required path prefix.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public UriBuilder WithPathPrefix(string pathPrefix, string? customMessage = null)
    {
        _validator.AddRule(new PathPrefixRule(pathPrefix, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the Uri has a query string.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public UriBuilder WithQueryString(string? customMessage = null)
    {
        _validator.AddRule(new HasQueryStringRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the Uri does not have a query string.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public UriBuilder WithoutQueryString(string? customMessage = null)
    {
        _validator.AddRule(new NoQueryStringRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the Uri has a fragment.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public UriBuilder WithFragment(string? customMessage = null)
    {
        _validator.AddRule(new HasFragmentRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the Uri is a localhost address.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public UriBuilder WithLocalhost(string? customMessage = null)
    {
        _validator.AddRule(new LocalhostRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a custom validation rule to the validator.
    /// </summary>
    /// <param name="predicate">The function that determines whether a Uri is valid.</param>
    /// <param name="errorMessage">The error message to display when validation fails.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public UriBuilder WithCustomRule(Func<Uri, bool> predicate, string errorMessage)
    {
        _validator.AddRule(new CustomRule<Uri>(predicate, errorMessage));
        return this;
    }

    /// <summary>
    /// Asynchronously prompts the user for input and returns the validated Uri.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated Uri.</returns>
    public async Task<Uri> GetAsync(CancellationToken cancellationToken = default) =>
        await _validator.GetValidInputAsync(cancellationToken);

    /// <summary>
    /// Synchronously prompts the user for input and returns the validated Uri.
    /// </summary>
    /// <returns>The validated Uri.</returns>
    public Uri Get() => _validator.GetValidInput();
}

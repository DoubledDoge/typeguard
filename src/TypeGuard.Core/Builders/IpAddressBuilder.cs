namespace TypeGuard.Core.Builders;

using System.Net;
using Abstractions;
using Rules;
using Validators;

/// <summary>
/// A fluent builder for constructing and configuring an IP address validator with validation rules.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
public class IpAddressBuilder(
    string prompt,
    IInputProvider inputProvider,
    IOutputProvider outputProvider
)
{
    private readonly IpAddressValidator _validator = new(inputProvider, outputProvider, prompt);

    /// <summary>
    /// Adds a validation rule that ensures the IP address is IPv4.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IpAddressBuilder WithIPv4(string? customMessage = null)
    {
        _validator.AddRule(new Ipv4Rule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the IP address is IPv6.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IpAddressBuilder WithIPv6(string? customMessage = null)
    {
        _validator.AddRule(new Ipv6Rule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the IP address is private.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IpAddressBuilder WithPrivate(string? customMessage = null)
    {
        _validator.AddRule(new PrivateIpRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the IP address is an APIPA address.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IpAddressBuilder WithApipa(string? customMessage = null)
    {
        _validator.AddRule(new ApipaRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the IP address is not an APIPA address.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IpAddressBuilder WithoutApipa(string? customMessage = null)
    {
        _validator.AddRule(new NotApipaRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the IP address is a loopback address.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IpAddressBuilder WithLoopback(string? customMessage = null)
    {
        _validator.AddRule(new LoopbackIpRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the IP address is public.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IpAddressBuilder WithPublic(string? customMessage = null)
    {
        _validator.AddRule(new PublicIpRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the IP address is not a loopback address.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IpAddressBuilder WithoutLoopback(string? customMessage = null)
    {
        _validator.AddRule(new NotLoopbackIpRule(customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the IP address is within a specific subnet.
    /// </summary>
    /// <param name="network">The network address.</param>
    /// <param name="prefixLength">The subnet prefix length.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IpAddressBuilder WithSubnet(
        IPAddress network,
        int prefixLength,
        string? customMessage = null
    )
    {
        _validator.AddRule(new SubnetRule(network, prefixLength, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the IP address is in a list of allowed addresses.
    /// </summary>
    /// <param name="allowedAddresses">The collection of allowed IP addresses.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IpAddressBuilder WithAllowedAddresses(
        IEnumerable<IPAddress> allowedAddresses,
        string? customMessage = null
    )
    {
        _validator.AddRule(new AllowedIpAddressesRule(allowedAddresses, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a validation rule that ensures the IP address is not in a list of blocked addresses.
    /// </summary>
    /// <param name="blockedAddresses">The collection of blocked IP addresses.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IpAddressBuilder WithoutBlockedAddresses(
        IEnumerable<IPAddress> blockedAddresses,
        string? customMessage = null
    )
    {
        _validator.AddRule(new BlockedIpAddressesRule(blockedAddresses, customMessage));
        return this;
    }

    /// <summary>
    /// Adds a custom validation rule to the validator.
    /// </summary>
    /// <param name="predicate">The function that determines whether an IP address is valid.</param>
    /// <param name="errorMessage">The error message to display when validation fails.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public IpAddressBuilder WithCustomRule(Func<IPAddress, bool> predicate, string errorMessage)
    {
        _validator.AddRule(new CustomRule<IPAddress>(predicate, errorMessage));
        return this;
    }

    /// <summary>
    /// Asynchronously prompts the user for input and returns the validated IP address.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validated IP address.</returns>
    public async Task<IPAddress> GetAsync(CancellationToken cancellationToken = default) =>
        await _validator.GetValidInputAsync(cancellationToken);

    /// <summary>
    /// Synchronously prompts the user for input and returns the validated IP address.
    /// </summary>
    /// <returns>The validated IP address.</returns>
    public IPAddress Get() => _validator.GetValidInput();
}

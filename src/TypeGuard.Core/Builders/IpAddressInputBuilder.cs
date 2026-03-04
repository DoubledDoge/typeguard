using System.Net;

namespace TypeGuard.Core.Builders;

using Handlers;
using Interfaces;
using Rules;

/// <summary>
/// A fluent builder for constructing and configuring an IP address input handler with validation rules.
/// Each <c>With*</c> method accumulates a rule onto the internal validator while the rules are evaluated
/// in the order they are added.
/// </summary>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="validatorFactory">
/// An optional factory for creating the internal <see cref="IpAddressHandler"/>.
/// Defaults to constructing a standard <see cref="IpAddressHandler"/> from the provided providers.
/// </param>
public class IpAddressInputBuilder(
    string prompt,
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    Func<string, IInputProvider, IOutputProvider, IpAddressHandler>? validatorFactory = null
)
    : BuilderBase<IPAddress, IpAddressInputBuilder>(
        (validatorFactory ?? ((p, i, o) => new IpAddressHandler(i, o, p)))(
            prompt ?? throw new ArgumentNullException(nameof(prompt)),
            inputProvider ?? throw new ArgumentNullException(nameof(inputProvider)),
            outputProvider ?? throw new ArgumentNullException(nameof(outputProvider))
        )
    )
{
    /// <summary>
    /// Adds a validation rule that ensures the IP address is IPv4.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public IpAddressInputBuilder WithIPv4(string? customMessage = null) =>
        this.AddRule(new Ipv4Rule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the IP address is IPv6.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public IpAddressInputBuilder WithIPv6(string? customMessage = null) =>
        this.AddRule(new Ipv6Rule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the IP address is a private address.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public IpAddressInputBuilder WithPrivate(string? customMessage = null) =>
        this.AddRule(new PrivateIpRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the IP address is an APIPA address (169.254.0.0/16).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public IpAddressInputBuilder WithApipa(string? customMessage = null) =>
        this.AddRule(new ApipaRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the IP address is not an APIPA address (169.254.0.0/16).
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public IpAddressInputBuilder WithoutApipa(string? customMessage = null) =>
        this.AddRule(new NotApipaRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the IP address is a loopback address.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public IpAddressInputBuilder WithLoopback(string? customMessage = null) =>
        this.AddRule(new LoopbackIpRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the IP address is a public address.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public IpAddressInputBuilder WithPublic(string? customMessage = null) =>
        this.AddRule(new PublicIpRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the IP address is not a loopback address.
    /// </summary>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public IpAddressInputBuilder WithoutLoopback(string? customMessage = null) =>
        this.AddRule(new NotLoopbackIpRule(customMessage));

    /// <summary>
    /// Adds a validation rule that ensures the IP address falls within the specified subnet.
    /// </summary>
    /// <param name="network">The network address of the subnet. Cannot be null.</param>
    /// <param name="prefixLength">
    /// The subnet prefix length. Must be between 0 and 32 for IPv4 addresses, or 0 and 128 for IPv6 addresses.
    /// </param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="network"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="prefixLength"/> is not between 0 and 128.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public IpAddressInputBuilder WithSubnet(
        IPAddress network,
        int prefixLength,
        string? customMessage = null
    )
    {
        ArgumentNullException.ThrowIfNull(network);

        return prefixLength is < 0 or > 128
            ? throw new ArgumentOutOfRangeException(
                nameof(prefixLength),
                prefixLength,
                "prefixLength must be between 0 and 128."
            )
            : this.AddRule(new SubnetRule(network, prefixLength, customMessage));
    }

    /// <summary>
    /// Adds a validation rule that ensures the IP address is in the specified collection of allowed addresses.
    /// </summary>
    /// <param name="allowedAddresses">The collection of allowed IP addresses. Cannot be null or empty.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="allowedAddresses"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="allowedAddresses"/> is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public IpAddressInputBuilder WithAllowedAddresses(
        IEnumerable<IPAddress> allowedAddresses,
        string? customMessage = null
    )
    {
        ArgumentNullException.ThrowIfNull(allowedAddresses);

        IEnumerable<IPAddress> enumerable = allowedAddresses.ToList();
        return !enumerable.Any()
            ? throw new ArgumentException("Cannot be empty.", nameof(allowedAddresses))
            : this.AddRule(new AllowedIpAddressesRule(enumerable, customMessage));
    }

    /// <summary>
    /// Adds a validation rule that ensures the IP address is not in the specified collection of blocked addresses.
    /// </summary>
    /// <param name="blockedAddresses">The collection of blocked IP addresses. Cannot be null or empty.</param>
    /// <param name="customMessage">An optional custom error message.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="blockedAddresses"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="blockedAddresses"/> is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public IpAddressInputBuilder WithoutBlockedAddresses(
        IEnumerable<IPAddress> blockedAddresses,
        string? customMessage = null
    )
    {
        ArgumentNullException.ThrowIfNull(blockedAddresses);

        IEnumerable<IPAddress> enumerable = blockedAddresses.ToList();
        return !enumerable.Any()
            ? throw new ArgumentException("Cannot be empty.", nameof(blockedAddresses))
            : this.AddRule(new BlockedIpAddressesRule(enumerable, customMessage));
    }

    /// <summary>
    /// Adds a custom validation rule to the handler.
    /// </summary>
    /// <param name="predicate">The function that determines whether an IP address is valid. Cannot be null.</param>
    /// <param name="errorMessage">The error message to display when validation fails. Cannot be null.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> or <paramref name="errorMessage"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
    public IpAddressInputBuilder WithCustomRule(
        Func<IPAddress, bool> predicate,
        string errorMessage
    )
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(errorMessage);

        return this.AddRule(new CustomRule<IPAddress>(predicate, errorMessage));
    }
}

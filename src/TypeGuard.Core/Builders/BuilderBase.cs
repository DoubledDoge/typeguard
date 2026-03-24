namespace TypeGuard.Core.Builders;

using Handlers;
using Interfaces;

/// <summary>
/// Provides a base implementation for fluent input builders, managing the validator rule list,
/// the freeze-after-get contract, and the terminal <see cref="Get"/> and <see cref="GetAsync"/> methods.
/// </summary>
/// <remarks>
/// Calling <see cref="Get"/> or <see cref="GetAsync"/> permanently freezes this builder.
/// Any subsequent call to <see cref="AddRule"/> will throw <see cref="InvalidOperationException"/>.
/// Create a new builder instance if you need to reconfigure and re-prompt.
/// </remarks>
/// <typeparam name="T">The type of value this builder validates and returns.</typeparam>
/// <typeparam name="TSelf">The concrete builder type. Used to return the correct type from <see cref="AddRule"/> for fluent chaining.</typeparam>
/// <param name="handler">The input handler instance that accumulates and evaluates rules.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is null.</exception>
public abstract class BuilderBase<T, TSelf>(HandlerBase<T> handler)
	where TSelf : BuilderBase<T, TSelf>
{
	private readonly HandlerBase<T> _handler =
		handler ?? throw new ArgumentNullException(nameof(handler));

	private bool _frozen;

	/// <summary>
	/// Ensures the builder has not been frozen, then adds <paramref name="rule"/> to the handler.
	/// </summary>
	/// <param name="rule">The rule to add. Cannot be null.</param>
	/// <returns>The current builder instance for method chaining.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="rule"/> is null.</exception>
	/// <exception cref="InvalidOperationException">Thrown if the builder has been frozen.</exception>
	protected TSelf AddRule(IValidatorRule<T> rule)
	{
		ArgumentNullException.ThrowIfNull(rule);

		if (_frozen)
		{
			throw new InvalidOperationException(
				"Rules cannot be added after Get() or GetAsync() has been called. Create a new instance to reconfigure."
			);
		}

		_handler.AddRule(rule);
		return (TSelf)this;
	}

	/// <summary>
	/// Prompts the user for input, validates it against all configured rules, and returns the
	/// validated value.
	/// </summary>
	/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the handled value.</returns>
	public async Task<T> GetAsync(CancellationToken cancellationToken = default)
	{
		_frozen = true;
		return await _handler.GetValidInputAsync(cancellationToken);
	}

	/// <summary>
	/// Prompts the user for input, validates it against all configured rules, and returns the
	/// validated value.
	/// </summary>
	/// <remarks>
	/// This method blocks the calling thread synchronously. Avoid calling this from a context
	/// that has a synchronization context (such as ASP.NET or UI threads), as it may cause a
	/// deadlock. Prefer <see cref="GetAsync"/> in async contexts.
	/// </remarks>
	/// <returns>The handled value.</returns>
	public T Get()
	{
		_frozen = true;
		return _handler.GetValidInput();
	}
}

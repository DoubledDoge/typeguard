namespace TypeGuard.Core.Handlers;

using Interfaces;

/// <summary>
/// An input handler that prompts for and validates GUID input.
/// </summary>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
public class GuidHandler(
	IInputProvider inputProvider,
	IOutputProvider outputProvider,
	string prompt
) : HandlerBase<Guid>(inputProvider, outputProvider, prompt)
{
	/// <inheritdoc cref="HandlerBase{T}.TryParse"/>
	/// <returns><c>true</c> if the input is a valid GUID; otherwise, <c>false</c>.</returns>
	protected override bool TryParse(string? input, out Guid value, out string? errorMessage)
	{
		if (Guid.TryParse(input, out value))
		{
			errorMessage = null;
			return true;
		}

		value = Guid.Empty;
		errorMessage = "Please enter a valid GUID (e.g., 3f2504e0-4f89-11d3-9a0c-0305e82c3301).";
		return false;
	}
}

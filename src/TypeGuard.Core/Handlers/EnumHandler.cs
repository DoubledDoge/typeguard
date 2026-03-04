namespace TypeGuard.Core.Handlers;

using Interfaces;

/// <summary>
/// An input handler that prompts for and validates enum input.
/// Accepts enum values by name or by numeric value.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="ignoreCase">If true, enum name parsing is case-insensitive. Defaults to true.</param>
public class EnumHandler<TEnum>(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt,
    bool ignoreCase = true
) : HandlerBase<TEnum>(inputProvider, outputProvider, prompt)
    where TEnum : struct, Enum
{
    /// <inheritdoc cref="HandlerBase{T}.TryParse"/>
    /// <returns><c>true</c> if the input is a valid <typeparamref name="TEnum"/> value by name or numeric value; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out TEnum value, out string? errorMessage)
    {
        if (Enum.TryParse(input, ignoreCase, out value))
        {
            errorMessage = null;
            return true;
        }

        value = default;
        errorMessage =
            $"Please enter a valid {typeof(TEnum).Name}. "
            + $"Valid values: {string.Join(", ", Enum.GetNames<TEnum>())}";
        return false;
    }
}

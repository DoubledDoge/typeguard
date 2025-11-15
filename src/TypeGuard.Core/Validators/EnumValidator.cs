namespace TypeGuard.Core.Validators;

using Abstractions;

/// <summary>
/// A validator that prompts for and validates enum input.
/// Accepts enum values by name or by numeric value.
/// </summary>
/// <typeparam name="TEnum">The enum type to validate.</typeparam>
/// <param name="inputProvider">The provider used to read user input.</param>
/// <param name="outputProvider">The provider used to display prompts and error messages.</param>
/// <param name="prompt">The prompt message to display to the user when requesting input.</param>
/// <param name="ignoreCase">If true, enum name parsing is case-insensitive. Default is true.</param>
public class EnumValidator<TEnum>(
    IInputProvider inputProvider,
    IOutputProvider outputProvider,
    string prompt,
    bool ignoreCase = true
) : ValidatorBase<TEnum>(inputProvider, outputProvider, prompt)
    where TEnum : struct, Enum
{
    /// <summary>
    /// Attempts to parse the raw user input into an enum value. (Overrides <see cref="ValidatorBase{T}.TryParse"/>)
    /// </summary>
    /// <param name="input">The raw input string from the user.</param>
    /// <param name="value">When this method returns, contains the parsed enum value if parsing succeeded, or the default enum value if parsing failed.</param>
    /// <param name="errorMessage">When this method returns, contains the error message if parsing failed, or null if parsing succeeded.</param>
    /// <returns><c>true</c> if the input is a valid enum value; otherwise, <c>false</c>.</returns>
    protected override bool TryParse(string? input, out TEnum value, out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            errorMessage = $"Please enter a valid {typeof(TEnum).Name}";
            value = default(TEnum);
            return false;
        }

        if (Enum.TryParse(input, ignoreCase, out value))
        {
            errorMessage = null;
            return true;
        }

        string validValues = string.Join(", ", Enum.GetNames(typeof(TEnum)));
        errorMessage = $"Please enter a valid {typeof(TEnum).Name}. Valid values: {validValues}";
        value = default(TEnum);
        return false;
    }
}

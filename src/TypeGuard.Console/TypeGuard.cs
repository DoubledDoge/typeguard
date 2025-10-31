namespace TypeGuard.Console;

using Core.Builders;
using Core.Validators;

public static class TypeGuard
{
    private static readonly ConsoleInput DefaultInputProvider = new();
    private static readonly ConsoleOutput DefaultOutputProvider = new();

    public static async Task<int> GetIntAsync(
        string prompt,
        CancellationToken cancellationToken = default
    )
    {
        IntValidator validator = new(
            DefaultInputProvider,
            DefaultOutputProvider,
            prompt
        );
        return await validator.GetValidInputAsync(cancellationToken);
    }

    public static int GetInt(string prompt)
    {
        IntValidator validator = new(
            DefaultInputProvider,
            DefaultOutputProvider,
            prompt
        );
        return validator.GetValidInput();
    }

    public static string GetString(string prompt)
    {
        StringValidator validator = new(
            DefaultInputProvider,
            DefaultOutputProvider,
            prompt
        );
        return validator.GetValidInput();
    }

    public static IntValidatorBuilder ForInt(string prompt) =>
        new(prompt, DefaultInputProvider, DefaultOutputProvider);

    public static StringValidatorBuilder ForString(string prompt) =>
        new(prompt, DefaultInputProvider, DefaultOutputProvider);

    public static DateTimeValidatorBuilder ForDateTime(string prompt, string? format = null) =>
        new(prompt, format, DefaultInputProvider, DefaultOutputProvider);
}

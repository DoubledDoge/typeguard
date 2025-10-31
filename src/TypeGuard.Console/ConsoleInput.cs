namespace TypeGuard.Console;

using Core.Abstractions;

public class ConsoleInput : IInputProvider
{
    public async Task<string?> GetInputAsync(CancellationToken cancellationToken = default) =>
        await Task.Run(() => System.Console.ReadLine()?.Trim(), cancellationToken);

    public string? GetInput() => System.Console.ReadLine()?.Trim();
}

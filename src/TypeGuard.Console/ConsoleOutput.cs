using ConsolePrism.Core;

namespace TypeGuard.Console;

using Core.Abstractions;
using Console = System.Console;

public class ConsoleOutput : IOutputProvider
{
    public async Task DisplayPromptAsync(
        string message,
        CancellationToken cancellationToken = default
    ) => await Task.Run(() => Console.Write($"{message}: "), cancellationToken);

    public async Task DisplayErrorAsync(
        string message,
        CancellationToken cancellationToken = default
    ) =>
        await Task.Run(
            () =>
            {
                ColorWriter.WriteError($"\n{message}");
                Console.Write("Press Enter to try again... ");
                Console.ReadLine();
                Console.WriteLine();
            },
            cancellationToken
        );

    public void DisplayPrompt(string message) => Console.Write($"{message}: ");

    public void DisplayError(string message)
    {
        ColorWriter.WriteError($"\n{message}");
        Console.Write("Press Enter to try again... ");
        Console.ReadLine();
        Console.WriteLine();
    }
}

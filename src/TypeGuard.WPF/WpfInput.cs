using System.Windows.Controls;

namespace TypeGuard.Wpf;

using Core.Interfaces;

/// <summary>
/// Provides WPF-based input by reading the current text value from a <see cref="TextBox"/>.
/// </summary>
/// <param name="textBox">The text box to read input from. Cannot be null.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="textBox"/> is null.</exception>
public class WpfInput(TextBox textBox) : IInputProvider
{
    private readonly TextBox _textBox = textBox ?? throw new ArgumentNullException(nameof(textBox));

    /// <summary>
    /// Asynchronously reads the current text value from the text box, trimming any leading or
    /// trailing whitespace.
    /// </summary>
    /// <remarks>
    /// Reading <see cref="TextBox.Text"/> is synchronous and instant, so this method wraps the
    /// result in a completed task via <see cref="Task.FromResult{TResult}"/> without consuming
    /// a thread pool thread.
    /// </remarks>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the trimmed text value of the text box, or null if the text box has no text.</returns>
    public Task<string?> GetInputAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(GetInput());

    /// <summary>
    /// Synchronously reads the current text value from the text box, trimming any leading or
    /// trailing whitespace.
    /// </summary>
    /// <remarks>
    /// If called from a thread other than the UI thread, the read is marshalled to the UI thread
    /// via <see cref="System.Windows.Threading.Dispatcher.Invoke{TResult}(Func{TResult})"/>.
    /// </remarks>
    /// <returns>The trimmed text value of the text box, or null if the text box has no text.</returns>
    public string? GetInput()
    {
        string? text = _textBox.Dispatcher.CheckAccess()
            ? _textBox.Text
            : _textBox.Dispatcher.Invoke(() => _textBox.Text);

        return string.IsNullOrEmpty(text) ? null : text.Trim();
    }
}

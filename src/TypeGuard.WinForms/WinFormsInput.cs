namespace TypeGuard.WinForms;

using Core.Interfaces;

/// <summary>
/// Provides WinForms-based input by reading the current text value from a <see cref="Control"/>.
/// </summary>
/// <remarks>
/// Any <see cref="Control"/> that exposes user-entered text via its <see cref="Control.Text"/>
/// property is supported, including <see cref="TextBox"/>, <see cref="ComboBox"/>,
/// and <see cref="RichTextBox"/>.
/// </remarks>
/// <param name="control">The control to read input from. Cannot be null.</param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="control"/> is null.</exception>
public class WinFormsInput(Control control) : IInputProvider
{
    private readonly Control _control = control ?? throw new ArgumentNullException(nameof(control));

    /// <summary>
    /// Asynchronously reads the current text value from the control, trimming any leading or
    /// trailing whitespace.
    /// </summary>
    /// <remarks>
    /// This method marshals the read back to the UI thread via
    /// Control.Invoke to ensure thread-safe access to the control's
    /// <see cref="Control.Text"/> property.
    /// </remarks>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the trimmed text value of the control, or null if the control has no text.</returns>
    public Task<string?> GetInputAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(GetInput());

    /// <summary>
    /// Synchronously reads the current text value from the control, trimming any leading or
    /// trailing whitespace.
    /// </summary>
    /// <remarks>
    /// If called from a thread other than the UI thread, the read is marshaled to the UI thread
    /// via Control.Invoke"/>.
    /// </remarks>
    /// <returns>The trimmed text value of the control, or null if the control has no text.</returns>
    public string? GetInput()
    {
        string? text = _control.InvokeRequired
            ? (string?)_control.Invoke(() => _control.Text)
            : _control.Text;

        return string.IsNullOrEmpty(text) ? null : text.Trim();
    }
}

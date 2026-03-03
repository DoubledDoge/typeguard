namespace TypeGuard.Maui;

using Core.Interfaces;

/// <summary>
/// Provides MAUI-based input by reading from an <see cref="Entry"/> control, optionally
/// advancing only when a submit <see cref="Button"/> is tapped.
/// </summary>
/// <remarks>
/// If a submit button is provided, GetInputAsync awaits a button tap before reading the
/// entry value.
/// If no submit button is provided, GetInputAsync awaits the Entry.Completed event, which
/// fires when the user presses the return key on the keyboard.
/// </remarks>
/// <param name="entry">The entry control to read input from. Cannot be null.</param>
/// <param name="submitButton">
/// An optional button that the user taps to submit their input. If null, input is read on
/// the Entry.Completed event instead.
/// </param>
/// <exception cref="ArgumentNullException">Thrown when <paramref name="entry"/> is null.</exception>
public class MauiInput(Entry entry, Button? submitButton = null) : IInputProvider
{
    private readonly Entry _entry = entry ?? throw new ArgumentNullException(nameof(entry));

    /// <summary>
    /// Asynchronously waits for the user to submit input via button tap or keyboard return,
    /// then reads and returns the trimmed entry value.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the trimmed entry value, or null if the entry has no text.</returns>
    public async Task<string?> GetInputAsync(CancellationToken cancellationToken = default)
    {
        TaskCompletionSource<string?> tcs = new();

        await using CancellationTokenRegistration registration = cancellationToken.Register(() =>
            tcs.TrySetCanceled()
        );

        if (submitButton is not null)
        {
            void OnButtonClicked(object? sender, EventArgs e) => tcs.TrySetResult(GetInput());

            submitButton.Clicked += OnButtonClicked;
            try
            {
                return await tcs.Task;
            }
            finally
            {
                submitButton.Clicked -= OnButtonClicked;
            }
        }

        void OnEntryCompleted(object? sender, EventArgs e) => tcs.TrySetResult(GetInput());

        _entry.Completed += OnEntryCompleted;
        try
        {
            return await tcs.Task;
        }
        finally
        {
            _entry.Completed -= OnEntryCompleted;
        }
    }

    /// <summary>
    /// Synchronously reads the current text value from the entry, trimming any leading or
    /// trailing whitespace.
    /// </summary>
    /// <remarks>
    /// If called from a thread other than the main thread, the read is marshalled via
    /// MainThread.InvokeOnMainThreadAsync. Prefer GetInputAsync in all MAUI contexts
    /// to avoid blocking the main thread.
    /// </remarks>
    /// <returns>The trimmed entry value, or null if the entry has no text.</returns>
    public string? GetInput()
    {
        string? text = MainThread.IsMainThread
            ? _entry.Text
            : MainThread.InvokeOnMainThreadAsync(() => _entry.Text).GetAwaiter().GetResult();

        return string.IsNullOrEmpty(text) ? null : text.Trim();
    }
}

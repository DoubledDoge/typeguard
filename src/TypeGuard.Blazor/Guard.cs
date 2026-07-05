namespace TypeGuard.Blazor;

using Core;

/// <summary>
///     Provides a concise API for creating configured input builders for Blazor applications.
/// </summary>
/// <remarks>
///     <para>
///         Register this class as a scoped service in your DI container so that each Blazor circuit
///         (Server) or application instance (WebAssembly) gets its own isolated validation state:
///     </para>
///     <code>
/// builder.Services.AddScoped&lt;Guard&gt;();
/// </code>
///     <para>
///         In your component, inject the guard, bind <see cref="Input" /> and <see cref="Output" />
///         properties to your markup, and register <c>StateHasChanged</c> as the re-render callback:
///     </para>
///     <code>
/// @inject Guard Guard
///
/// &lt;InputText @bind-Value="Guard.Input.Value" /&gt;
/// &lt;p&gt;@Guard.Output.PromptMessage&lt;/p&gt;
/// &lt;p style="color: red"&gt;@Guard.Output.ErrorMessage&lt;/p&gt;
///
/// @code {
///     protected override void OnInitialized()
///     {
///         Guard.Output.OnStateChanged = StateHasChanged;
///     }
///
///     private async Task SubmitAsync()
///     {
///         int age = await Guard.Int("Enter your age")
///             .WithRange(1, 120)
///             .GetAsync();
///     }
/// }
/// </code>
///     <para>
///         Only <see cref="Core.Builders.BuilderBase{T,TSelf}.GetAsync" /> is supported in Blazor.
///         Calling <see cref="Core.Builders.BuilderBase{T,TSelf}.Get" /> is not exposed as blocking
///         the Blazor rendering thread, particularly on WebAssembly, will freeze the UI entirely.
///     </para>
/// </remarks>
public sealed class Guard() : GuardBase<BlazorInput, BlazorOutput>(new(), new())
{
	/// <summary>
	///     Gets the input provider that holds the current bound input value.
	///     Bind <see cref="BlazorInput.Value" /> to the input control in markup.
	/// </summary>
	public new BlazorInput Input => base.Input;

	/// <summary>
	///     Gets the output provider that holds the current prompt and error messages.
	///     Bind <see cref="BlazorOutput.PromptMessage" /> and <see cref="BlazorOutput.ErrorMessage" />
	///     in markup, and set <see cref="BlazorOutput.OnStateChanged" /> to <c>StateHasChanged</c>
	///     in <c>OnInitialized</c>.
	/// </summary>
	public new BlazorOutput Output => base.Output;
}
